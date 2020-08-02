using System;

namespace TrajectoryFinder2D.Models
{
    internal class Square : ShapeWithLeftTopCornerBase
    {
        private double _length;

        private Point _center;

        public double Length
        {
            get => _length;
            set
            {
                if (SetProperty(ref _length, value))
                    UpdateLeftTopCorner();
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

        public Square(double length)
        {
            Center = new Point();
            Length = length;
            FillColor = Avalonia.Media.Brushes.Brown;
        }

        public Square(double length, Point center)
            : this(length)
        {
            Center = center;
        }

        private void UpdateLeftTopCorner()
        {
            Left = Center.X - Length / 2;
            Top = Center.Y - Length / 2;
        }
    }
}
