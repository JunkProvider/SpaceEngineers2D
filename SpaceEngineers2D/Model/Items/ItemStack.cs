using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpaceEngineers2D.Annotations;

namespace SpaceEngineers2D.Model.Items
{
    public class ItemStack : INotifyPropertyChanged
    {
        public static ItemStack Combine(ItemStack a, ItemStack b)
        {
            if (a == null && b == null)
                return null;

            if (a != null)
            {
                return new ItemStack(a.Item.Clone(), a.Size + (b?.Size ?? 0));
            }

            return b;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private int _size;

        public Item Item { get; }
        
        public int Size
        {
            get => _size;
            set
            {
                if (value == _size)
                    return;

                _size = value;

                RaisePropertyChanged(nameof(Size));
                RaisePropertyChanged(nameof(Mass));
                RaisePropertyChanged(nameof(Volume));
            }
        }

        public float Mass => Item.Mass * Size;

        public float Volume => Item.Volume * Size;

        public ItemStack(Item item)
            : this(item, 1)
        {
            
        }

        public ItemStack(Item item, int size)
        {
            Item = item;
            Size = size;
        }

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
