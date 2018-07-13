using SpaceEngineers2D.Model.Blueprints;

namespace SpaceEngineers2D.Model.BlockBlueprints
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Inventories;
    using Items;

    public class BlockBlueprintState
    {
        private readonly BlockBlueprint _blockBlueprint;

        private readonly BlueprintState _blueprintState;

        private readonly List<BlockBlueprintComponentState> _blockComponents;

        public double Integrity { get; private set; }

        public double IntegrityRatio => Integrity / _blockBlueprint.MaxIntegrity;

        public double IntegrityCap => GetIntegrityCap();

        public double IntegrityCapRatio => IntegrityCap / _blockBlueprint.MaxIntegrity;

        public bool Finished => Integrity >= _blockBlueprint.MaxIntegrity;

        public BlockBlueprintState(BlockBlueprint blockBlueprint)
        {
            _blockBlueprint = blockBlueprint;
            _blueprintState = blockBlueprint.Blueprint.CreateState();

            var components = new List<BlockBlueprintComponentState>();

            for (var i = 0; i < blockBlueprint.Components.Count; i++)
            {
                components.Add(new BlockBlueprintComponentState(blockBlueprint.Components[i], _blueprintState.Components[i]));
            }

            _blockComponents = components;
        }

        public void PutAllPossible(Inventory inventory)
        {
            foreach (var slot in inventory.Slots)
            {
                if (!slot.ContainsItem)
                {
                    continue;
                }

                var remainingItem = PutItem(slot.Item);
                slot.Item = remainingItem;
            }
        }

        public IItem PutItem(IItem item)
        {
            foreach (var blockComponent in _blockComponents)
            {
                var result = blockComponent.AddItem(item);

                var doBreak = result.Match(
                    added =>
                        {
                            return true;
                        },
                    addedPartial =>
                        {
                            item = addedPartial.Remaining;
                            return false;
                        },
                    notAdded =>
                        {
                            return false;
                        });

                if (doBreak)
                {
                    return null;
                }
            }

            return item;
        }

        public bool Weld(Inventory inventory, float integrityIncrease)
        {
            foreach (var slot in inventory.Slots)
            {
                if (!slot.ContainsItem)
                {
                    continue;
                }

                var remainingItem = PutItem(slot.Item);
                slot.Item = remainingItem;
            }

            var integrityCap = GetIntegrityCap();

            if (Integrity >= integrityCap)
            {
                return false;
            }

            Integrity = Math.Min(integrityCap, Integrity + integrityIncrease);
            return true;
        }

        /* public bool Grind(Inventory inventory, float integrityDecrease)
        {
            Integrity = Math.Max(0, Integrity - integrityDecrease);

            float f = 0;
            foreach (var component in _components)
            {
                if (component.ActualCount == 0)
                    continue;

                f += component.ActualIntegrityValue;

                if (f <= Integrity)
                {
                    continue;
                }

                if (inventory.TryTakeNOfType(component.ItemType, component.RemainingCount, out var stack))
                    component.ActualCount += stack.Size;
            }

            var maxIntegrity = GetIntegrityCap();

            if (Integrity >= maxIntegrity)
            {
                return false;
            }

            Integrity = Math.Min(maxIntegrity, Integrity + integrityIncrease);
            return true;
        }*/

        public void Damage(float integrityDecrease)
        {
            Integrity = Math.Max(0, Integrity - integrityDecrease);

            /* var f = 0f;

            for (var i = 0; i < _blueprint.Components.Count; i++)
            {
                var requiredCount = _blueprint.Components[i].Count;
                var actualCount = _components[i];
                var integrityValue = _blueprint.Components[i].IntegrityValue;

                for (var j = 0; j < requiredCount; j++)
                {
                    f += integrityValue;

                    if (f > Integrity && actualCount > j)
                    {
                        actualCount = j;
                    }
                }

                _components[i] = actualCount; 
            } */
        }

        public IReadOnlyList<IItem> GetDroppedItems()
        {
            return _blockComponents.Select(c => c.RemoveItem()).Where(i => i != null).ToList();
        }

        private double GetIntegrityCap()
        {
            return _blockComponents.Sum(c => c.IntegrityCap);
        }
    }
}
