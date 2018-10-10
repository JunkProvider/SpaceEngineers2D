using System.Linq;
using System.Windows.Media;
using SpaceEngineers2D.Model.Blocks;
using SpaceEngineers2D.Persistence;
using SpaceEngineers2D.Persistence.DataModel;

namespace SpaceEngineers2D.Model.Entities
{
    public class PlayerType : EntityType<Player>
    {
        public PlayerType(int id, string name, ImageSource image)
            : base(id, name, image)
        {
        }

        public Player Instantiate(BlockTypes blockTypes)
        {
            var player = new Player(this);

            player.BlueprintSlots[0].BlueprintedBlock = blockTypes.Concrete;
            player.BlueprintSlots[1].BlueprintedBlock = blockTypes.IronPlate;
            player.SelectBlueprintSlot(player.BlueprintSlots.First());

            return player;
        }

        public override IEntity Load(LoadContext context, DictionaryAccess data)
        {
            var player = Instantiate(context.BlockTypes);

            if (data.TryGetClassNotNull("inventory", out InventoryData inventoryData))
            {
                context.Deserializer.MapInventory(player.Inventory, inventoryData);
            }

            return player;
        }

        public override void Save(Serializer serializer, Player player, DictionaryAccess data)
        {
            base.Save(serializer, player, data);
            data.Set("inventory", serializer.MapInventory(player.Inventory));
        }
    }
}
