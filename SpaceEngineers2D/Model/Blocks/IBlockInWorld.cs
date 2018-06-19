using System;
using System.Windows.Controls;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Model.Blocks
{
    using Geometry;

    public interface IBlockInWorld
    {
        Block Object { get; }

        Grid Grid { get; }

        IntRectangle Bounds { get; }

        void As<TCastedBlock>(Action<BlockInWorld<TCastedBlock>> action) where TCastedBlock : Block;
    }
}
