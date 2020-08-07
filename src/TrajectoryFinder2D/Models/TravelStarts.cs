using System.Collections.Generic;

namespace TrajectoryFinder2D.Models
{
    internal class TravelStarts
    {
        public double Velocity { get; }

        public IReadOnlyList<Point> Points { get; }

        public IReadOnlyList<IReadOnlyList<double>> ToPointTimes { get; }

        public TravelStarts(
            IReadOnlyList<Point> points,
            IReadOnlyList<IReadOnlyList<double>> toPointsTime)
        {
            Velocity = 1e6;
            Points = points;
            ToPointTimes = toPointsTime;
        }
    }
}
