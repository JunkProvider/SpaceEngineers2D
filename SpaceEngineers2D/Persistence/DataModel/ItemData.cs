using System.Collections.Generic;

namespace SpaceEngineers2D.Persistence.DataModel
{
    public class ItemData : IDataModel
    {
        public int ItemTypeId { get; set; }

        public Dictionary<string, object> Data { get; set; }

        public ItemData(int itemTypeId, Dictionary<string, object> data)
        {
            ItemTypeId = itemTypeId;
            Data = data;
        }

        public ItemData()
        {
        }
    }
}