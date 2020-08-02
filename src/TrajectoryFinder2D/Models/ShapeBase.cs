using Avalonia.Media;

namespace TrajectoryFinder2D.Models
{
    internal abstract class ShapeBase : ObservableObjectBase
    {
        private ISolidColorBrush _fillColor;

        public ISolidColorBrush FillColor
        {
            get => _fillColor;
            set => SetProperty(ref _fillColor, value);
        }
    }
}
