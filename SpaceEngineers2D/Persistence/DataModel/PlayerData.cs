using SpaceEngineers2D.Geometry;

namespace SpaceEngineers2D.Persistence.DataModel
{
    public class PlayerData : IDataModel
    {
        public IntVector Position { get; set; } 

        public InventoryData Inventory { get; set; }

        public PlayerData(IntVector position, InventoryData inventory)
        {
            Position = position;
            Inventory = inventory;
        }

        public PlayerData()
        {
        }
    }
}