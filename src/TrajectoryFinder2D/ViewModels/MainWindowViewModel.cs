using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrajectoryFinder2D.Commands;
using TrajectoryFinder2D.Models;

namespace TrajectoryFinder2D.ViewModels
{
    internal class MainWindowViewModel : ObservableObjectBase
    {
        bool _isShapeCaptured;

        private Point _panelMousePosition;

        private readonly Point _previousPanelMousePosition;

        private readonly List<Circle> _circles;

        private readonly Square _square;

        private readonly PolyLine _polyLine;

        public RelayCommand<Circle> PreviewMouseMove { get; }

        public RelayCommand LeftMouseButtonUp { get; }

        public RelayCommand LeftMouseButtonDown { get; }

        public Point PanelMousePosition
        {
            get => _panelMousePosition;
            set => SetProperty(ref _panelMousePosition, value);
        }

        public ObservableCollection<ShapeBase> ShapeCollection { get; set; }

        public MainWindowViewModel()
        {
            _circles = new List<Circle>()
            {
                new Circle(),
                new Circle(),
                new Circle(),
            };
            _square = new Square(20, new Point { X = 200, Y = 100, });
            _polyLine = new PolyLine();

            ShapeCollection = CreateShapeCollection();

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

        private ObservableCollection<ShapeBase> CreateShapeCollection()
        {
            const int radius = 50;
            var y = radius;

            foreach (var circle in _circles)
            {
                circle.Radius = radius;
                circle.Center = new Point { X = radius, Y = y };
                y += 2 * radius + 10;
            }

            return new ObservableCollection<ShapeBase>(_circles);
        }
    }
}
