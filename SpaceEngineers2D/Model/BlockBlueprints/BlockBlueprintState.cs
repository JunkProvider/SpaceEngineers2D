namespace SpaceEngineers2D.Model.BlockBlueprints
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Inventories;
    using Items;

    public class BlockBlueprintState
    {
        private readonly BlockBlueprint _blueprint;

        private readonly List<BlockBlueprintComponentState> _components;

        public double Integrity { get; private set; }

        public double IntegrityRatio => Integrity / _blueprint.GetIntegrityValueSum();

        public double IntegrityCap => GetIntegrityCap();

        public double IntegrityCapRatio => IntegrityCap / _blueprint.GetIntegrityValueSum();

        public bool Finished => Integrity >= _blueprint.GetIntegrityValueSum();

        public BlockBlueprintState(BlockBlueprint blueprint)
        {
            _blueprint = blueprint;
            _components = blueprint.Components.Select(c => new BlockBlueprintComponentState(c)).ToList();
        }

        public IReadOnlyList<BlockBlueprintComponentState> GetComponents()
        {
            return _components;
        }

        public void PutItem(Item item)
        {
            foreach (var component in _components)
            {
                if (component.Complete)
                    continue;

                if (component.ItemType != item.ItemType)
                    continue;

                component.ActualCount += 1;
                return;
            }

            throw new ArgumentException();
        }

        public void PutItem(StandardItemType itemType, int count)
        {
            foreach (var component in _components)
            {
                if (component.Complete)
                    continue;

                if (component.ItemType != itemType)
                    continue;

                var countToTransfere = Math.Min(component.RemainingCount, count);
                count -= countToTransfere;

                component.ActualCount += countToTransfere;

                if (count == 0)
                    return;
            }
        }

        public bool Weld(Inventory inventory, double integrityIncrease)
        {
            foreach (var component in _components)
            {
                if (component.Complete)
                    continue;

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
        }

        public bool Weld(double integrityIncrease)
        {
            var maxIntegrity = GetIntegrityCap();

            if (Integrity >= maxIntegrity)
            {
                return false;
            }

            Integrity = Math.Min(maxIntegrity, Integrity + integrityIncrease);
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

        public void FinishImmediately()
        {
            foreach (var component in _components)
            {
                component.ActualCount = component.RequiredCount;
            }

            Integrity = _blueprint.GetIntegrityValueSum();
        }

        public ICollection<ItemStack> GetDroppedItems()
        {
            return _components
                .Where(c => c.ActualCount > 0)
                .Select(c => new ItemStack(c.ItemType.Instantiate(), c.ActualCount))
                .ToList();
        }

        private double GetIntegrityCap()
        {
            return _components.Sum(c => c.ActualIntegrityValue);
        }
    }
}
