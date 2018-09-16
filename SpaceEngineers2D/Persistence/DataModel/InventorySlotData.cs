namespace SpaceEngineers2D.Persistence.DataModel
{
    public class InventorySlotData : IDataModel
    {
        public int Id { get; set; }

        public ItemStackData ItemStack { get; set; }

        public InventorySlotData(int id, ItemStackData itemStack)
        {
            Id = id;
            ItemStack = itemStack;
        }

        public InventorySlotData()
        {
        }
    }
}