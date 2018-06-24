using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Model.Chemicals
{
    public class Mixture
    {
        public static Mixture FromAbsoluteAmounts(IReadOnlyDictionary<Compound, double> compounds)
        {
            var totalAmount = compounds.Values.Sum();
            var components = new HashSet<MixtureComponent>();
            foreach (var compound in compounds)
            {
                components.Add(new MixtureComponent(compound.Key, compound.Value, compound.Value / totalAmount));
            }
            return new Mixture(components);
        }

        private Dictionary<Compound, MixtureComponent> _components;

        public IReadOnlyCollection<MixtureComponent> Components => _components.Values;

        /// <summary>
        /// In mol.
        /// </summary>
        public double Amount => Components.Sum(component => component.Amount);

        /// <summary>
        /// In g.
        /// </summary>
        public double Mass => Components.Sum(component => component.Mass);

        private Mixture(IReadOnlyCollection<MixtureComponent> components)
        {
            if (components == null || !components.Any())
            {
                throw new ArgumentNullException(nameof(components), @"Mixture must have at least one component");
            }

            _components = components.ToDictionary(component => component.Compound);
        }

        public bool HasSameConcentrationsAs(Mixture otherMixture)
        {
            if (otherMixture._components.Count != _components.Count)
            {
                return false;
            }

            foreach (var component in Components)
            {
                if (!otherMixture._components.TryGetValue(component.Compound, out var otherComponent))
                {
                    return false;
                }

                if (Math.Abs(otherComponent.Portion - component.Portion) > double.Epsilon)
                {
                    return false;
                }
            }

            return true;
        }

        public Mixture MixWith(Mixture otherMixture)
        {
            var compounds = new Dictionary<Compound, double>();

            foreach (var component in Components)
            {
                var amount = component.Amount;

                if (otherMixture._components.TryGetValue(component.Compound, out var otherComponent))
                {
                    amount += otherComponent.Amount;
                }

                compounds.Add(component.Compound, amount);
            }

            foreach (var component in otherMixture.Components)
            {
                if (compounds.ContainsKey(component.Compound))
                {
                    continue;
                }

                compounds.Add(component.Compound, component.Amount);
            }

            return FromAbsoluteAmounts(compounds);
        }
    }

    public class MixtureComponent
    {
        public Compound Compound { get; }

        /// <summary>
        /// In mol.
        /// </summary>
        public double Amount { get; }

        /// <summary>
        /// Number greater than 0  and less or equal 1.
        /// </summary>
        public double Portion { get; }

        /// <summary>
        /// In g.
        /// </summary>
        public double Mass => Amount * Compound.Mass;

        public MixtureComponent(Compound compound, double amount, double portion)
        {
            Compound = compound;
            Amount = amount;
            Portion = portion;
        }
    }
}
