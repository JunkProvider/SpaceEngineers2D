using System;
using SpaceEngineers2D.Model.Items;

namespace SpaceEngineers2D.Model.Blueprints
{
    public class MixtureBlueprintComponentState : IBlueprintComponentState
    {
        private readonly MixtureBlueprintComponent _component;

        public MixtureItem Item { get; set; }

        public string DisplayText => _component.DisplayText;

        public double Progress => Item != null ? Item.Mixture.Volume / _component.RequiredVolume : 0;

        public bool IsCompleted => Item != null && Item.Mixture.Volume == _component.RequiredVolume;

        public MixtureBlueprintComponentState(MixtureBlueprintComponent component)
        {
            _component = component;
        }

        public BlueprintComponentAddItemResult AddItem(IItem item)
        {
            return _component.AddItem(this, item);
        }

        public IItem RemoveItem()
        {
            var removedItem = Item;
            Item = null;
            return removedItem;
        }
    }
}