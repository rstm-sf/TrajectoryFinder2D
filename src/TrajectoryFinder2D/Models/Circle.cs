namespace TrajectoryFinder2D.Models
{
    internal class Circle : ShapeWithLeftTopCornerBase
    {
        private double _radius;

        public double Diameter => _radius * 2;

        public Point Center => new Point
        {
            X = Left + _radius,
            Y = Top + _radius,
        };

        public double Radius
        {
            get => _radius;
            set
            {
                if (SetProperty(ref _radius, value))
                {
                    NotifyPropertyChanged(nameof(Diameter));
                    NotifyPropertyChanged(nameof(Center));
                }
            }
        }

        public Circle(double radius, Point center)
        {
            Radius = radius;
            Left = center.X - radius;
            Top = center.Y - radius;
        }
    }
}
