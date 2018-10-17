using System.Collections.Generic;
using System.Windows.Media;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model.Items;
using SpaceEngineers2D.Physics;

namespace SpaceEngineers2D.Model.Blocks
{
    public class ReedBlock : Block, IStandardRenderableBlock
    {
        private bool _isDestroyed;

        public ReedBlockType ReedBlockType { get; }

        public double IntegrityRatio => 1;

        public override bool IsDestoryed => _isDestroyed;

        public override bool IsSolid => false;

        public ImageSource Image => ReedBlockType.Image;

        public ReedBlock(ReedBlockType blockType) : base(blockType)
        {
            ReedBlockType = blockType;
        }

        public override ICollection<ItemStack> GetDroppedItems()
        {
            return new List<ItemStack>();
        }

        public override void Damage(float damage)
        {
            _isDestroyed = true;
        }

        public override void OnNeighborChanged(World world, IBlockInWorld changedNeighbor)
        {
            base.OnNeighborChanged(world, changedNeighbor);

            if (!world.IsBottomBlock(Bounds.Position, changedNeighbor.Bounds.Position))
            {
                return;
            }

            var bottomBlock = world.GetBottomBlock(Bounds.Position);

            if (bottomBlock?.Object.BlockType != world.BlockTypes.Dirt
                && bottomBlock?.Object.BlockType != world.BlockTypes.DirtWithGrass
                && bottomBlock?.Object.BlockType != world.BlockTypes.Reed)
            {
                world.RemoveBlock(Bounds.LeftTopFront);
            }
        }
    }
}
