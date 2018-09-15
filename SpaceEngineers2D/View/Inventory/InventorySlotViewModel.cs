using SpaceEngineers2D.Model.Inventories;

namespace SpaceEngineers2D.View.Inventory
{
    public class InventorySlotViewModel
    {
        public InventorySlot Slot { get; }

        public InventorySlot InteractionSlot { get; }

        public InventorySlotViewModel(InventorySlot slot, InventorySlot interactionSlot)
        {
            Slot = slot;
            InteractionSlot = interactionSlot;
        }

        public void TryTransfereWithInteractionSlot()
        {
            InventorySlot.TryExchangeItem(InteractionSlot, Slot);
        }
    }
}
