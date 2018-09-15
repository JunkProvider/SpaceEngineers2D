using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpaceEngineers2D.Annotations;

namespace SpaceEngineers2D.View.Inventory
{
    public class PropertyObservable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!object.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}