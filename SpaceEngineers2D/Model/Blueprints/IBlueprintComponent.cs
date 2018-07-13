namespace SpaceEngineers2D.Model.Blueprints
{
    public interface IBlueprintComponent
    {
        /// <summary>
        /// Something like "3x steel plate" or "1kg iron" or "mixture (min 90% iron, max 10% carbon)".
        /// </summary>
        string DisplayText { get; }

        IBlueprintComponentState CreateState();
    }
}