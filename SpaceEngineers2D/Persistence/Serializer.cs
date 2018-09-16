using System.Collections.Generic;
using System.Linq;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Model.BlockBlueprints;
using SpaceEngineers2D.Model.Inventories;
using SpaceEngineers2D.Persistence.DataModel;

namespace SpaceEngineers2D.Persistence
{
    public class Serializer
    {
        public WorldData MapWorld(World world)
        {
            return new WorldData(
                player: new PlayerData(
                    position: world.Player.Position,
                    inventory: MapInventory(world.Player.Inventory)),
                grids: world.Grids.Select(MapGrid).ToList()
            );
        }

        public InventoryData MapInventory(Inventory inventory)
        {
            return new InventoryData(
                slots: inventory.Slots.Select(MapInventorySlot).ToList()
            );
        }

        public InventorySlotData MapInventorySlot(InventorySlot slot)
        {
            ItemStackData itemStackData = null;

            if (slot.ContainsItem)
            {
                var itemDataData = new Dictionary<string, object>();
                slot.ItemStack.Item.ItemType.Save(slot.ItemStack.Item, new DictionaryAccess(itemDataData));

                itemStackData = new ItemStackData(
                    item: new ItemData(
                        itemTypeId: slot.ItemStack.Item.ItemType.Id,
                        data: itemDataData),
                    size: slot.ItemStack.Size);
            }

            return new InventorySlotData(
                id: slot.Id,
                itemStack: itemStackData
            );
        }

        public GridData MapGrid(Grid grid)
        {
            var blockDataList = new List<BlockData>();

            grid.ForEach((block, position) =>
            {
                var blockDataData = new Dictionary<string, object>();

                block.BlockType.Save(this, block, new DictionaryAccess(blockDataData));

                blockDataList.Add(new BlockData(
                    position: position,
                    blockTypeId: block.BlockType.Id,
                    data: blockDataData));


            });

            return new GridData(
                id: grid.Id,
                blocks: blockDataList);
        }

        public BlockBlueprintStateData MapBlockBlueprintState(BlockBlueprintState blueprintState)
        {
            var componentsData = new Dictionary<int, int>();

            foreach (var component in blueprintState.GetComponents())
            {
                var count = component.ActualCount;

                if (count == 0)
                    continue;

                if (componentsData.TryGetValue(component.ItemType.Id, out var existingCount))
                    count += existingCount;

                componentsData[component.ItemType.Id] = count;

            }

            return new BlockBlueprintStateData(
                integirty: blueprintState.Integrity,
                components: componentsData
            );
        }

        public IList<KeyValuePair<string, object>> MapDictionary(IDictionary<string, object> dictionary)
        {
            return dictionary.Select(pair => pair).ToList();
        }
    }
}
