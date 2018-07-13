using System;

namespace SpaceEngineers2D.Model.Blocks
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Media;

    using Items;

    public sealed class StandardBlock : Block
    {
        public StandardBlockType StandardBlockType => (StandardBlockType)BlockType;

        public override bool IsSolid => true;

        public ImageSource Image => StandardBlockType.Image;

        public float Integrity { get; private set; }

        public override bool IsDestoryed => Integrity <= 0;

        public float IntegrityRatio => Integrity / StandardBlockType.MaxIntegrity;

        public StandardBlock(StandardBlockType blockType)
            : base(blockType)
        {
            Integrity = blockType.MaxIntegrity;
        }

        public override IReadOnlyList<IItem> GetDroppedItems()
        {
            return StandardBlockType.GetDroppedItems();
        }

        public override void Damage(float damage)
        {
            Integrity = Math.Max(0, Integrity - damage);
        }
    }
}
