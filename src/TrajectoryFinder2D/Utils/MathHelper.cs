using System;
using TrajectoryFinder2D.Models;

namespace TrajectoryFinder2D.Utils
{
    internal static class MathHelper
    {
        public static bool TryFindThreeCircleIntersection(
            Circle circle1,
            Circle circle2,
            Circle circle3,
            out Point point)
        {
            point = default;

            var dx = circle2.Center.X - circle1.Center.X;
            var dy = circle2.Center.Y - circle1.Center.Y;

            var distance1 = Math.Sqrt((dy * dy) + (dx * dx));
            if (distance1 > (circle1.Radius + circle2.Radius))
                return false;

            // Check on the one circle is contained in the other
            if (distance1 < Math.Abs(circle1.Radius - circle2.Radius))
                return false;

            // Determine the distance from point 1 to point 3
            var distance2 = (
                (circle1.Radius * circle1.Radius) -
                (circle2.Radius * circle2.Radius) +
                (distance1 * distance1)) /
                (2.0 * distance1);

            // Determine the coordinates of point 3
            var point3X = circle1.Center.X + (dx * distance2 / distance1);
            var point3Y = circle1.Center.Y + (dy * distance2 / distance1);

            // Determine the distance from point 2 to either of the intersection points
            var distance3 = Math.Sqrt((circle1.Radius * circle1.Radius) - (distance2 * distance2));

            // Now determine the offsets of the intersection points from point 3
            var rx = -dy * (distance3 / distance1);
            var ry = dx * (distance3 / distance1);

            // Determine the absolute intersection points
            var intersectionPoint1 = new Point { X = point3X + rx, Y = point3Y + ry };
            var intersectionPoint2 = new Point { X = point3X - rx, Y = point3Y - ry };

            // Lets determine if circle 3 intersects at either of the above intersection points
            dx = intersectionPoint1.X - circle3.Center.X;
            dy = intersectionPoint1.Y - circle3.Center.Y;
            var distance4 = Math.Sqrt((dy * dy) + (dx * dx));

            dx = intersectionPoint2.X - circle3.Center.X;
            dy = intersectionPoint2.Y - circle3.Center.Y;
            var distance5 = Math.Sqrt((dy * dy) + (dx * dx));

            if (Math.Abs(distance4 - circle3.Radius) < 1e-6)
            {
                point = intersectionPoint1;
            }
            else if (Math.Abs(distance5 - circle3.Radius) < 1e-6)
            {
                point = intersectionPoint2;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
