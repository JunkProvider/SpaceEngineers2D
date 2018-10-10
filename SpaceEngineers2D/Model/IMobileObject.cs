namespace SpaceEngineers2D.Model
{
    using System.Collections.Generic;

    using Geometry;
    using Blocks;

    public interface IMobileObject
    {
        IntVector Position { get; set; }

        IntVector Size { get; }

        IntRectangle Bounds { get; }

        IntVector Velocity { get; set; }

        Dictionary<Side, List<Block>> TouchedBlocks { get; set; }
    }
}
