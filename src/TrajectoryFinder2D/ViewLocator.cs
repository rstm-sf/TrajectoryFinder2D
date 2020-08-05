using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;

namespace TrajectoryFinder2D
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public IControl Build(object data)
        {
            if (data is null)
                throw new ArgumentNullException(nameof(data));

            var name = data.GetType().FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            return type is null
                ? new TextBlock { Text = "Not Found: " + name }
                : (Control)Activator.CreateInstance(type);
        }

        public bool Match(object data) => data is ObservableObjectBase;
    }
}
