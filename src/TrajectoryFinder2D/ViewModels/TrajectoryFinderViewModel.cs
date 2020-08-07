using System.IO;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using TrajectoryFinder2D.Models;
using TrajectoryFinder2D.Utils;

namespace TrajectoryFinder2D.ViewModels
{
    internal class TrajectoryFinderViewModel : TrajectoryFinderViewModelBase
    {
        private const double Scale = 6;

        private readonly Point Offset = new Point { X = 200, Y = 200, };

        private readonly TravelStarts _travelStarts;

        public TrajectoryFinderViewModel()
        {
            _travelStarts = new TravelStarts();

            for (var i = 0; i < _circles.Count; ++i)
            {
                _circles[i].Radius = 50;
                _circles[i].Center = ConvertToViewPoint(_travelStarts.Points[i]);
            }
        }

        public async Task Save()
        {
            var result = await _saveFileDialog.ShowAsync(new Window());
            if (result != null)
            {
                var sb = new StringBuilder();
                foreach (var point in _polyLine.Points)
                {
                    var x = (point.X - Offset.X) / Scale;
                    var y = (point.Y - Offset.Y) / Scale;
                    sb.AppendLine(x + ", " + y);
                }

                await File.WriteAllTextAsync(result, sb.ToString());
            }
        }

        protected override bool TryTick()
        {
            if (TickCount == _travelStarts.ToPointTimes.Count)
            {
                for (int i = 0; i < _circles.Count + 1; i++)
                    ShapeCollection.RemoveAt(0);

                IsSaveEnabled = true;
                IsPauseContinueEnabled = false;
                return false;
            }

            var times = _travelStarts.ToPointTimes[TickCount];
            for (var i = 0; i < _circles.Count; ++i)
                _circles[i].Radius = ConvertToViewRadius(times[i] * _travelStarts.Velocity);

            if (MathHelper.TryFindThreeCircleIntersection(
                _circles[0], _circles[1], _circles[2], out var point))
            {
                _square.Center = point;
                _polyLine.AddPoint(point);
                if (TickCount == 0)
                    ShapeCollection.Add(_polyLine);
            }

            return true;
        }

        private Point ConvertToViewPoint(Point point) =>
            new Point
            {
                X = point.X * Scale + Offset.X,
                Y = point.Y * Scale + Offset.Y,
            };

        private double ConvertToViewRadius(double radius) =>
            radius * Scale;
    }
}
