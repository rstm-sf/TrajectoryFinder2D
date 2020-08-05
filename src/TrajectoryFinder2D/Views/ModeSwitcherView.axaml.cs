using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TrajectoryFinder2D.Views
{
    public class ModeSwitcherView : UserControl
    {
        public ModeSwitcherView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
