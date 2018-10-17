using System;
using System.Collections.Generic;
using System.Windows.Media;
using SpaceEngineers2D.Geometry;

namespace SpaceEngineers2D.Model.Entities
{
    public abstract class Entity : IEntity
    {
        public IEntityType EntityType { get; }

        public ImageSource Image => EntityType.Image;

        public IntVector Position { get; set; }

        public abstract IntVector Size { get; }

        public IntRectangle Bounds => IntRectangle.FromPositionAndSize(Position, Size);

        public IntVector Velocity { get; set; }

        public TouchedObjectCollection TouchedObjects { get; } = new TouchedObjectCollection();

        public bool IsSolid => true;

        protected Entity(IEntityType entityType)
        {
            EntityType = entityType;
        }

        public virtual void Update(World world, TimeSpan elapsedTime)
        {
        }
    }
}