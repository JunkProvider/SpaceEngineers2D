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

        public IStructuralBlockType StructuralBlockType { get; }

        public bool Finished => BlueprintState.Finished;

        public ImageSource Image => StructuralBlockType.Image;

        public override bool IsDestoryed => BlueprintState.Integrity <= 0;

        public StructuralBlock(IStructuralBlockType blockType)
            : base(blockType)
        {
            StructuralBlockType = blockType;
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
