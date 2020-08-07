using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Threading;
using TrajectoryFinder2D.Models;

namespace TrajectoryFinder2D.ViewModels
{
    internal abstract class TrajectoryFinderViewModelBase : ObservableObjectBase
    {
        private readonly DispatcherTimer _timer;

        private bool _isPause;

        private string _pauseContinueText;

        private bool _isPauseContinueEnabled;

        private bool _isSaveEnabled;

        protected readonly IReadOnlyList<Circle> _circles;

        protected readonly Square _square;

        protected readonly PolyLine _polyLine;

        protected readonly SaveFileDialog _saveFileDialog;

        protected int TickCount { get; private set; }

        public bool IsSaveEnabled
        {
            get => _isSaveEnabled;
            set => SetProperty(ref _isSaveEnabled, value);
        }

        public bool IsPauseContinueEnabled
        {
            get => _isPauseContinueEnabled;
            set => SetProperty(ref _isPauseContinueEnabled, value);
        }

        public string PauseContinueText
        {
            get => _pauseContinueText;
            set => SetProperty(ref _pauseContinueText, value);
        }

        public ItemsChangeObservableCollection<ShapeBase> ShapeCollection { get; }

        protected TrajectoryFinderViewModelBase(int ticksPerSecond = 10)
        {
            _timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 1000 / ticksPerSecond)
            };
            _timer.Tick += (sender, args) =>
            {
                if (TryTick())
                {
                    ++TickCount;
                }
                else
                {
                    _timer.Stop();
                }
            };

            PauseContinueText = "Start";
            IsPauseContinueEnabled = true;

            _circles = new List<Circle>
            {
                new Circle(),
                new Circle(),
                new Circle(),
            };
            _square = new Square(20);
            _polyLine = new PolyLine();

            var radius = 50;
            var y = radius;
            foreach (var circle in _circles)
            {
                circle.Radius = radius;
                circle.Center = new Point { X = radius, Y = y };
                y += 2 * radius + 10;
            }

            _square.Center = new Point { X = radius, Y = y };

            // It is necessary to add immediately so that the collection does not track
            // changes in the properties of these shapes.
            ShapeCollection = new ItemsChangeObservableCollection<ShapeBase>(
                new List<ShapeBase>
                {
                    _circles[0],
                    _circles[1],
                    _circles[2],
                    _square,
                });

            _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.Filters.Add(new FileDialogFilter
            {
                Name = "Text",
                Extensions = { "txt" },
            });
        }

        public void PauseContinue()
        {
            _isPause = !_isPause;
            PauseContinueText = _isPause ? "Pause" : "Continue";

            if (TickCount != 0)
                IsSaveEnabled = !IsSaveEnabled;

            _timer.IsEnabled = !_timer.IsEnabled;
        }

        protected abstract bool TryTick();
    }
}
