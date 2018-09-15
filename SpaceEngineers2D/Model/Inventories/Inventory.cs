namespace SpaceEngineers2D.Model.Inventories
{
    using System;
    using System.Collections.Generic;

    using Items;

    public class Inventory
    {
        public List<InventorySlot> Slots { get; } = new List<InventorySlot>();

        public Inventory(int slotCount)
        {
            for (var i = 0; i < slotCount; i++)
            {
                this.Slots.Add(new InventorySlot());
            }
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
