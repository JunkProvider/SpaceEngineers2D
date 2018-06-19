namespace SpaceEngineers2D.Model
{
    using System.Collections.Generic;

    using Geometry;
    using Blocks;
    using Inventories;

    public class Player : IMobileObject
    {
        public const int ItemPickupRange = 1000;

        public Inventory Inventory { get; } = new Inventory();

        public IntVector Position { get; set; }

        public IntVector Size { get; set; } = new IntVector(400, 1600);

        public IntRectangle Bounds => IntRectangle.FromPositionAndSize(Position, Size);

        public IntVector Velocity { get; set; }

        public Dictionary<Side, bool> MovementOrders { get; set; } = new Dictionary<Side, bool>();

        public IntVector TargetPosition { get; set; }

        public BlockCoords TargetBlockCoords { get; set; }

        public bool TargetBlockCoordsInRange { get; set; }

        public IBlockInWorld TargetBlock { get; set; }

        public Dictionary<Side, List<Block>> TouchedBlocks { get; set; } = new Dictionary<Side, List<Block>>();

        public Player()
        {
            TouchedBlocks[Side.Left] = new List<Block>();
            TouchedBlocks[Side.Right] = new List<Block>();
            TouchedBlocks[Side.Top] = new List<Block>();
            TouchedBlocks[Side.Bottom] = new List<Block>();

            MovementOrders[Side.Left] = false;
            MovementOrders[Side.Right] = false;
            MovementOrders[Side.Top] = false;
            MovementOrders[Side.Bottom] = false;
        }
    }
}
