using System.Collections.Generic;

namespace SpaceEngineers2D.Persistence.DataModel
{
    public class WorldData : IDataModel
    {
        public List<GridData> Grids { get; set; }

        public List<EntityData> Entities { get; set; }

        public WorldData(List<EntityData> entities, List<GridData> grids)
        {
            Entities = entities;
            Grids = grids;
        }

        public WorldData()
        {
        }
    }
}
