namespace TrajectoryFinder2D.ViewModels
{
    internal class MainWindowViewModel : ObservableObjectBase
    {
        private ObservableObjectBase _content;

        public ObservableObjectBase Content
        {
            get => _content;
            private set => SetProperty(ref _content, value);
        }

        public ModeSwitcherViewModel Switcher { get; }

        public MainWindowViewModel()
        {
            Content = Switcher = new ModeSwitcherViewModel();
        }

        public void OpenTrajectoryFinder() =>
            Content = new TrajectoryFinderViewModel();
    }
}
