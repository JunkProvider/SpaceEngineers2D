using SpaceEngineers2D.Model.Inventories;

namespace SpaceEngineers2D.Model.Blocks
{
    public class BlastFurnaceBlock : StructuralBlock
    {
        public Inventory Inventory { get; } = new Inventory(3);

        public BlastFurnaceBlock(BlastFurnaceBlockType blockType)
            : base(blockType)
        {
        }

        public override InteractionResult OnInteraction(OnInteractionContext context)
        {
            base.OnInteraction(context);

            return InteractionResult.Continuing;
        }
    }
}
