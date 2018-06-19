using System.Collections.Generic;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model.Items;

namespace SpaceEngineers2D.Model.Blocks
{
    public class GrassBlock : Block
    {
        private bool _isDestroyed;

        public override bool IsDestoryed => _isDestroyed;

        public override bool IsSolid => false;

        public GrassBlock(BlockType blockType) : base(blockType)
        {
        }

        public override ICollection<ItemStack> GetDroppedItems()
        {
            return new List<ItemStack>();
        }

        public override void Damage(float damage)
        {
            _isDestroyed = true;
        }

        public override void OnNeighborChanged(World world, IntRectangle ownBounds, IBlockInWorld changedNeighbor)
        {
            base.OnNeighborChanged(world, ownBounds, changedNeighbor);

            if (ownBounds.Left == changedNeighbor.Bounds.Left && ownBounds.Bottom == changedNeighbor.Bounds.Top)
            {
                world.RemoveBlock(ownBounds.LeftTop);
            }
        }
    }
}
