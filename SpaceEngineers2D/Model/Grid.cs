namespace SpaceEngineers2D.Model
{
    using System;

    using Geometry;
    using Geometry.BinaryGrid;
    using Blocks;
    using Physics;

    public class Grid
    {
        public int Id { get; }

        private IntVector Position { get; set; }

        private CircularBinaryGrid<Block> InnerGrid { get; }

        private ICoordinateSystem CoordinateSystem { get; }

        public Grid(int id, ICoordinateSystem coordinateSystem)
        {
            Id = id;
            CoordinateSystem = coordinateSystem;
            InnerGrid = new CircularBinaryGrid<Block>(new CoordinateSystem(
                coordinateSystem.MinX / Constants.PhysicsUnit,
                coordinateSystem.MaxX / Constants.PhysicsUnit,
                coordinateSystem.MinY / Constants.PhysicsUnit,
                coordinateSystem.MaxY / Constants.PhysicsUnit));
        }

        public IntRectangle GetBlockBounds(IntVector position)
        {
            position = position - Position;
            var blockPosition = new IntVector(
                (int)Math.Floor((double)position.X / Constants.PhysicsUnit),
                (int)Math.Floor((double)position.Y / Constants.PhysicsUnit));
            blockPosition *= Constants.PhysicsUnit;
            blockPosition = CoordinateSystem.Normalize(blockPosition);
            return IntRectangle.FromPositionAndSize(Position + blockPosition, IntVector.RightBottom * Constants.PhysicsUnit);
        }

        public BlockInWorld<Block> GetBlock(IntVector position)
        {
            position = position - Position;
            position = new IntVector(
                (int)Math.Floor((double)position.X / Constants.PhysicsUnit),
                (int)Math.Floor((double)position.Y / Constants.PhysicsUnit));
            var block = InnerGrid.Get(position);
            return block!= null ? new BlockInWorld<Block>(block, this, Position + position * Constants.PhysicsUnit) : null;
        }

        public Block SetBlock(IntVector position, Block block)
        {
            // position = CoordinateSystem.Normalize(position);
            position = position - Position;
            position = new IntVector(
                (int)Math.Floor((double)position.X / Constants.PhysicsUnit),
                (int)Math.Floor((double)position.Y / Constants.PhysicsUnit));
            return InnerGrid.Set(position, block).RemovedItem;
        }

        public void ForEach(EnumerateItemDelegate<Block> func)
        {
            InnerGrid.ForEach((block, blockPosition) => func(block, Position + blockPosition * Constants.PhysicsUnit));
        }

        public void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<Block> func)
        {
            var transformedRectangle = rectangle.Move(-Position);
            transformedRectangle = IntRectangle.FromLeftTopRightAndBottom(
                (int)Math.Floor((double)transformedRectangle.Left / Constants.PhysicsUnit),
                (int)Math.Floor((double)transformedRectangle.Top / Constants.PhysicsUnit),
                (int)Math.Ceiling((double)transformedRectangle.Right / Constants.PhysicsUnit),
                (int)Math.Ceiling((double)transformedRectangle.Bottom / Constants.PhysicsUnit)
            );

            InnerGrid.ForEachWithin(transformedRectangle, (block, blockPosition) => func(block, Position + blockPosition * Constants.PhysicsUnit));
        }
    }
}
