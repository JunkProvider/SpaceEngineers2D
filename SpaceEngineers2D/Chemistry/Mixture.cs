using System;
using System.Collections.Generic;
using System.Linq;
using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2D.Chemistry
{
    public class Mixture
    {
        public static Mixture FromAbsoluteAmounts(IReadOnlyDictionary<Compound, AmountOfSubstance> compounds, Energy thermalEnergy)
        {
            var totalAmount = new AmountOfSubstance(compounds.Values.Sum(c => c.Value));
            var components = new HashSet<MixtureComponent>();
            foreach (var compound in compounds)
            {
                components.Add(new MixtureComponent(compound.Key, compound.Value, compound.Value / totalAmount));
            }
            return new Mixture(components, thermalEnergy);
        }

        public static Mixture FromSingleCompound(Compound compound, Volume volume, Temperature temperature)
        {
            var amount = volume / compound.MolecularVolume;
            return FromSingleCompound(compound, volume, temperature * (amount * compound.MolecularHeatCapacity));
        }

        public static Mixture FromSingleCompound(Compound compound, Volume volume, Energy thermalEnergy)
        {
            var components = new HashSet<MixtureComponent>
            {
                new MixtureComponent(compound, volume / compound.MolecularVolume, 1)
            };
            return new Mixture(components, thermalEnergy);
        }

        private readonly Dictionary<Compound, MixtureComponent> _componentDictionary;

        public IReadOnlyCollection<MixtureComponent> Components { get; }

        public Mass Mass => Mass.Sum(Components.Select(component => component.Mass));

        public Volume Volume => Volume.Sum(Components.Select(component => component.Volume));

        public HeatCapacity HeatCapacity => HeatCapacity.Sum(Components.Select(component => component.HeatCapacity));

        public Energy ThermalEnergy { get; }

        public Temperature Temperature => ThermalEnergy / HeatCapacity;

        public IDictionary<Compound, AmountOfSubstance> GetAmountsMappedByCompounds()
        {
            var amounts = new Dictionary<Compound, AmountOfSubstance>();
            foreach (var component in this.Components)
            {
                amounts.Add(component.Compound, component.Amount);                    
            }
            return amounts;
        }

        public AmountOfSubstance GetAmountOfCompound(Compound compound)
        {
            if (TryGetComponentByCompound(compound, out var component))
            {
                return component.Amount;
            }

            return AmountOfSubstance.Zero;
        }

        public bool TryGetComponentByCompound(Compound compound, out MixtureComponent component)
        {
            return _componentDictionary.TryGetValue(compound, out component);
        }

        private Mixture(IReadOnlyCollection<MixtureComponent> components, Energy thermalEnergy)
        {
            if (components == null || !components.Any())
            {
                throw new ArgumentNullException(nameof(components), @"Mixture must have at least one component");
            }

            _componentDictionary = components.ToDictionary(component => component.Compound);
            Components = components.OrderByDescending(component => component.Portion).ToList();

            ThermalEnergy = thermalEnergy;
        }

        public bool HasSameConcentrationsAs(Mixture otherMixture)
        {
            if (otherMixture._componentDictionary.Count != _componentDictionary.Count)
            {
                return false;
            }

            foreach (var component in Components)
            {
                if (!otherMixture._componentDictionary.TryGetValue(component.Compound, out var otherComponent))
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
            var compounds = new Dictionary<Compound, AmountOfSubstance>();

            foreach (var component in Components)
            {
                var amount = component.Amount;

                if (otherMixture._componentDictionary.TryGetValue(component.Compound, out var otherComponent))
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

            return FromAbsoluteAmounts(compounds, ThermalEnergy + otherMixture.ThermalEnergy);
        }

        public IExtractAmountResult Extract(Volume volume)
        {
            if (volume >= Volume)
            {
                return new ExtractResult.Depleted(this);
            }

            var factor = volume / Volume;

            return new ExtractResult.Extracted(Multiply(factor), Multiply(1 - factor));
        }

        private Mixture Multiply(double factor)
        {
            var multipliedComponents = new Dictionary<Compound, AmountOfSubstance>();

            foreach (var component in Components)
            {
                multipliedComponents.Add(component.Compound, component.Amount * factor);
            }

            return FromAbsoluteAmounts(multipliedComponents, ThermalEnergy * factor);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((Mixture) obj);
        }

        public bool Equals(Mixture other)
        {
            return _componentDictionary.SequenceEqual(other._componentDictionary) && ThermalEnergy == other.ThermalEnergy;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((_componentDictionary != null ? _componentDictionary.GetHashCode() : 0) * 397) ^ ThermalEnergy.GetHashCode();
            }
        }

        public override string ToString()
        {
            return Mass.InGram.ToString("0") + "g (" + string.Join(" & ", Components.Select(c => c.Amount.Value.ToString("0") + "mol " + c.Compound.Forumla)) + ")";
        }

        public interface IExtractAmountResult
        {
            void Match(Action<ExtractResult.Extracted> extracted, Action<ExtractResult.Depleted> depleted);

            TResult Match<TResult>(Func<ExtractResult.Extracted, TResult> extracted, Func<ExtractResult.Depleted, TResult> depleted);
        }

        public abstract class ExtractResult : IExtractAmountResult
        {
            public void Match(Action<Extracted> extracted, Action<Depleted> depleted)
            {
                Match(extracted, depleted, nothingExtracted => throw new InvalidOperationException());
            }

            public TResult Match<TResult>(Func<Extracted, TResult> extracted, Func<Depleted, TResult> depleted)
            {
                return Match(extracted, depleted, nothingExtracted => throw new InvalidOperationException());
            }

            public void Match(Action<Extracted> extracted, Action<Depleted> depleted, Action<NothingExtracted> nothingExtracted)
            {
                if (this is Extracted extractedResult)
                {
                    extracted(extractedResult);
                }

                if (this is Depleted depletedResult)
                {
                    depleted(depletedResult);
                }

                if (this is NothingExtracted nothingExtractedResult)
                {
                    nothingExtracted(nothingExtractedResult);
                }

                throw new NotSupportedException();
            }

            public TResult Match<TResult>(Func<Extracted, TResult> extracted, Func<Depleted, TResult> depleted, Func<NothingExtracted, TResult> nothingExtracted)
            {
                if (this is Extracted extractedResult)
                {
                    return extracted(extractedResult);
                }

                if (this is Depleted depletedResult)
                {
                    return depleted(depletedResult);
                }

                if (this is NothingExtracted nothingExtractedResult)
                {
                    return nothingExtracted(nothingExtractedResult);
                }

                throw new NotSupportedException();
            }

            public class Extracted : ExtractResult
            {
                public Mixture ExtractedMixture { get; }

                public Mixture RemainingMixture { get; }

                public Extracted(Mixture extractedMixture, Mixture remainingMixture)
                {
                    ExtractedMixture = extractedMixture;
                    RemainingMixture = remainingMixture;
                }
            }

            public class Depleted : ExtractResult
            {
                public Mixture ExtractedMixture { get; }

                public Depleted(Mixture extractedMixture)
                {
                    ExtractedMixture = extractedMixture;
                }
            }

            public class NothingExtracted : ExtractResult
            {
                
            }
        }
    }
}
