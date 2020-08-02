using System;

namespace TrajectoryFinder2D.Models
{
    internal class Circle : ShapeWithLeftTopCornerBase
    {
        private double _radius;

        private Point _center;

        public double Diameter => _radius * 2;

        public double Radius
        {
            get => _radius;
            set
            {
                if (SetProperty(ref _radius, value))
                {
                    NotifyPropertyChanged(nameof(Diameter));
                    Center = new Point
                    {
                        X = Top + _radius,
                        Y = Left + _radius,
                    };
                }
            }
        }

        public Point Center
        {
            get => _center;
            set
            {
                if (value is null)
                    throw new ArgumentException(nameof(value));

                if (SetProperty(ref _center, value))
                {
                    Left = _center.X - _radius;
                    Top = _center.Y - _radius;
                }
            }
        }

        public Circle()
        {
            FillColor = new Avalonia.Media.SolidColorBrush(
                            Avalonia.Media.Colors.Green, 0.2);
        }

        public Circle(double radius, Point center)
            : this()
        {
            Radius = radius;
            Center = center;
        }
    }
}
