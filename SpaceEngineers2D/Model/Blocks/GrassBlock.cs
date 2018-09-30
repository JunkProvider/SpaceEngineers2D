using System.Collections.Generic;
using System.Windows.Media;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model.Items;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Model.Blocks
{
    public class GrassBlock : Block, IStandardRenderableBlock
    {
        private bool _isDestroyed;

        public GrassBlockType GrassBlockType { get; }

        public double IntegrityRatio => 1;

        public override bool IsDestoryed => _isDestroyed;

        public override bool IsSolid => false;

        public ImageSource Image => GrassBlockType.Image;

        public GrassBlock(GrassBlockType blockType) : base(blockType)
        {
            GrassBlockType = blockType;
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
            
            if (!world.IsBottomBlock(ownBounds.Position, changedNeighbor.Bounds.Position))
            {
                return;
            }

            var bottomBlock = world.GetBottomBlock(ownBounds.Position);

            if (bottomBlock?.Object.BlockType != world.BlockTypes.Dirt && bottomBlock?.Object.BlockType != world.BlockTypes.DirtWithGrass)
            {
                world.RemoveBlock(ownBounds.LeftTop);
            }
        }
    }
}
