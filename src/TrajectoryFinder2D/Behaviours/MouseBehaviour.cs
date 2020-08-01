using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace TrajectoryFinder2D.Behaviours
{
    internal class MouseBehaviour : Behavior<Control>
    {
        public static readonly AvaloniaProperty MousePositionProperty =
            AvaloniaProperty.Register<MouseBehaviour, Models.Point>(nameof(MousePosition));

        public Models.Point MousePosition
        {
            get { return (Models.Point)GetValue(MousePositionProperty); }
            set { SetValue(MousePositionProperty, value); }
        }

        public MouseBehaviour()
        {
            MousePosition = new Models.Point { X = 0, Y = 0 };
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.PointerMoved += AssociatedObject_PointerMoved;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
            {
                AssociatedObject.PointerMoved -= AssociatedObject_PointerMoved;
            }
        }

        private void AssociatedObject_PointerMoved(object sender, PointerEventArgs e)
        {
            var position = e.GetPosition(AssociatedObject);
            MousePosition.X = position.X;
            MousePosition.Y = position.Y;
        }
    }
}
