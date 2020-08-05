using System;
using System.ComponentModel;

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

            PropertyChanged += SquarePropertyChanged;
        }

        public Square(double length, Point center)
            : this(length)
        {
            Center = center;
        }

        private void UpdateLeftTopCorner()
        {
            Left = _center.X - _length / 2;
            Top = _center.Y - _length / 2;
        }

        private void SquarePropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(Left):
                    {
                        var point = new Point { X = Left + _length / 2, Y = _center.Y };
                        SetProperty(ref _center, point, nameof(Center));
                        break;
                    }
                case nameof(Top):
                    {
                        var point = new Point { X = _center.X, Y = Top + _length / 2 };
                        SetProperty(ref _center, point, nameof(Center));
                        break;
                    }
            }
        }
    }
}
