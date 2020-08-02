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
            _circles = new List<Circle>();
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
            const int circleCount = 3;
            const int radius = 50;
            var y = radius;

            for (var i = 0; i < circleCount; ++i)
            {
                _circles.Add(new Circle(radius, new Point { X = radius, Y = y })
                {
                    FillColor = new Avalonia.Media.SolidColorBrush(
                        Avalonia.Media.Colors.Green, 0.1),
                });
                y += 2 * radius + 10;
            }

            _square.FillColor = Avalonia.Media.Brushes.Blue;

            _polyLine.Points.AddRange(new List<Avalonia.Point>
            {
                new Avalonia.Point(200, 200),
                new Avalonia.Point(300, 300),
            });
            _polyLine.FillColor = Avalonia.Media.Brushes.Red;

            var result = new List<ShapeBase>(circleCount + 2);
            result.AddRange(_circles);
            result.Add(_square);
            result.Add(_polyLine);
            return new ObservableCollection<ShapeBase>(result);
        }
    }
}
