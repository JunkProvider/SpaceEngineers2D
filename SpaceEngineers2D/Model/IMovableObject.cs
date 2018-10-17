namespace SpaceEngineers2D.Model
{
    using Geometry;

    public interface IMovableObject : ICollidable
    {
        IntVector Position { get; set; }

        IntVector Size { get; }

        IntVector Velocity { get; set; }

        TouchedObjectCollection TouchedObjects { get; }
    }
}
