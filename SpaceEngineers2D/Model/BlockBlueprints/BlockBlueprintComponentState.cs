using SpaceEngineers2D.Model.Blueprints;
using SpaceEngineers2D.Model.Items;

namespace SpaceEngineers2D.Model.BlockBlueprints
{
    public class BlockBlueprintComponentState
    {
        private readonly BlockBlueprintComponent _component;

        private readonly IBlueprintComponentState _blueprintComponentState;

        public double IntegrityCap => _blueprintComponentState.Progress * _component.IntegrityValue;

        public bool IsCompleted => _blueprintComponentState.IsCompleted;

        public BlockBlueprintComponentState(BlockBlueprintComponent component, IBlueprintComponentState blueprintComponentState)
        {
            _component = component;
            _blueprintComponentState = blueprintComponentState;
        }

        public BlueprintComponentAddItemResult AddItem(IItem item)
        {
            return _blueprintComponentState.AddItem(item);
        }

        public IItem RemoveItem()
        {
            return _blueprintComponentState.RemoveItem();
        }
    }
}