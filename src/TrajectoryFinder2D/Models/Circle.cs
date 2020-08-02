using System;
using System.Collections.Generic;
using Avalonia.Media;

namespace TrajectoryFinder2D.Models
{
    internal class Circle : ShapeWithLeftTopCornerBase
    {
        private static IReadOnlyList<ISolidColorBrush> SolidColorBrushes =
            new List<ISolidColorBrush>
            {
                new SolidColorBrush(Colors.Red, 0.2),
                new SolidColorBrush(Colors.Green, 0.2),
                new SolidColorBrush(Colors.Blue, 0.2),
            };

        private static int Count;

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
                    UpdateLeftTopCorner();
                }
            }
        }

        public Point Center
        {
            get => _center;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));

                if (SetProperty(ref _center, value))
                    UpdateLeftTopCorner();
            }
        }

        public Circle()
        {
            Center = new Point();
            FillColor = SolidColorBrushes[Count % SolidColorBrushes.Count];
            ++Count;
        }

        public Circle(double radius, Point center)
            : this()
        {
            Radius = radius;
            Center = center;
        }

        private void UpdateLeftTopCorner()
        {
            Left = _center.X - _radius;
            Top = _center.Y - _radius;
        }
    }
}
