using System.Linq;
using System.Windows.Media;
using SpaceEngineers2D.Model.Chemicals;

namespace SpaceEngineers2D.Model.Items
{
    public class MixtureItem : IItem
    {
        private readonly CompoundIconProvider _iconProvider;

        public Mixture Mixture { get; }

        public string Name => string.Join(", ", Mixture.Components.Select(component => component.Compound.Name));

        public double Amount => Mixture.Amount;

        public double Mass => Mixture.Mass;

        public double Volume => 1;

        public ImageSource Icon { get; }

        public MixtureItem(CompoundIconProvider iconProvider, Mixture mixture)
        {
            _iconProvider = iconProvider;
            Mixture = mixture;
            Icon = iconProvider.GetIcon(mixture.Components.First().Compound);
        }

        public bool ShouldBeAutoCombined(IItem other)
        {
            return other is MixtureItem otherMixtureItem && otherMixtureItem.Mixture.HasSameConcentrationsAs(Mixture);
        }

        public bool CanBeCombinded(IItem other)
        {
            return other is MixtureItem;
        }

        public IItem Combine(IItem other)
        {
            var otherMixtureItem = (MixtureItem) other;
            return new MixtureItem(_iconProvider, Mixture.MixWith(otherMixtureItem.Mixture));
        }
    }
}
