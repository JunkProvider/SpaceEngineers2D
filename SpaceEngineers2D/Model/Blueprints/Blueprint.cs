using SpaceEngineers2D.Model.Chemicals;

namespace SpaceEngineers2D.Model.Blueprints
{
    public class Blueprint
    {
    }

    public interface IBlueprintComponent
    {
        /// <summary>
        /// Something like "3x steel plate" or "1kg iron" or "mixture (min 90% iron, max 10% carbon)".
        /// </summary>
        string DisplayText { get; }
    }

    public delegate bool DoesMixtureFulfillCriteria(Mixture mixture);

    public class MixtureBlueprintComponent : IBlueprintComponent
    {
        public string DisplayText { get; }

        public double RequiredVolume { get; }

        public DoesMixtureFulfillCriteria DoesMixtureFulfillCriteriaFunc { get; }
    }
}
