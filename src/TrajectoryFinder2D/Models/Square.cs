namespace TrajectoryFinder2D.Models
{
    internal class Square : ShapeWithLeftTopCornerBase
    {
        private double _length;

        public double Length
        {
            get => _length;
            set => SetProperty(ref _length, value);
        }

        public Square(double length, Point center)
        {
            Length = length;

            Left = center.X - length / 2;
            Top = center.Y - length / 2;

            FillColor = Avalonia.Media.Brushes.Blue;
        }
    }
}
