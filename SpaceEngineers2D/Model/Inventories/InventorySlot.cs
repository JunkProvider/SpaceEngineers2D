using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpaceEngineers2D.Annotations;

namespace SpaceEngineers2D.Model.Inventories
{
    using Items;

    public class InventorySlot : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool ContainsItem => Item != null;

        public IItem Item { get; set; }

        /* public IItem Take(Func<IItem, bool> condition, double amount)
        {
            if (Item == null || !condition(Item))
                return null;

            var returnedStack = new ItemStack(Item.Item.Clone(), Math.Min(Item.Size, n));

            Item.Size -= returnedStack.Size;

            if (Item.Size == 0)
                Item = null;

            RaisePropertyChanged(nameof(Item));
            RaisePropertyChanged(nameof(ContainsItem));

            return returnedStack;
        }

        public bool Put(IItem item)
        {
            if (Item == null)
            {
                Item = item;

                RaisePropertyChanged(nameof(Item));
                RaisePropertyChanged(nameof(ContainsItem));

                return true;
            }

            if (Item.CanBeCombinded(item))
            {
                Item = Item.Combine(item);

                RaisePropertyChanged(nameof(Item));
                RaisePropertyChanged(nameof(ContainsItem));

                return true;
            }

            return false;
        } */

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
