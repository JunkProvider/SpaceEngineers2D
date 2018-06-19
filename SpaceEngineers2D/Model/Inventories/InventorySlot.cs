using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpaceEngineers2D.Annotations;

namespace SpaceEngineers2D.Model.Inventories
{
    using System;

    using Items;

    public class InventorySlot : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool ContainsItem => ItemStack != null;

        public ItemStack ItemStack { get; private set; }

        public ItemStack TakeNOfType(ItemType itemType, int n)
        {
            if (ItemStack == null || ItemStack.Item.ItemType != itemType)
                return null;

            var returnedStack = new ItemStack(ItemStack.Item.Clone(), Math.Min(ItemStack.Size, n));

            ItemStack.Size -= returnedStack.Size;

            if (ItemStack.Size == 0)
                ItemStack = null;

            RaisePropertyChanged(nameof(ItemStack));
            RaisePropertyChanged(nameof(ContainsItem));

            return returnedStack;
        }

        public void Put(ItemStack itemStack)
        {
            if (ItemStack == null)
            {
                ItemStack = new ItemStack(itemStack.Item.Clone(), itemStack.Size);
                itemStack.Size = 0;
            }
            else if (itemStack.Item.Equals(ItemStack.Item))
            {
                ItemStack.Size += itemStack.Size;
                itemStack.Size = 0;
            }

            RaisePropertyChanged(nameof(ItemStack));
            RaisePropertyChanged(nameof(ContainsItem));
        }

        [NotifyPropertyChangedInvocator]
        private void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
