namespace SpaceEngineers2D.Model
{
    using System.Collections.Generic;

    using SpaceEngineers2D.Geometry;
    using SpaceEngineers2D.Model.Blocks;
    using SpaceEngineers2D.Model.Items;

    public class MobileItem : IMobileObject
    {
        public IntVector Position { get; set; }

        public IntVector Size { get; set; } = new IntVector(200, 200);

        public IntRectangle Bounds => IntRectangle.FromPositionAndSize(Position, Size);

        public IntVector Velocity { get; set; }

        public Dictionary<Side, List<Block>> TouchedBlocks { get; set; } = new Dictionary<Side, List<Block>>();

        public ItemStack ItemStack { get; }

        public MobileItem(ItemStack itemStack)
        {
            ItemStack = itemStack;
            TouchedBlocks[Side.Left] = new List<Block>();
            TouchedBlocks[Side.Right] = new List<Block>();
            TouchedBlocks[Side.Top] = new List<Block>();
            TouchedBlocks[Side.Bottom] = new List<Block>();
        }
    }
}
