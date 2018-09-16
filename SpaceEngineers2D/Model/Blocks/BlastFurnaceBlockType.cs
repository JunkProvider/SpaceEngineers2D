using System.Collections.Generic;
using System.Windows.Media;
using SpaceEngineers2D.Model.BlockBlueprints;
using SpaceEngineers2D.Persistence;
using SpaceEngineers2D.Persistence.DataModel;

namespace SpaceEngineers2D.Model.Blocks
{
    public class BlastFurnaceBlockType : StructuralBlockType<BlastFurnaceBlock>
    {
        public BlastFurnaceBlockType(int id, string name, ImageSource image, BlockBlueprint blueprint)
            : base(id, name, image, blueprint)
        {

        }

        public StructuralBlock Instantiate()
        {
            return new BlastFurnaceBlock(this);
        }

        public override Block Load(Deserializer deserializer, DictionaryAccess data)
        {
            var block = new BlastFurnaceBlock(this);
            // TODO: Load blueprint state
            block.BlueprintState.FinishImmediately();

            if (data.TryGetClass("inventory", out InventoryData inventoryData))
            {
                deserializer.MapInventory(block.Inventory, inventoryData);
            }

            return block;
        }

        public override void Save(Serializer serializer, BlastFurnaceBlock block, DictionaryAccess data)
        {
            base.Save(serializer, block, data);

            // TODO: Save blueprint state
            data.Set("inventory", serializer.MapInventory(block.Inventory));
        }
    }
}
