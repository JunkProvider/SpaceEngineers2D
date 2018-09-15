using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using SpaceEngineers2D.Annotations;

namespace SpaceEngineers2D.View
{
    public partial class BlastFurnaceView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public BlastFurnaceView()
        {
            InitializeComponent();
        }

        private void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!object.Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
            }
        }

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
