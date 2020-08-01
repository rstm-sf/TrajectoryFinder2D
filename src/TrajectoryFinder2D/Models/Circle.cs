namespace TrajectoryFinder2D.Models
{
    internal class Circle : ObservableObject
    {
        private double _left;
        private double _top;

        public double Radius { get; }

        public double Left
        {
            get => _left;
            set => SetProperty(ref _left, value);
        }

        public double Top
        {
            get => _top;
            set => SetProperty(ref _top, value);
        }

        public double Diameter => Radius * 2;

        public Point Center => new Point
        {
            X = _left + Radius,
            Y = _top + Radius,
        };

        public Circle(double radius, Point center)
        {
            Radius = radius;
            Left = center.X - radius;
            Top = center.Y - radius;
        }
    }
}
