namespace SpaceEngineers2D.Model.Inventories
{
    using System;
    using System.Collections.Generic;

    using Items;

    public class Inventory
    {
        public List<InventorySlot> Slots { get; } = new List<InventorySlot>
        {
            new InventorySlot(), new InventorySlot(), new InventorySlot(),
            new InventorySlot(), new InventorySlot(), new InventorySlot(),
            new InventorySlot(), new InventorySlot(), new InventorySlot()
        };

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
            if (itemStack.Size == 0)
                throw new InvalidOperationException();

            foreach (var slot in Slots)
            {
                slot.Put(itemStack);

                if (itemStack.Size == 0)
                    break;
            }
        }
    }
}
