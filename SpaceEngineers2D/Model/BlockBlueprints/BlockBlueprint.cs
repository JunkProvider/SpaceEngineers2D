using SpaceEngineers2D.Model.Blueprints;

namespace SpaceEngineers2D.Model.BlockBlueprints
{
    using System.Collections.Generic;
    using System.Linq;

    public class BlockBlueprint
    {
        public Blueprint Blueprint { get; }

        public IReadOnlyList<BlockBlueprintComponent> Components { get; }

        public double MaxIntegrity => Components.Sum(c => c.IntegrityValue);

        public BlockBlueprint(IReadOnlyList<BlockBlueprintComponent> components)
        {
            Blueprint = new Blueprint(components.Select(c => c.BlueprintComponent).ToList());
            Components = components;
        }
    }
}
