using System;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Model.Blocks
{
    using System.Collections.Generic;
    using Items;

    public abstract class Block : ICollidable
    {
        public IBlockType BlockType { get; }

        public IntVector Position { get; set; }

        public IntVector Size => Constants.BlockSizeVector;

        public IntRectangle Bounds => IntRectangle.FromPositionAndSize(Position, Size);

        public abstract bool IsSolid { get; }

        public abstract bool IsDestoryed { get; }

        public IntVector Velocity => IntVector.Zero;

        protected Block(IBlockType blockType)
        {
            BlockType = blockType;
        }

        public abstract ICollection<ItemStack> GetDroppedItems();

        public abstract void Damage(float damage);

        public virtual void OnUpdate(World world, TimeSpan elapsedTime)
        {

        }

        public virtual void OnNeighborChanged(World world, IBlockInWorld changedNeighbor)
        {

        }

        public virtual InteractionResult OnInteraction(OnInteractionContext context)
        {
            return InteractionResult.None;
        }

        public virtual void OnInteractionEnded()
        {

        }

        public class OnInteractionContext
        {
            public World World { get; }

            public IntRectangle OwnBlockBounds { get; }

            public OnInteractionContext( World world, IntRectangle ownBlockBounds)
            {
                World = world;
                OwnBlockBounds = ownBlockBounds;
            }
        }

        public enum InteractionResult
        {
            None,
            Finished,
            Continuing
        }
    }
}
