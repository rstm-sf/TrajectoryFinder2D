using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TrajectoryFinder2D.ViewModels;

namespace TrajectoryFinder2D.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
