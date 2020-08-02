using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TrajectoryFinder2D
{
    internal abstract class ObservableObjectBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            VerifyPropertyName(propertyName);

            if (Equals(field, value))
                return false;

            field = value;
            NotifyPropertyChanged(propertyName);

            return true;
        }

        [Conditional("DEBUG")]
        private void VerifyPropertyName([CallerMemberName] string propertyName = "")
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
                throw new ArgumentNullException(GetType().Name + " does not contain property: " + propertyName);
        }
    }
}
