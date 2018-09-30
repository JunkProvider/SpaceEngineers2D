﻿namespace SpaceEngineers2D.Model
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
                coordinateSystem.MinX / Constants.BlockSize,
                coordinateSystem.MaxX / Constants.BlockSize,
                coordinateSystem.MinY / Constants.BlockSize,
                coordinateSystem.MaxY / Constants.BlockSize,
                coordinateSystem.MinZ / Constants.BlockSize,
                coordinateSystem.MaxZ / Constants.BlockSize));
        }

        public IntRectangle GetBlockBounds(IntVector position)
        {
            position = position - Position;
            var blockPosition = position.Floor(Constants.BlockSize);
            blockPosition = CoordinateSystem.Normalize(blockPosition);
            return IntRectangle.FromPositionAndSize(Position + blockPosition, Constants.BlockSizeVector);
        }

        public BlockInWorld<Block> GetBlock(IntVector position)
        {
            position = position - Position;
            position = position.DivideRoundDown(Constants.BlockSize);
            var block = InnerGrid.Get(position);
            return block!= null ? new BlockInWorld<Block>(block, this, Position + position * Constants.BlockSize) : null;
        }

        public Block SetBlock(IntVector position, Block block)
        {
            // position = CoordinateSystem.Normalize(position);
            position = position - Position;
            position = position.DivideRoundDown(Constants.BlockSize);
            return InnerGrid.Set(position, block).RemovedItem;
        }

        public void ForEach(EnumerateItemDelegate<Block> func)
        {
            InnerGrid.ForEach((block, blockPosition) => func(block, Position + blockPosition * Constants.BlockSize));
        }

        public void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<Block> func)
        {
            var transformedRectangle = rectangle.Move(-Position);
            transformedRectangle = IntRectangle.FromPositionAndSize(
                transformedRectangle.Position.DivideRoundDown(Constants.BlockSize),
                transformedRectangle.Size.DivideRoundUp(Constants.BlockSize)
            );

            InnerGrid.ForEachWithin(transformedRectangle, (block, blockPosition) => func(block, Position + blockPosition * Constants.BlockSize));
        }
    }
}
