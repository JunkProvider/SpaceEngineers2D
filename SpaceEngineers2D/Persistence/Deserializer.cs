using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.BlockBlueprints;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Model.Inventories;
using SpaceEngineers2D.Model.Items;
using SpaceEngineers2D.Persistence.DataModel;

namespace SpaceEngineers2D.Persistence
{
    public class Deserializer
    {
        private BlockTypes BlockTypes { get; }

        private ItemTypes ItemTypes { get; }

        public Deserializer(BlockTypes blockTypes, ItemTypes itemTypes)
        {
            BlockTypes = blockTypes;
            ItemTypes = itemTypes;
        }

        public void MapWorld(World world, WorldData worldData)
        {
            world.Player.Position = worldData.Player.Position;
            MapInventory(world.Player.Inventory, worldData.Player.Inventory);

            var gridDataDictionary = worldData.Grids.ToDictionary(gridData => gridData.Id);
            foreach (var grid in world.Grids)
            {
                if (gridDataDictionary.TryGetValue(grid.Id, out var gridData))
                {
                    MapGrid(grid, gridData);
                }
            }
        }

        public void MapInventory(Inventory inventory, InventoryData inventoryData)
        {
            var slots = inventory.Slots.ToDictionary(slot => slot.Id);

            foreach (var slotData in inventoryData.Slots)
            {
                if (slotData.ItemStack == null)
                    continue;

                var slot = slots[slotData.Id];

                MapInventorySlot(slot, slotData);
            }
        }

        public void MapInventorySlot(InventorySlot slot, InventorySlotData slotData)
        {
            var itemType = ItemTypes.GetItemType(slotData.ItemStack.Item.ItemTypeId);
            var item = itemType.Load(new DictionaryAccess(slotData.ItemStack.Item.Data));
            var itemStack = new ItemStack(item, slotData.ItemStack.Size);

            slot.Put(itemStack);
        }

        public void MapGrid(Grid grid, GridData gridData)
        {
            foreach (var blockData in gridData.Blocks)
            {
                var blockType = BlockTypes.GetBlockType(blockData.BlockTypeId);

                var block = blockType.Load(this, new DictionaryAccess(blockData.Data));
                
                grid.SetBlock(blockData.Position, block);
            }
        }

        public void MapBlockBlueprintState(BlockBlueprintState blueprintState, BlockBlueprintStateData blueprintStateData)
        {
            foreach (var componentData in blueprintStateData.Components)
            {
                if (ItemTypes.GetItemTypeDictionary().TryGetValue(componentData.Key, out var itemType))
                {
                    if (itemType is StandardItemType standardItemType)
                    {
                        blueprintState.PutItem(standardItemType, componentData.Value);
                    }
                }
            }

            blueprintState.Weld(blueprintStateData.Integirty);
        }
    }
}
