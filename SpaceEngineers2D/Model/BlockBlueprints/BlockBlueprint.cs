namespace SpaceEngineers2D.Model.BlockBlueprints
{
    using System.Collections.Generic;
    using System.Linq;

    public class BlockBlueprint
    {
        public IList<BlockBlueprintComponent> Components { get; }

        public BlockBlueprint(IList<BlockBlueprintComponent> components)
        {
            Components = components;
        }

        public float GetIntegrityValueSum()
        {
            return Components.Sum(c => c.Count * c.IntegrityValue);
        }
    }
}
