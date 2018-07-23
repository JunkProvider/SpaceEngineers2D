using System;
using System.Collections.Generic;
using System.Linq;
using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2D.Chemistry
{
    public class ReactionService
    {
        private readonly ElementList _elementList;

        private readonly CompoundList _compoundList;

        public ReactionService(ElementList elementList, CompoundList compoundList)
        {
            _elementList = elementList;
            _compoundList = compoundList;
        }

        public Mixture Check(Mixture mixture, Temperature temperature, TimeSpan elapsedTime)
        {
            var originalMixture = mixture;
            var reactedMixture = originalMixture;

            foreach (var originalComponentA in originalMixture.Components)
            {
                foreach (var originalComponentB in originalMixture.Components)
                {
                    if (ReferenceEquals(originalComponentA, originalComponentB))
                    {
                        continue;
                    }

                    if (reactedMixture.TryGetComponentByCompound(originalComponentA.Compound, out var componentA))
                    {
                        if (reactedMixture.TryGetComponentByCompound(originalComponentB.Compound, out var componentB))
                        {
                            var componentReactionResult = CheckTwoComponents(reactedMixture, componentA, componentB, elapsedTime);

                            reactedMixture = AlterMixture(reactedMixture, componentReactionResult.ReactedCompounds, componentReactionResult.ThermalEnergyDelta);
                        }
                    }
                }
            }

            return reactedMixture;
        }

        private Mixture AlterMixture(Mixture mixture, IDictionary<Compound, AmountOfSubstance> compoundsToChange, Energy thermalEnergyDelta)
        {
            var compounds = AlterMixtureComponents(mixture, compoundsToChange);
            return Mixture.FromAbsoluteAmounts(compounds, mixture.ThermalEnergy + thermalEnergyDelta);
        }

        private IReadOnlyDictionary<Compound, AmountOfSubstance> AlterMixtureComponents(Mixture mixture, IDictionary<Compound, AmountOfSubstance> compoundsToChange)
        {
            var compounds = new Dictionary<Compound, AmountOfSubstance>();

            foreach (var component in mixture.Components)
            {
                if (compoundsToChange.TryGetValue(component.Compound, out var newAmount))
                {
                    if (newAmount.IsZero)
                    {
                        continue;
                    }

                    compounds.Add(component.Compound, newAmount);
                }
                else
                {
                    compounds.Add(component.Compound, component.Amount);
                }
            }

            foreach (var component in compoundsToChange)
            {
                if (component.Value.IsZero || compounds.ContainsKey(component.Key))
                {
                    continue;
                }

                compounds.Add(component.Key, component.Value);
            }

            return compounds;
        }

        private TwoComponentReactionResult CheckTwoComponents(Mixture mixture, MixtureComponent componentA, MixtureComponent componentB, TimeSpan elapsedTime)
        {
            var originalCompounds = new Dictionary<Compound, AmountOfSubstance>
            {
                { componentA.Compound, componentA.Amount },
                { componentB.Compound, componentB.Amount }
            };

            var componentDict = new Dictionary<Compound, MixtureComponent>
            {
                { componentA.Compound, componentA },
                { componentB.Compound, componentB }
            };

            var oxideCompounds = new HashSet<Compound>
            {
                _compoundList.Fe2O3,
                _compoundList.Fe3O4
            };

            if (componentDict.TryGetValue(_compoundList.C, out var reducer))
            {
                foreach (var oxideCompound in oxideCompounds)
                {
                    if (componentDict.TryGetValue(oxideCompound, out var oxide))
                    {
                        return Reduce(mixture, oxide, reducer, elapsedTime);
                    }
                }
            }

            return new TwoComponentReactionResult(originalCompounds, Energy.Zero);
        }

        private TwoComponentReactionResult Reduce(Mixture mixture, MixtureComponent oxide, MixtureComponent reducer, TimeSpan elapsedTime)
        {
            var oxideACompound = oxide.Compound;
            var reducerACompound = reducer.Compound;
            var oxidizerElement = _elementList.Oxygen;
            var oxideBCompound = _compoundList.CO2;
            var reducerAElement = oxideBCompound.Components.Select(c => c.Element).Single(e => e != oxidizerElement);
            var reducerBElement = oxideACompound.Components.Select(c => c.Element).Single(e => e != oxidizerElement);
            var reducerBCompound = _compoundList.GetForElement(reducerBElement);

            var oxideAOxidizerAtomCount = oxideACompound.GetElementCount(oxidizerElement);
            var oxideBOxidizerAtomCount = oxideBCompound.GetElementCount(oxidizerElement);
            var oxideAReducerBAtomCount = oxideACompound.GetElementCount(reducerBElement);

            var oxideBPerOxideA = oxideAOxidizerAtomCount / (double)oxideBOxidizerAtomCount;
            var reducerAPerOxideA = oxideBCompound.GetElementCount(reducerAElement) * oxideBPerOxideA;
            var reducerBPerOxideA = oxideAReducerBAtomCount / (double)reducerBCompound.GetElementCount(reducerBElement);

            var enthalpyOfFormation = (reducerBCompound.EnthalpyOfFormation * reducerBPerOxideA + oxideBCompound.EnthalpyOfFormation * oxideBPerOxideA) -
                                      (oxideACompound.EnthalpyOfFormation + reducerACompound.EnthalpyOfFormation * reducerBPerOxideA);
  
            var requiredTemperature = Temperature.FromKelvin(1200);
            var requiredEnergy = requiredTemperature * mixture.HeatCapacity;
            var usableEnergy = mixture.ThermalEnergy - requiredEnergy;

            var reactionSpeedFactor = mixture.ThermalEnergy / requiredEnergy - 1;

            if (reactionSpeedFactor <= 0)
            {
                // Available energy is less than required energy so no reaction at all.
                return new TwoComponentReactionResult(mixture.GetAmountsMappedByCompounds(), Energy.Zero);
            }

            var reactedOxideAAmount = oxide.Amount;
            if (reducer.Amount / reducerAPerOxideA < oxide.Amount)
            {
                reactedOxideAAmount = reducer.Amount * reducerAPerOxideA;
            }

            reactedOxideAAmount = AmountOfSubstance.Min(reactedOxideAAmount, elapsedTime.TotalSeconds * reactionSpeedFactor * AmountOfSubstance.FromMol(1));

            if (enthalpyOfFormation > EnthalpyOfFormation.Zero)
            {
                reactedOxideAAmount = AmountOfSubstance.Min(reactedOxideAAmount, usableEnergy / enthalpyOfFormation);
            }
            
            var reactedCompounds = new Dictionary<Compound, AmountOfSubstance>
            {
                { oxideACompound, oxide.Amount - reactedOxideAAmount },
                { reducerBCompound, mixture.GetAmountOfCompound(reducerBCompound) + reactedOxideAAmount * reducerBPerOxideA },
                { reducerACompound, reducer.Amount - reactedOxideAAmount * reducerAPerOxideA },
                { oxideBCompound, mixture.GetAmountOfCompound(oxideBCompound) + reactedOxideAAmount * oxideBPerOxideA }
            };

            var energyDelta = reactedOxideAAmount * -enthalpyOfFormation;

            return new TwoComponentReactionResult(reactedCompounds, energyDelta);
        }

        private class TwoComponentReactionResult
        {
            public IDictionary<Compound, AmountOfSubstance> ReactedCompounds { get; }

            public Energy ThermalEnergyDelta { get; }

            public TwoComponentReactionResult(IDictionary<Compound, AmountOfSubstance> reactedCompounds, Energy thermalEnergyDelta)
            {
                ReactedCompounds = reactedCompounds;
                ThermalEnergyDelta = thermalEnergyDelta;
            }
        }
    }
}
