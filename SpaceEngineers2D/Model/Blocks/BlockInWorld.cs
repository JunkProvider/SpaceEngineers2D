using System;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Model.Blocks
{
    using Geometry;

    public class BlockInWorld<TBlock> : IBlockInWorld
        where TBlock : Block
    {
        public TBlock Object { get; }

        Block IBlockInWorld.Object => Object;

        public Grid Grid { get; }

        public IntRectangle Bounds { get; }

        public BlockInWorld(TBlock obj, Grid grid, IntVector position)
        {
            Object = obj ?? throw new ArgumentNullException(nameof(obj));
            Grid = grid ?? throw new ArgumentNullException(nameof(grid));
            Bounds = IntRectangle.FromPositionAndSize(position, Constants.BlockSizeVector);
        }

        public void As<TCastedBlock>(Action<BlockInWorld<TCastedBlock>> action) where TCastedBlock : Block
        {
            var castedBlock = Object as TCastedBlock;
            if (castedBlock != null)
            {
                action(new BlockInWorld<TCastedBlock>(castedBlock, Grid, Bounds.Position));
            }
        }
    }
}
