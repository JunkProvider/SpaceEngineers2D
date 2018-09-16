namespace SpaceEngineers2D.Persistence.DataModel
{
    public class ItemStackData : IDataModel
    {
        public ItemData Item { get; set; }

        public int Size { get; set; }

        public ItemStackData(ItemData item, int size)
        {
            Item = item;
            Size = size;
        }

        public ItemStackData()
        {
        }
    }
}