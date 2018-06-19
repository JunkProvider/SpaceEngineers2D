namespace SpaceEngineers2D.Model.Blocks
{
    using System.Collections.Generic;
    using System.Windows.Media;

    using BlockBlueprints;
    using Items;

    public class StructuralBlock : Block
    {
        public override bool IsSolid => true;

        public BlockBlueprintState BlueprintState { get; }

        public StructuralBlockType StructuralBlockType => (StructuralBlockType)BlockType;

        public bool Finished => BlueprintState.Finished;

        public ImageSource Image => StructuralBlockType.Image;

        public override bool IsDestoryed => BlueprintState.Integrity <= 0;

        public StructuralBlock(StructuralBlockType blockType)
            : base(blockType)
        {
            BlueprintState = new BlockBlueprintState(blockType.Blueprint);
        }

        public override ICollection<ItemStack> GetDroppedItems()
        {
            return BlueprintState.GetDroppedItems();
        }

        public override void Damage(float damage)
        {
            BlueprintState.Damage(damage);
        }
    }
}
