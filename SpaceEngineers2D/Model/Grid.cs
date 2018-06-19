namespace SpaceEngineers2D.Model
{
    using System;

    using Geometry;
    using Geometry.BinaryGrid;
    using Blocks;
    using Physics;

    public class Grid
    {
        private IntVector Position { get; set; }

        private readonly BinaryGridRoot<Block> _root = new BinaryGridRoot<Block>();

        public IntRectangle GetBlockBounds(IntVector position)
        {
            var blockPosition = new IntVector(
                (int)Math.Floor((double)position.X / Constants.PhysicsUnit),
                (int)Math.Floor((double)position.Y / Constants.PhysicsUnit));
            blockPosition *= Constants.PhysicsUnit;
            return IntRectangle.FromPositionAndSize(blockPosition, IntVector.RightBottom * Constants.PhysicsUnit);
        }

        public BlockInWorld<Block> GetBlock(IntVector position)
        {
            position = position - Position;
            position = new IntVector(
                (int)Math.Floor((double)position.X / Constants.PhysicsUnit),
                (int)Math.Floor((double)position.Y / Constants.PhysicsUnit));
            var block = _root.Get(position);
            return block!= null ? new BlockInWorld<Block>(block, this, position * Constants.PhysicsUnit) : null;
        }

        public Block SetBlock(IntVector position, Block block)
        {
            position = position - Position;
            position = new IntVector(
                (int)Math.Floor((double)position.X / Constants.PhysicsUnit),
                (int)Math.Floor((double)position.Y / Constants.PhysicsUnit));
            return _root.Set(position, block).RemovedItem;
        }

        public void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<Block> func)
        {
            rectangle = rectangle.Move(-Position);
            rectangle = IntRectangle.FromLeftTopRightAndBottom(
                (int)Math.Floor((double)rectangle.Left / Constants.PhysicsUnit),
                (int)Math.Floor((double)rectangle.Top / Constants.PhysicsUnit),
                (int)Math.Ceiling((double)rectangle.Right / Constants.PhysicsUnit),
                (int)Math.Ceiling((double)rectangle.Bottom / Constants.PhysicsUnit)
            );
            _root.ForEachWithin(rectangle, (block, blockPosition) => func(block, Position + blockPosition * Constants.PhysicsUnit));
        }
    }
}
