using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using TrajectoryFinder2D.Commands;
using TrajectoryFinder2D.Models;

namespace TrajectoryFinder2D.ViewModels
{
    internal class DataGeneratorViewModel : TrajectoryFinderViewModelBase
    {
        private const double Velocity = 1e6;

        private readonly PointGenerator _pointGenerator;

        private readonly List<IReadOnlyList<double>> _tickTimes;

        private bool _isShapeCaptured;

        private Point _panelMousePosition;

        private readonly Point _previousPanelMousePosition;

        public RelayCommand<ShapeWithLeftTopCornerBase> PreviewMouseMove { get; }

        public RelayCommand LeftMouseButtonUp { get; }

        public RelayCommand LeftMouseButtonDown { get; }

        public Point PanelMousePosition
        {
            get => _panelMousePosition;
            set => SetProperty(ref _panelMousePosition, value);
        }

        public DataGeneratorViewModel(Action backAction)
            : base(backAction)
        {
            _pointGenerator = new PointGenerator(3, 3);

            _tickTimes = new List<IReadOnlyList<double>>();

            _previousPanelMousePosition = new Point { X = -1, Y = -1 };

            bool isCanMoveShape() => TickCount == 0;

            LeftMouseButtonDown = new RelayCommand(
                _ =>
                {
                    _isShapeCaptured = true;
                    SavePreviousPanelMousePosition();
                },
                _ => isCanMoveShape());

            LeftMouseButtonUp = new RelayCommand(
                _ => _isShapeCaptured = false,
                _ => isCanMoveShape());

            PreviewMouseMove = new RelayCommand<ShapeWithLeftTopCornerBase>(
                shape =>
                {
                    if (!_isShapeCaptured)
                        return;
                    shape.Left += _panelMousePosition.X - _previousPanelMousePosition.X;
                    shape.Top += _panelMousePosition.Y - _previousPanelMousePosition.Y;
                    SavePreviousPanelMousePosition();
                },
                _ => isCanMoveShape());
        }

        public async Task Save()
        {
            var result = await _saveFileDialog.ShowAsync(new Window());
            if (result != null)
            {
                var sb = new StringBuilder();

                for (var i = 0; i < _circles.Count - 1; ++i)
                    sb.Append(string.Concat(
                        _circles[i].Center.X.ToString(CultureInfo.InvariantCulture),
                        ", ",
                        _circles[i].Center.Y.ToString(CultureInfo.InvariantCulture),
                        ", "));
                sb.AppendLine(string.Concat(
                    _circles[_circles.Count - 1].Center.X.ToString(CultureInfo.InvariantCulture),
                    ", ",
                    _circles[_circles.Count - 1].Center.Y.ToString(CultureInfo.InvariantCulture)));

                foreach (var time in _tickTimes)
                    sb.AppendLine(string.Concat(
                        time[0].ToString(CultureInfo.InvariantCulture),
                        ", ",
                        time[1].ToString(CultureInfo.InvariantCulture),
                        ", ",
                        time[2].ToString(CultureInfo.InvariantCulture)));

                await File.WriteAllTextAsync(result, sb.ToString());
            }
        }

        protected override bool TryTick()
        {
            var newPoint = _pointGenerator.GetNewPoint(_square.Center);
            _square.Center = newPoint;
            _polyLine.AddPoint(newPoint);

            var times = new List<double>(_circles.Count);
            foreach (var circle in _circles)
            {
                var dx = circle.Center.X - newPoint.X;
                var dy = circle.Center.Y - newPoint.Y;
                circle.Radius = Math.Sqrt(dx * dx + dy * dy);
                times.Add(circle.Radius / Velocity);
            }
            _tickTimes.Add(times);

            if (TickCount == 0)
                ShapeCollection.Add(_polyLine);

            return true;
        }

        private void SavePreviousPanelMousePosition()
        {
            _previousPanelMousePosition.X = _panelMousePosition.X;
            _previousPanelMousePosition.Y = _panelMousePosition.Y;
        }
    }
}
