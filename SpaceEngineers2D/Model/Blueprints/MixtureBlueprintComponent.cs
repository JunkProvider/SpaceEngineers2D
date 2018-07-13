using System;
using SpaceEngineers2D.Chemistry;
using SpaceEngineers2D.Chemistry.Quantities;
using SpaceEngineers2D.Model.Items;

namespace SpaceEngineers2D.Model.Blueprints
{
    public delegate bool DoesMixtureFulfillCriteria(Mixture mixture);

    public class MixtureBlueprintComponent : IBlueprintComponent
    {
        public string DisplayText { get; }

        public Volume RequiredVolume { get; }

        public DoesMixtureFulfillCriteria DoesMixtureFulfillCriteriaFunc { get; }

        public MixtureBlueprintComponent(string displayText, Volume requiredVolume, DoesMixtureFulfillCriteria doesMixtureFulfillCriteriaFunc)
        {
            DisplayText = displayText;
            RequiredVolume = requiredVolume;
            DoesMixtureFulfillCriteriaFunc = doesMixtureFulfillCriteriaFunc;
        }

        public IBlueprintComponentState CreateState()
        {
            return new MixtureBlueprintComponentState(this);
        }

        public BlueprintComponentAddItemResult AddItem(MixtureBlueprintComponentState state, IItem item)
        {
            if (!(item is MixtureItem itemToMixIn))
            {
                return new BlueprintComponentAddItemResult.NotAdded();
            }

            if (!DoesMixtureFulfillCriteriaFunc(itemToMixIn.Mixture))
            {
                return new BlueprintComponentAddItemResult.NotAdded();
            }

            if (state.Item != null && state.Item.Mixture.Volume >= RequiredVolume)
            {
                return new BlueprintComponentAddItemResult.NotAdded();
            }

            var combinedMixture = state.Item != null
                ? itemToMixIn.Mixture.MixWith(state.Item.Mixture)
                : itemToMixIn.Mixture;

            if (combinedMixture.Volume > RequiredVolume)
            {
                return combinedMixture.Extract(RequiredVolume).Match(
                    extracted =>
                    {
                        state.Item = CreateItem(extracted.ExtractedMixture);

                        return new BlueprintComponentAddItemResult.AddedPartial(CreateItem(extracted.RemainingMixture));
                    },
                    depleted =>
                    {
                        throw new InvalidOperationException();
                    }
                );
            }

            state.Item = CreateItem(combinedMixture);
            return new BlueprintComponentAddItemResult.Added();
        }

        private MixtureItem CreateItem(Mixture mixture)
        {
            return new MixtureItem(mixture);
        }
    }
}