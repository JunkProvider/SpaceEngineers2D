using System.Linq;
using System.Windows.Media;
using SpaceEngineers2D.Model.Chemicals;

namespace SpaceEngineers2D.Model.Items
{
    public class MixtureItem : Item
    {
        private readonly CompoundIconProvider _iconProvider;

        public Mixture Mixture { get; }

        public override float Mass => (float)Mixture.Mass;

        public override float Volume => 1;

        public override ImageSource Icon { get; }

        public MixtureItem(ItemType itemType, CompoundIconProvider iconProvider, Mixture mixture)
            : base(itemType)
        {
            _iconProvider = iconProvider;
            Mixture = mixture;
            Icon = iconProvider.GetIcon(mixture.Components.First().Compound);
        }

        protected override bool Equals(Item other)
        {
            // TODO: Remove this method
            return ReferenceEquals(other, this);
        }

        public override Item Clone()
        {
            // TODO: Remove this method
            return new MixtureItem(ItemType, _iconProvider, Mixture);
        }
    }
}
