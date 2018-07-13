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

        // private Dictionary<Compound, Dictionary<Compound, Func<>>>

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
                    if (originalComponentA == originalComponentB)
                    {
                        continue;
                    }

                    if (reactedMixture.TryGetComponentByCompound(originalComponentA.Compound, out var componentA))
                    {
                        if (reactedMixture.TryGetComponentByCompound(originalComponentB.Compound, out var componentB))
                        {
                            var componentReactionResult = CheckTwoComponents(reactedMixture, componentA, componentB, temperature, elapsedTime);

                            reactedMixture = AlterMixture(reactedMixture, componentReactionResult.ReactedCompounds);
                        }
                    }
                }
            }

            return reactedMixture;
        }

        private Mixture AlterMixture(Mixture mixture, IDictionary<Compound, AmountOfSubstance> compoundsToChange)
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

            return Mixture.FromAbsoluteAmounts(compounds);
        }

        private TwoComponentReactionResult CheckTwoComponents(Mixture mixture, MixtureComponent componentA, MixtureComponent componentB, Temperature temperature, TimeSpan elapsedTime)
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
                        return new TwoComponentReactionResult(Reduce(mixture, oxide, reducer, temperature, elapsedTime));
                    }
                }
            }

            return new TwoComponentReactionResult(originalCompounds);
        }

        private Dictionary<Compound, AmountOfSubstance> Reduce(Mixture mixture, MixtureComponent oxide, MixtureComponent reducer, Temperature temperature, TimeSpan elapsedTime)
        {
            var deltaTemperature = temperature - Temperature.FromKelvin(900);

            if (deltaTemperature < Temperature.Zero)
            {
                deltaTemperature = Temperature.Zero;
            }

            var t = deltaTemperature.InKelvin / 10;

            // Get the first element of the oxide which is not oxygen. It must be the element which will be the result of the reduction.
            var reducedOxideCompound = _compoundList.GetForElement(oxide.Compound.Components.Select(c => c.Element).Single(e => e != _elementList.Oxygen));
            var oxideCompound = oxide.Compound;
            var reducerCompound = reducer.Compound;
            var oxydizedReducerCompound = _compoundList.CO2;

            var oxideOxygenAtomCount = oxide.Compound.GetElementCount(_elementList.Oxygen);
            var oxidizedReducerOxygenAtomCount = oxydizedReducerCompound.GetElementCount(_elementList.Oxygen);
            var metalAtomsPerOxide = oxideCompound.GetElementCount(_elementList.Iron);
            var oxidizedReducerToOxideOxygenAtomCountRatio = (double)oxidizedReducerOxygenAtomCount / oxideOxygenAtomCount;

            var reducedOxideAmount = oxide.Amount;
            if (reducer.Amount * oxidizedReducerToOxideOxygenAtomCountRatio < oxide.Amount)
            {
                reducedOxideAmount = reducer.Amount * oxidizedReducerToOxideOxygenAtomCountRatio;
            }

            reducedOxideAmount = new AmountOfSubstance(Math.Min(elapsedTime.TotalSeconds * t, reducedOxideAmount.Value));

            var oxidizedReducerAmount = reducedOxideAmount / oxidizedReducerToOxideOxygenAtomCountRatio;

            return new Dictionary<Compound, AmountOfSubstance>
            {
                { oxideCompound, oxide.Amount - reducedOxideAmount },
                { reducedOxideCompound, mixture.GetAmountOfCompound(reducedOxideCompound) + reducedOxideAmount * metalAtomsPerOxide },
                { reducerCompound, reducer.Amount - oxidizedReducerAmount },
                { oxydizedReducerCompound, mixture.GetAmountOfCompound(oxydizedReducerCompound) + oxidizedReducerAmount }
            };
        }
    }

    public class TwoComponentReactionParameters
    {
        public Compound Compound1 { get; }
    }

    public class TwoComponentReactionResult
    {
        public Dictionary<Compound, AmountOfSubstance> ReactedCompounds { get; }

        public TwoComponentReactionResult(Dictionary<Compound, AmountOfSubstance> reactedCompounds)
        {
            ReactedCompounds = reactedCompounds;
        }
    }
}
