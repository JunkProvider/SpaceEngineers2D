
using System;
using System.ComponentModel;

namespace SpaceEngineers2D.Model.Inventories
{
    using System.Collections.Generic;

    using Items;

    public class Inventory
    {
        public event Action Changed;

        public List<InventorySlot> Slots { get; } = new List<InventorySlot>();

        public Inventory(int slotCount)
        {
            for (var i = 0; i < slotCount; i++)
            {
                var slot = new InventorySlot(i);
                slot.PropertyChanged += OnSlotPropertyChanged;
                Slots.Add(slot);
            }
        }

        private void OnSlotPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Changed?.Invoke();
        }

        public bool TryTakeNOfType(ItemType itemType, int n, out ItemStack stack)
        {
            stack = null;

            foreach (var slot in Slots)
            {
                stack = ItemStack.Combine(stack, slot.TakeNOfType(itemType, n));
                
                if (stack != null && stack.Size == n)
                    break;
            }

            return stack != null;
        }

        public void Put(ItemStack itemStack)
        {
            foreach (var slot in Slots)
            {
                if (slot.Put(itemStack))
                    break;
            }
        }
    }
}
