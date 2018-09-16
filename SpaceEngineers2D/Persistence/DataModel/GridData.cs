using System.Collections.Generic;

namespace SpaceEngineers2D.Persistence.DataModel
{
    public class GridData : IDataModel
    {
        public int Id { get; set; }

        public List<BlockData> Blocks { get; set; }

        public GridData(int id, List<BlockData> blocks)
        {
            Id = id;
            Blocks = blocks;
        }

        public GridData()
        {
        }
    }
}