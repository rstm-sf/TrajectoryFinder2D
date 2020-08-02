using System.Collections.Generic;

namespace TrajectoryFinder2D.Models
{
    internal class TravelStarts
    {
        public IReadOnlyList<Point> Points { get; }

        public IReadOnlyList<IReadOnlyList<double>> ToPointTimes { get; }

        public TravelStarts()
        {
            Points = CreatePoints();
            ToPointTimes = CreateToPointTimes();
        }

        private static IReadOnlyList<Point> CreatePoints() =>
            new List<Point>
            {
                new Point { X = 0d, Y = 10d },
                new Point { X = -5.4, Y = -7.5 },
                new Point { X = 6.21, Y = -8d },
            };

        private static IReadOnlyList<IReadOnlyList<double>> CreateToPointTimes() =>
            new List<IReadOnlyList<double>>
            {
                new List<double> { 0.00001716, 0.00000583, 0.00001694 },
                new List<double> { 0.00001539, 0.00000517, 0.00001558 },
                new List<double> { 0.00001324, 0.00000524, 0.00001373 },
                new List<double> { 0.00001320, 0.00000570, 0.00001243 },
                new List<double> { 0.00001143, 0.00000682, 0.00001091 },
                new List<double> { 0.00001063, 0.00000854, 0.00000889 },
                new List<double> { 0.00001085, 0.00001054, 0.00000880 },
                new List<double> { 0.00001093, 0.00001199, 0.00000863 },
                new List<double> { 0.00001179, 0.00001335, 0.00000860 },
                new List<double> { 0.00001214, 0.00001660, 0.00000938 },
            };
    }
}
