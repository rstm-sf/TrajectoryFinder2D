using System.Collections.Generic;
using Avalonia.Collections;

namespace TrajectoryFinder2D.Models
{
    internal class PolyLine : ShapeBase
    {
        public AvaloniaList<Avalonia.Point> Points { get; }

        public PolyLine()
        {
            Points = new AvaloniaList<Avalonia.Point>();
        }

        public PolyLine(IReadOnlyList<Avalonia.Point> points)
            : this()
        {
            Points.AddRange(points);
        }
    }
}
