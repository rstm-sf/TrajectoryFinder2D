using TrajectoryFinder2D.Commands;
using TrajectoryFinder2D.Models;

namespace TrajectoryFinder2D.ViewModels
{
    internal class MainWindowViewModel : ObservableObject
    {
        bool _isShapeCaptured;

        private Point _panelMousePosition;

        private readonly Point _previousPanelMousePosition;

        private Circle _circle;

        public RelayCommand<Circle> PreviewMouseMove { get; }

        public RelayCommand LeftMouseButtonUp { get; }

        public RelayCommand LeftMouseButtonDown { get; }

        public Point PanelMousePosition
        {
            get => _panelMousePosition;
            set => SetProperty(ref _panelMousePosition, value);
        }

        public Circle Circle
        {
            get => _circle;
            set => SetProperty(ref _circle, value);
        }

        public MainWindowViewModel()
        {
            var radius = 50;
            Circle = new Circle(radius, new Point { X = radius, Y = radius });

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

        private void SavePreviousPanelMousePosition()
        {
            _previousPanelMousePosition.X = _panelMousePosition.X;
            _previousPanelMousePosition.Y = _panelMousePosition.Y;
        }
    }
}
