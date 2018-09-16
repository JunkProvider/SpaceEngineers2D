using System.Collections.Generic;

namespace SpaceEngineers2D.Persistence.DataModel
{
    public class InventoryData : IDataModel
    {
        public List<InventorySlotData> Slots { get; set; }

        public InventoryData(List<InventorySlotData> slots)
        {
            Slots = slots;
        }

        public InventoryData()
        {
        }
    }
}