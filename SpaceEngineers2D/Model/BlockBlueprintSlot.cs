using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SpaceEngineers2D.Annotations;
using SpaceEngineers2D.Model.Blocks;

namespace SpaceEngineers2D.Model
{
    public class BlockBlueprintSlot : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private StructuralBlockType _blueprintedBlock;
        private Key _hotkey;
        private bool _selected;

        public Key Hotkey
        {
            get => _hotkey;
            set => SetProperty(ref _hotkey, value);
        }

        public StructuralBlockType BlueprintedBlock
        {
            get => _blueprintedBlock;
            set => SetProperty(ref _blueprintedBlock, value);
        }

        public bool Selected
        {
            get => _selected;
            set => SetProperty(ref _selected, value);
        }

        public BlockBlueprintSlot(Key hotkey)
        {
            _hotkey = hotkey;
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
    }
}