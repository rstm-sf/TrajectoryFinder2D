using System.Collections.Generic;

namespace TrajectoryFinder2D.Models
{
    internal class PolyLine : ShapeBase
    {
        public List<Avalonia.Point> Points { get; }

        public PolyLine()
        {
            Points = new List<Avalonia.Point>();
            FillColor = Avalonia.Media.Brushes.Brown;
        }

        public PolyLine(IReadOnlyList<Avalonia.Point> points)
            : this()
        {
            Points.AddRange(points);
        }

        public void AddPoint(Point point)
        {
            Points.Add(new Avalonia.Point(point.X, point.Y));
            NotifyPropertyChanged(nameof(Points));
        }
    }
}
