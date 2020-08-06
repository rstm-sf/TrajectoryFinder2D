using TrajectoryFinder2D.Commands;
using TrajectoryFinder2D.Models;

namespace TrajectoryFinder2D.ViewModels
{
    internal class DataGeneratorViewModel : TrajectoryFinderViewModelBase
    {
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

        public DataGeneratorViewModel()
        {
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

        protected override bool TryTick()
        {
            return false;
        }

        private void SavePreviousPanelMousePosition()
        {
            _previousPanelMousePosition.X = _panelMousePosition.X;
            _previousPanelMousePosition.Y = _panelMousePosition.Y;
        }
    }
}
