using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TrajectoryFinder2D.Views
{
    public class TrajectoryFinderView : UserControl
    {
        public TrajectoryFinderView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
