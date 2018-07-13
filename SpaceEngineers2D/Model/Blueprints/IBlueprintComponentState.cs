using SpaceEngineers2D.Model.Items;

namespace SpaceEngineers2D.Model.Blueprints
{
    public interface IBlueprintComponentState
    {
        /// <summary>
        /// Something like "3x steel plate" or "1kg iron" or "mixture (min 90% iron, max 10% carbon)".
        /// </summary>
        string DisplayText { get; }

        double Progress { get; }

        bool IsCompleted { get; }

        BlueprintComponentAddItemResult AddItem(IItem item);

        IItem RemoveItem();
    }
}