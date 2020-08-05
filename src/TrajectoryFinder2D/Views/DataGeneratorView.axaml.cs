using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TrajectoryFinder2D.Views
{
    public class DataGeneratorView : UserControl
    {
        public DataGeneratorView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
