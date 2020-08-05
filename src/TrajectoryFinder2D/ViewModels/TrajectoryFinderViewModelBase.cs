using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Threading;
using TrajectoryFinder2D.Commands;
using TrajectoryFinder2D.Models;

namespace TrajectoryFinder2D.ViewModels
{
    internal abstract class TrajectoryFinderViewModelBase : ObservableObjectBase
    {
        private readonly DispatcherTimer _timer;

        private bool _isStartEnabled;

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

        public bool IsStartEnabled
        {
            get => _isStartEnabled;
            set => SetProperty(ref _isStartEnabled, value);
        }

        public RelayCommand Start { get; }

        public ItemsChangeObservableCollection<ShapeBase> ShapeCollection { get; }

        protected TrajectoryFinderViewModelBase(int ticksPerSecond = 1)
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
                    if (TickCount == 1)
                        IsStartEnabled = false;
                }
                else
                {
                    IsSaveEnabled = true;
                    _timer.Stop();
                }
            };

            IsStartEnabled = true;
            Start = new RelayCommand(_ => _timer.Start(), _ => IsStartEnabled);

            _circles = new List<Circle>
            {
                new Circle(),
                new Circle(),
                new Circle(),
            };
            _square = new Square(20);
            _polyLine = new PolyLine();

            ShapeCollection = new ItemsChangeObservableCollection<ShapeBase>(_circles);

            _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.Filters.Add(new FileDialogFilter
            {
                Name = "Text",
                Extensions = { "txt" },
            });
        }

        protected abstract bool TryTick();
    }
}