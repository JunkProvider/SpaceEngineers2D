using System.Collections.Generic;

namespace SpaceEngineers2D.Persistence.DataModel
{
    public class BlockBlueprintStateData : IDataModel
    {
        public double Integirty { get; set; }

        public Dictionary<int, int> Components { get; set; }

        public BlockBlueprintStateData(double integirty, Dictionary<int, int> components)
        {
            Integirty = integirty;
            Components = components;
        }

        public BlockBlueprintStateData()
        {
        }
    }
}
