using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrajectoryFinder2D.Commands;
using TrajectoryFinder2D.Models;

namespace TrajectoryFinder2D.ViewModels
{
    internal class MainWindowViewModel : ObservableObject
    {
        bool _isShapeCaptured;

        private Point _panelMousePosition;

        private readonly Point _previousPanelMousePosition;

        public RelayCommand<Circle> PreviewMouseMove { get; }

        public RelayCommand LeftMouseButtonUp { get; }

        public RelayCommand LeftMouseButtonDown { get; }

        public Point PanelMousePosition
        {
            get => _panelMousePosition;
            set => SetProperty(ref _panelMousePosition, value);
        }

        public ObservableCollection<VisualShape> VisualShapeCollection { get; set; }

        public MainWindowViewModel()
        {
            VisualShapeCollection = CreateVisualShapeCollection();

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

        private static ObservableCollection<VisualShape> CreateVisualShapeCollection()
        {
            const int circleCount = 3;
            const int radius = 50;

            var y = radius;
            var result = new List<Circle>(circleCount);
            for (var i = 0; i < circleCount; ++i)
            {
                result.Add(new Circle(radius, new Point { X = radius, Y = y })
                {
                    FillColor = new Avalonia.Media.SolidColorBrush(
                        Avalonia.Media.Colors.Red, 0.1),
                });
                y += 2 * radius + 10;
            }

            return new ObservableCollection<VisualShape>(result);
        }
    }
}
