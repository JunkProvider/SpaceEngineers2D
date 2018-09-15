using System.Collections.Generic;
using System.Linq;
using SpaceEngineers2D.Model.Inventories;

namespace SpaceEngineers2D.View.Inventory
{
    public class InventoryViewModel
    {
        public IReadOnlyList<InventorySlotViewModel> SlotViewModels { get; }

        public InventoryViewModel(Model.Inventories.Inventory inventory, InventorySlot interactionSlot)
        {
            SlotViewModels = inventory.Slots.Select(slot => new InventorySlotViewModel(slot, interactionSlot)).ToList();
        }
    }
}