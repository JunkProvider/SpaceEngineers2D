using System.Collections.Generic;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model;

namespace SpaceEngineers2D.Physics
{
    public interface ICollisionEngineContext
    {
        IEnumerable<ICollidable> GetCollidableObjectsWithin(IntRectangle rectangle);

        IEnumerable<IMovableObject> GetMovableObjects();
    }
}