using SpaceEngineers2D.Geometry;

namespace SpaceEngineers2D.Model
{
    public interface ICollidable
    {
        IntRectangle Bounds { get; }

        bool IsSolid { get; }

        IntVector Velocity { get; }
    }
}
