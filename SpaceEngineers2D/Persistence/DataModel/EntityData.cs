using System.Collections.Generic;
using SpaceEngineers2D.Geometry;

namespace SpaceEngineers2D.Persistence.DataModel
{
    public class EntityData : IDataModel
    {
        public int EntityTypeId { get; set; }

        public IntVector Position { get; set; }

        public Dictionary<string, object> Data { get; set; }
        
        public EntityData(int entityTypeId, IntVector position, Dictionary<string, object> data)
        {
            EntityTypeId = entityTypeId;
            Position = position;
            Data = data;
        }

        public EntityData()
        {
        }
    }
}