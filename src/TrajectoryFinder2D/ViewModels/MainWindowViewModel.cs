using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using TrajectoryFinder2D.Commands;
using TrajectoryFinder2D.Models;
using TrajectoryFinder2D.Utils;

namespace TrajectoryFinder2D.ViewModels
{
    internal class MainWindowViewModel : TrajectoryFinderViewModelBase
    {
        private const double Scale = 6;

        private readonly Point Offset = new Point { X = 200, Y = 200, };

        private bool _isShapeCaptured;

        private Point _panelMousePosition;

        private readonly Point _previousPanelMousePosition;

        private readonly TravelStarts _travelStarts;

        public RelayCommand<Circle> PreviewMouseMove { get; }

        public RelayCommand LeftMouseButtonUp { get; }

        public RelayCommand LeftMouseButtonDown { get; }

        public Point PanelMousePosition
        {
            get => _panelMousePosition;
            set => SetProperty(ref _panelMousePosition, value);
        }

        public MainWindowViewModel()
        {
            _travelStarts = new TravelStarts();

            for (var i = 0; i < _circles.Count; ++i)
            {
                _circles[i].Radius = 50;
                _circles[i].Center = ConvertToViewPoint(_travelStarts.Points[i]);
            }

            _previousPanelMousePosition = new Point { X = -1, Y = -1 };

            LeftMouseButtonDown = new RelayCommand(
                _ =>
                {
                    _isShapeCaptured = true;
                    SavePreviousPanelMousePosition();
                });

            LeftMouseButtonUp = new RelayCommand(_ => _isShapeCaptured = false);

            PreviewMouseMove = new RelayCommand<Circle>(
                circle =>
                {
                    if (!_isShapeCaptured)
                        return;
                    circle.Left += _panelMousePosition.X - _previousPanelMousePosition.X;
                    circle.Top += _panelMousePosition.Y - _previousPanelMousePosition.Y;
                    SavePreviousPanelMousePosition();
                });
        }

        public async Task Save()
        {
            var result = await _saveFileDialog.ShowAsync(new Window());
            if (result != null)
            {
                var sb = new StringBuilder();
                foreach (var point in _polyLine.Points)
                {
                    var x = (point.X - Offset.X) / Scale;
                    var y = (point.Y - Offset.Y) / Scale;
                    sb.AppendLine(x + ", " + y);
                }

                await File.WriteAllTextAsync(result, sb.ToString());
            }
        }

        private void SavePreviousPanelMousePosition()
        {
            _previousPanelMousePosition.X = _panelMousePosition.X;
            _previousPanelMousePosition.Y = _panelMousePosition.Y;
        }

        protected override bool TryTick()
        {
            if (TickCount == _travelStarts.ToPointTimes.Count)
            {
                for (int i = 0; i < _circles.Count + 1; i++)
                    ShapeCollection.RemoveAt(0);
                return false;
            }

            var times = _travelStarts.ToPointTimes[TickCount];
            for (var i = 0; i < _circles.Count; ++i)
                _circles[i].Radius = ConvertToViewRadius(times[i] * _travelStarts.Velocity);

            if (MathHelper.TryFindThreeCircleIntersection(
                _circles[0], _circles[1], _circles[2], out var point))
            {
                _square.Center = point;
                _polyLine.AddPoint(point);
                if (ShapeCollection.Count == _circles.Count)
                {
                    ShapeCollection.Add(_square);
                    ShapeCollection.Add(_polyLine);
                }
            }

            return true;
        }

        private Point ConvertToViewPoint(Point point) =>
            new Point
            {
                X = point.X * Scale + Offset.X,
                Y = point.Y * Scale + Offset.Y,
            };

        private double ConvertToViewRadius(double radius) =>
            radius * Scale;
    }
}
