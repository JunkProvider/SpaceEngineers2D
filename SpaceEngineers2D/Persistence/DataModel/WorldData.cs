using System.Collections.Generic;

namespace SpaceEngineers2D.Persistence.DataModel
{
    public class WorldData : IDataModel
    {
        public PlayerData Player { get; set; }

        public List<GridData> Grids { get; set; }

        public WorldData(PlayerData player, List<GridData> grids)
        {
            Player = player;
            Grids = grids;
        }

        public WorldData()
        {
        }
    }
}
