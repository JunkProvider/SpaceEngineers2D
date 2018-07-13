using SpaceEngineers2D.Model.Blueprints;

namespace SpaceEngineers2D.Model.BlockBlueprints
{
    using Items;

    public class BlockBlueprintComponent
    {
        public IBlueprintComponent BlueprintComponent { get; }

        public double IntegrityValue { get; }

        public BlockBlueprintComponent(IBlueprintComponent blueprintComponent, double integrityValue)
        {
            BlueprintComponent = blueprintComponent;
            IntegrityValue = integrityValue;
        }
    }
}
