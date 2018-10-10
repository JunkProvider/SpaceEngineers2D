using System;
using System.Collections.Generic;
using System.Windows.Media;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model.Blocks;

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

        public Dictionary<Side, List<Block>> TouchedBlocks { get; set; } = new Dictionary<Side, List<Block>>();

        protected Entity(IEntityType entityType)
        {
            EntityType = entityType;

            TouchedBlocks[Side.Left] = new List<Block>();
            TouchedBlocks[Side.Right] = new List<Block>();
            TouchedBlocks[Side.Top] = new List<Block>();
            TouchedBlocks[Side.Bottom] = new List<Block>();
            TouchedBlocks[Side.Front] = new List<Block>();
            TouchedBlocks[Side.Back] = new List<Block>();
        }

        public virtual void Update(World world, TimeSpan elapsedTime)
        {
        }
    }
}