namespace SpaceEngineers2D.Model.Items
{
    public class ItemStack
    {
        public static ItemStack Combine(ItemStack a, ItemStack b)
        {
            if (a == null && b == null)
                return null;

            if (a != null)
            {
                return new ItemStack(a.Item.Clone(), a.Size + (b?.Size ?? 0));
            }

            return b;
        }
        
        public Item Item { get; }
        
        public int Size { get; }

        public float Mass => Item.Mass * Size;

        public float Volume => Item.Volume * Size;

        public ItemStack(Item item)
            : this(item, 1)
        {
        }

        public ItemStack(Item item, int size)
        {
            Item = item;
            Size = size;
        }

        public override string ToString()
        {
            return Size + " " + Item;
        }
    }
}
