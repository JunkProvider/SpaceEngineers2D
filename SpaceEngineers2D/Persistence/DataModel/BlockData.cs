using System.Collections.Generic;
using SpaceEngineers2D.Geometry;

namespace SpaceEngineers2D.Persistence.DataModel
{
    public class BlockData : IDataModel
    {
        public IntVector Position { get; set; }

        public int BlockTypeId { get; set; }

        public Dictionary<string, object> Data { get; set; }

        public BlockData(IntVector position, int blockTypeId, Dictionary<string, object> data)
        {
            Position = position;
            BlockTypeId = blockTypeId;
            Data = data;
        }

        public BlockData()
        {
        }
    }
}