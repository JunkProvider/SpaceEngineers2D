using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Model.Chemicals
{
    public class Mixture
    {
        public IReadOnlyList<MixstureComponent> Components { get; }

        /// <summary>
        /// In mol.
        /// </summary>
        public double Amount => Components.Sum(component => component.Amount);

        /// <summary>
        /// In g.
        /// </summary>
        public double Mass => Components.Sum(component => component.Mass);

        public Mixture(IReadOnlyList<MixstureComponent> components)
        {
            if (components == null || !components.Any())
            {
                throw new ArgumentNullException(nameof(components), @"Mixture must have at least one component");
            }

            Components = components;
        }
    }

    public class MixstureComponent
    {
        public Compound Compound { get; }

        /// <summary>
        /// In mol.
        /// </summary>
        public double Amount { get; }

        /// <summary>
        /// In g.
        /// </summary>
        public double Mass => Amount * Compound.Mass;

        public MixstureComponent(double amount, Compound compound)
        {
            Amount = amount;
            Compound = compound;
        }
    }
}
