using System;
using TrajectoryFinder2D.Commands;
using TrajectoryFinder2D.Models;

namespace TrajectoryFinder2D.ViewModels
{
    internal class DataGeneratorViewModel : TrajectoryFinderViewModelBase
    {
        private readonly PointGenerator _pointGenerator;

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

        public DataGeneratorViewModel() : base(10)
        {
            _pointGenerator = new PointGenerator(3, 3);

            _previousPanelMousePosition = new Point { X = -1, Y = -1 };

            LeftMouseButtonDown = new RelayCommand(
                _ =>
                {
                    _isShapeCaptured = true;
                    SavePreviousPanelMousePosition();
                });

            LeftMouseButtonUp = new RelayCommand(_ => _isShapeCaptured = false);

            PreviewMouseMove = new RelayCommand<ShapeWithLeftTopCornerBase>(
                shape =>
                {
                    if (!_isShapeCaptured)
                        return;
                    shape.Left += _panelMousePosition.X - _previousPanelMousePosition.X;
                    shape.Top += _panelMousePosition.Y - _previousPanelMousePosition.Y;
                    SavePreviousPanelMousePosition();
                });

            var radius = 50;
            var y = radius;
            foreach (var circle in _circles)
            {
                circle.Radius = radius;
                circle.Center = new Point { X = radius, Y = y };
                y += 2 * radius + 10;
            }

            _square.Center = new Point { X = radius, Y = y };
        }

        public void Save() { }

        protected override bool TryTick()
        {
            var newPoint = _pointGenerator.GetNewPoint(_square.Center);
            _square.Center = newPoint;
            _polyLine.AddPoint(newPoint);

            foreach (var circle in _circles)
            {
                var dx = circle.Center.X - newPoint.X;
                var dy = circle.Center.Y - newPoint.Y;
                circle.Radius = Math.Sqrt(dx * dx + dy * dy);
            }

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
