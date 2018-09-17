using SpaceEngineers2D.View.Inventory;

namespace SpaceEngineers2D.View
{
    public class BlastFurnaceViewModel
    {
        public InventoryViewModel InventoryViewModel { get; }

        public BlastFurnaceViewModel(InventoryViewModel inventoryViewModel)
        {
            InventoryViewModel = inventoryViewModel;
        }
    }
}