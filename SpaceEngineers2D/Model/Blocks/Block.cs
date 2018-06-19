using SpaceEngineers2D.Geometry;

namespace SpaceEngineers2D.Model.Blocks
{
    using System.Collections.Generic;
    using Items;

    public abstract class Block
    {
        public BlockType BlockType { get; }

        public abstract bool IsSolid { get; }

        public abstract bool IsDestoryed { get; }

        protected Block(BlockType blockType)
        {
            BlockType = blockType;
        }

        public abstract ICollection<ItemStack> GetDroppedItems();

        public abstract void Damage(float damage);

        public virtual void OnNeighborChanged(World world, IntRectangle ownBounds, IBlockInWorld changedNeighbor)
        {

        }
    }
}
