using System.Linq;

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

        /* public IList<IItem> GetItems()
        {
            return this.Slots.Select(slot => slot.Item).Where(item => item != null).ToList();
        }*/

        /* public bool TryTakeNOfType(ItemType itemType, int n, out ItemStack stack)
        {
            stack = null;

            foreach (var slot in Slots)
            {
                stack = ItemStack.Combine(stack, slot.TakeNOfType(itemType, n));
                
                if (stack != null && stack.Size == n)
                    break;
            }

            return stack != null;
        }*/

        /* public bool Put(IItem item)
        {
            foreach (var slot in Slots)
            {
                if (slot.Put(item))
                {
                    return true;
                }
            }

            return false;
        } */
    }
}
