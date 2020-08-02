using System.Collections.Generic;

namespace TrajectoryFinder2D.Models
{
    internal class PolyLine : ShapeBase
    {
        public List<Avalonia.Point> Points { get; }

        public PolyLine()
        {
            Points = new List<Avalonia.Point>();
            FillColor = Avalonia.Media.Brushes.Red;
        }

        public PolyLine(IReadOnlyList<Avalonia.Point> points)
            : this()
        {
            Points.AddRange(points);
        }
    }
}
