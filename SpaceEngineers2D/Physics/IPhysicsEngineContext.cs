using SpaceEngineers2D.Geometry;

namespace SpaceEngineers2D.Physics
{
    public interface IPhysicsEngineContext : ICollisionEngineContext
    {
        IntVector Gravity { get; }
    }
}