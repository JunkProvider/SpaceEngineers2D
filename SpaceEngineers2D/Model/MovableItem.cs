namespace SpaceEngineers2D.Model
{
    using Geometry;
    using Items;

    public class MovableItem : IMovableObject
    {
        public IntVector Position { get; set; }

        public IntVector Size { get; set; } = new IntVector(200, 200, 200);

        public IntRectangle Bounds => IntRectangle.FromPositionAndSize(Position, Size);

        public IntVector Velocity { get; set; }

        public TouchedObjectCollection TouchedObjects { get; set; } = new TouchedObjectCollection();

        public ItemStack ItemStack { get; }

        public bool IsSolid => true;

        public MovableItem(ItemStack itemStack)
        {
            ItemStack = itemStack;
        }
    }
}
