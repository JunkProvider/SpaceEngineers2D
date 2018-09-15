using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SpaceEngineers2D.Annotations;

namespace SpaceEngineers2D.View
{
    public class MousePositionProvider : INotifyPropertyChanged
    {
        public static readonly MousePositionProvider Instance = new MousePositionProvider();
        private Point _absoluteMousePosition;

        public event PropertyChangedEventHandler PropertyChanged;

        public Point AbsoluteMousePosition
        {
            get => _absoluteMousePosition;
            set => SetProperty(ref _absoluteMousePosition, value);
        }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!object.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
