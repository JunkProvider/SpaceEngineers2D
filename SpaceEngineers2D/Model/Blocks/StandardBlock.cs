using System;

namespace SpaceEngineers2D.Model.Blocks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    using Items;

    public sealed class StandardBlock : Block, IStandardRenderableBlock
    {
        public StandardBlockType StandardBlockType => (StandardBlockType)BlockType;

        public override bool IsSolid => StandardBlockType.IsSolid;

        public ImageSource Image => StandardBlockType.Image;

        public double Integrity { get; private set; }

        public override bool IsDestoryed => Integrity <= 0;

        public double IntegrityRatio => Integrity / StandardBlockType.MaxIntegrity;

        public StandardBlock(StandardBlockType blockType)
            : base(blockType)
        {
            Integrity = blockType.MaxIntegrity;
        }

        public StandardBlock(StandardBlockType blockType, double integrity)
            : base(blockType)
        {
            Integrity = integrity;
        }

        public override ICollection<ItemStack> GetDroppedItems()
        {
            return StandardBlockType.DroppedItems.Select(pair => new ItemStack(pair.Key.Instantiate(), pair.Value)).ToList();
        }

        public override void Damage(float damage)
        {
            Integrity = Math.Max(0, Integrity - damage);
        }
    }
}
