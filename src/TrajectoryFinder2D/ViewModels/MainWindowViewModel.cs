using System;
using System.Collections.Generic;
using Avalonia.Threading;
using TrajectoryFinder2D.Commands;
using TrajectoryFinder2D.Models;
using TrajectoryFinder2D.Utils;

namespace TrajectoryFinder2D.ViewModels
{
    internal class MainWindowViewModel : ObservableObjectBase
    {
        private const double Scale = 6;

        private readonly Point Offset = new Point { X = 200, Y = 200, };

        private readonly DispatcherTimer _timer;

        private int _tickCount;

        private bool _isShapeCaptured;

        private Point _panelMousePosition;

        private readonly Point _previousPanelMousePosition;

        private readonly IReadOnlyList<Circle> _circles;

        private readonly Square _square;

        private readonly PolyLine _polyLine;

        private readonly double _velocity;

        private readonly TravelStarts _travelStarts;

        public RelayCommand<Circle> PreviewMouseMove { get; }

        public RelayCommand LeftMouseButtonUp { get; }

        public RelayCommand LeftMouseButtonDown { get; }

        public RelayCommand Start { get; }

        public Point PanelMousePosition
        {
            get => _panelMousePosition;
            set => SetProperty(ref _panelMousePosition, value);
        }

        public ItemsChangeObservableCollection<ShapeBase> ShapeCollection { get; set; }

        public MainWindowViewModel()
        {
            _velocity = 1e6;
            _travelStarts = new TravelStarts();

            _circles = new List<Circle>()
            {
                new Circle(50, ConvertToViewPoint(_travelStarts.Points[0])),
                new Circle(50, ConvertToViewPoint(_travelStarts.Points[1])),
                new Circle(50, ConvertToViewPoint(_travelStarts.Points[2])),
            };
            _square = new Square(20);
            _polyLine = new PolyLine();

            ShapeCollection = new ItemsChangeObservableCollection<ShapeBase>(_circles);

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

            Start = new RelayCommand(_ => _timer.Start());

            var ticksPerSecond = 1;
            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 1000 / ticksPerSecond)
            };
            _timer.Tick += (sender, args) => Tick();
        }

        private void SavePreviousPanelMousePosition()
        {
            _previousPanelMousePosition.X = _panelMousePosition.X;
            _previousPanelMousePosition.Y = _panelMousePosition.Y;
        }

        private void Tick()
        {
            if (_tickCount == _travelStarts.ToPointTimes.Count)
            {
                _timer.Stop();
                return;
            }

            var times = _travelStarts.ToPointTimes[_tickCount];
            for (var i = 0; i < _circles.Count; ++i)
                _circles[i].Radius = ConvertToViewRadius(times[i] * _velocity);

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

            ++_tickCount;
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
