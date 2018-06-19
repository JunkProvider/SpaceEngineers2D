using SpaceEngineers2D.Geometry;

namespace SpaceEngineers2D.Model
{
    public class BlockCoords
    {
        public Grid Grid { get; }

        public IntRectangle Bounds { get; }

        public BlockCoords(Grid grid, IntRectangle bounds)
        {
            Grid = grid;
            Bounds = bounds;
        }
    }
}
