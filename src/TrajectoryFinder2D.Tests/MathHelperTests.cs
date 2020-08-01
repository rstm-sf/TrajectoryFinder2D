using TrajectoryFinder2D.Models;
using TrajectoryFinder2D.Utils;
using Xunit;

namespace TrajectoryFinder2D.Tests
{
    public class MathHelperTests
    {
        private const double Velocity = 1e6;

        private readonly Point Point1 = new Point { X = 0d, Y = 10d };
        private readonly Point Point2 = new Point { X = -5.4, Y = -7.5 };
        private readonly Point Point3 = new Point { X = 6.21, Y = -8d };

        [Theory]
        [InlineData(0.00001716, 0.00000583, 0.00001694, -9.85427700, -3.30208200)]
        [InlineData(0.00001539, 0.00000517, 0.00001558, -7.92068200, -2.74886900)]
        [InlineData(0.00001324, 0.00000524, 0.00001373, -5.78066500, -2.35031000)]
        [InlineData(0.00001320, 0.00000570, 0.00001243, -3.81985900, -1.98844000)]
        [InlineData(0.00001143, 0.00000682, 0.00001091, -1.94217300, -1.73413300)]
        [InlineData(0.00001063, 0.00000854, 0.00000889, -0.15681350, -0.89424750)]
        [InlineData(0.00001085, 0.00001054, 0.00000880, 2.13924600, -0.62065110)]
        [InlineData(0.00001093, 0.00001199, 0.00000863, 4.13878300, -0.12695850)]
        [InlineData(0.00001179, 0.00001335, 0.00000860, 5.81872400, 0.53746240)]
        [InlineData(0.00001214, 0.00001660, 0.00000938, 8.04749200, 1.01432800)]
        public void TryFindThreeCircleIntersectionTest(double dt1, double dt2, double dt3, double x, double y)
        {
            var circle1 = new Circle(dt1 * Velocity, Point1);
            var circle2 = new Circle(dt2 * Velocity, Point2);
            var circle3 = new Circle(dt3 * Velocity, Point3);

            var isFind = MathHelper.TryFindThreeCircleIntersection(
                circle1, circle2, circle3, out var point);

            Assert.True(isFind);
        }
    }
}
