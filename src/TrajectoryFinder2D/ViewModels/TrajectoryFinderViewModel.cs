using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using TrajectoryFinder2D.Models;
using TrajectoryFinder2D.Utils;

namespace TrajectoryFinder2D.ViewModels
{
    internal class TrajectoryFinderViewModel : TrajectoryFinderViewModelBase
    {
        private readonly OpenFileDialog _openFileDialog;

        private TravelStarts _travelStarts;

        private bool _isReadEnabled;

        public bool IsVisibleRead
        {
            get => _isReadEnabled;
            set => SetProperty(ref _isReadEnabled, value);
        }

        public TrajectoryFinderViewModel(Action backAction)
            : base(backAction)
        {
            _openFileDialog = new OpenFileDialog();
            _openFileDialog.Filters.Add(new FileDialogFilter
            {
                Name = "Text",
                Extensions = { "txt" },
            });

            IsVisibleRead = true;
            IsPauseContinueEnabled = false;
        }

        public async Task Save()
        {
            var result = await _saveFileDialog.ShowAsync(new Window());
            if (result != null)
            {
                var sb = new StringBuilder();
                foreach (var point in _polyLine.Points)
                    sb.AppendLine(string.Concat(
                        point.X.ToString(CultureInfo.InvariantCulture),
                        ", ",
                        point.Y.ToString(CultureInfo.InvariantCulture)));

                await File.WriteAllTextAsync(result, sb.ToString());
            }
        }

        public async Task Read()
        {
            var result = await _openFileDialog.ShowAsync(new Window());
            if (result != null)
            {
                var lines = (await File.ReadAllLinesAsync(result[0])).ToArray();

                var numbers = lines.First().Split(',');
                for (var i = 0; i < _circles.Count; ++i)
                {
                    var x = double.Parse(numbers[2 * i], NumberStyles.Float, CultureInfo.InvariantCulture);
                    var y = double.Parse(numbers[2 * i + 1], NumberStyles.Float, CultureInfo.InvariantCulture);
                    _circles[i].Center = new Point { X = x, Y = y };
                }

                var tickTimes = new List<IReadOnlyList<double>>(lines.Length - 1);
                foreach (var line in lines.Skip(1))
                {
                    var timeText = line.Split(',');
                    var time = new List<double>(_circles.Count);
                    for (var i = 0; i < _circles.Count; ++i)
                        time.Add(double.Parse(timeText[i], NumberStyles.Float, CultureInfo.InvariantCulture));
                    tickTimes.Add(time);
                }

                _travelStarts = new TravelStarts(
                    _circles.Select(x => x.Center).ToList(),
                    tickTimes);

                IsVisibleRead = false;
                IsPauseContinueEnabled = true;
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
                IsBackEnabled = true;
                return false;
            }

            var times = _travelStarts.ToPointTimes[TickCount];
            for (var i = 0; i < _circles.Count; ++i)
                _circles[i].Radius = times[i] * _travelStarts.Velocity;

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
    }
}
