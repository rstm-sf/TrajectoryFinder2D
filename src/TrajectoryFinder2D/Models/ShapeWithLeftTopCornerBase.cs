namespace TrajectoryFinder2D.Models
{
    internal abstract class ShapeWithLeftTopCornerBase : ShapeBase
    {
        private double _left;
        private double _top;

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
    }
}
