using System;
using System.Linq;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Model.Inventories;
using SpaceEngineers2D.Model.Items;

namespace SpaceEngineers2D.Model.Blocks
{
    public class BlastFurnaceBlock : StructuralBlock
    {
        public Inventory Inventory { get; } = new Inventory(3);

        public BlastFurnaceBlock(BlastFurnaceBlockType blockType)
            : base(blockType)
        {
        }

        public override void OnUpdate(World world, IntRectangle ownBounds, TimeSpan elapsedTime)
        {
            base.OnUpdate(world, ownBounds, elapsedTime);

            var coalSlot = Inventory.Slots.FirstOrDefault(slot => slot.ContainsItem && slot.ItemStack.Item.ItemType == world.ItemTypes.Coal);

            if (coalSlot == null)
                return;

            var ironOreSlot = Inventory.Slots.FirstOrDefault(slot => slot.ContainsItem && slot.ItemStack.Item.ItemType == world.ItemTypes.IronOre);

            if (ironOreSlot == null)
                return;

            var ironSlot = Inventory.Slots.FirstOrDefault(slot => !slot.ContainsItem || slot.ItemStack.Item.ItemType == world.ItemTypes.Iron);

            if (ironSlot == null)
                return;

            coalSlot.Take(1);
            ironOreSlot.Take(1);
            ironSlot.Put(new ItemStack(world.ItemTypes.Iron.Instantiate()));
        }

        public override InteractionResult OnInteraction(OnInteractionContext context)
        {
            base.OnInteraction(context);

            return InteractionResult.Continuing;
        }
    }
}
