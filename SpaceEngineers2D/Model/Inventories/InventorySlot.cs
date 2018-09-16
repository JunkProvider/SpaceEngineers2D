using System.ComponentModel;
using System.Runtime.CompilerServices;
using SpaceEngineers2D.Annotations;

namespace SpaceEngineers2D.Model.Inventories
{
    using System;

    using Items;

    public class InventorySlot : INotifyPropertyChanged
    {
        public static void TryExchangeItem(InventorySlot slotA, InventorySlot slotB)
        {
            if (slotA.ContainsItem)
            {
                var replacingItem = slotB.ReplaceOrCombine(slotA.Take());

                if (replacingItem != null)
                {
                    slotA.Put(replacingItem);
                }
            }
            else if (slotB.ContainsItem)
            {
                slotA.Put(slotB.Take());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; }

        public bool ContainsItem => ItemStack != null;

        public ItemStack ItemStack { get; private set; }

        public InventorySlot(int id)
        {
            Id = id;
        }

        public ItemStack ReplaceOrCombine(ItemStack itemStack)
        {
            if (itemStack == null)
                throw new ArgumentNullException(nameof(itemStack));

            var prevItemStack = ItemStack;

            if (prevItemStack == null)
            {
                ItemStack = itemStack;
                RaiseContentChanged();
                return null;
            }

            if (prevItemStack.Item.Equals(itemStack.Item))
            {
                ItemStack = ItemStack.Combine(ItemStack, itemStack);
                RaiseContentChanged();
                return null;
            }

            ItemStack = itemStack;
            RaiseContentChanged();
            return prevItemStack;
        }

        public ItemStack TakeNOfType(ItemType itemType, int n)
        {
            if (ItemStack == null || ItemStack.Item.ItemType != itemType)
                return null;

            var returnedStack = new ItemStack(ItemStack.Item.Clone(), Math.Min(ItemStack.Size, n));
            var remainingStackSize = ItemStack.Size - returnedStack.Size;

            if (remainingStackSize == 0)
            {
                ItemStack = null;
            }
            else
            {
                ItemStack = new ItemStack(ItemStack.Item.Clone(), remainingStackSize);
            }
                
            RaiseContentChanged();

            return returnedStack;
        }

        public ItemStack Take()
        {
            var takenItemSTack = ItemStack;

            if (takenItemSTack == null)
                throw new InvalidOperationException();

            ItemStack = null;

            RaiseContentChanged();

            return takenItemSTack;
        }

        public bool Put(ItemStack itemStack)
        {
            if (ItemStack == null)
            {
                ItemStack = itemStack;

                RaiseContentChanged();

                return true;
            }

            if (itemStack.Item.Equals(ItemStack.Item))
            {
                ItemStack = new ItemStack(ItemStack.Item.Clone(), ItemStack.Size + itemStack.Size);

                RaiseContentChanged();

                return true;
            }

            return false;
        }

        public void Empty()
        {
            ItemStack = null;

            RaisePropertyChanged(nameof(ItemStack));
            RaisePropertyChanged(nameof(ContainsItem));
        }

        public override string ToString()
        {
            return ItemStack?.ToString() ?? "[Empty Slot]";
        }

        private void RaiseContentChanged()
        {
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
