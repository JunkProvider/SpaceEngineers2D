namespace SpaceEngineers2D.Model
{
    using System.Collections.Generic;

    using SpaceEngineers2D.Geometry;
    using SpaceEngineers2D.Model.Blocks;

    public interface IMobileObject
    {
        IntVector Position { get; set; }

        IntVector Size { get; }

        IntRectangle Bounds { get; }

        IntVector Velocity { get; set; }

        Dictionary<Side, List<Block>> TouchedBlocks { get; set; }
    }
}
