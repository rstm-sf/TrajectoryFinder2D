using System;

namespace TrajectoryFinder2D.Models
{
    internal class PointGenerator
    {
        private static readonly Random s_random = new Random();

        private static readonly int s_sidesCount = Enum.GetNames(typeof(Sides)).Length;

        private const int SideMoveMaxCount = 10;

        private int _sideMoveCurrentCount;

        private Sides _currentSide;

        private readonly double _dx;

        private readonly double _dy;

        public PointGenerator(double dx, double dy)
        {
            _dx = dx;
            _dy = dy;
        }

        public Point GetNewPoint(Point point)
        {
            if (_sideMoveCurrentCount % SideMoveMaxCount == 0)
                _currentSide = (Sides)s_random.Next(s_sidesCount);

            ++_sideMoveCurrentCount;

            return _currentSide switch
            {
                Sides.N  => new Point { X = point.X,       Y = point.Y - _dy },
                Sides.Nw => new Point { X = point.X - _dx, Y = point.Y - _dy },
                Sides.W  => new Point { X = point.X - _dx, Y = point.Y },
                Sides.Sw => new Point { X = point.X - _dx, Y = point.Y + _dy },
                Sides.S  => new Point { X = point.X,       Y = point.Y + _dy },
                Sides.Se => new Point { X = point.X + _dx, Y = point.Y + _dy },
                Sides.E  => new Point { X = point.X + _dx, Y = point.Y },
                _        => new Point { X = point.X + _dx, Y = point.Y - _dy },
            };
        }

        private enum Sides
        {
            N,
            Nw,
            W,
            Sw,
            S,
            Se,
            E,
            Ne,
        }
    }
}
