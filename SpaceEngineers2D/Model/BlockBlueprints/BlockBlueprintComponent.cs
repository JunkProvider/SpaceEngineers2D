namespace SpaceEngineers2D.Model.BlockBlueprints
{
    using SpaceEngineers2D.Model.Items;

    public class BlockBlueprintComponent
    {
        public int Count { get; }

        public StandardItemType ItemType { get; }

        public float IntegrityValue { get; }

        public BlockBlueprintComponent(int count, StandardItemType itemType, float integrityValue)
        {
            Count = count;
            ItemType = itemType;
            IntegrityValue = integrityValue;
        }

        public Item CreateItem()
        {
            return new StandardItem(ItemType);
        }
    }
}
