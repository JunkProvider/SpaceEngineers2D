using System.Linq;
using System.Windows.Media;
using SpaceEngineers2D.Chemistry;
using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2D.Model.Items
{
    public class MixtureItem : IItem
    {
        public Mixture Mixture { get; }

        public string Name => string.Join(", ", Mixture.Components.Select(component => component.Compound.Name));

        public string Tooltip => $"Volume: {Mixture.Volume}\nMass: {Mixture.Mass}\nTemperature: {Mixture.Temperature}\nEnergy: {Mixture.ThermalEnergy}\n\n{string.Join("\n", Mixture.Components.Select(c => (100 * c.Portion).ToString("0") + "% " + c.Compound.Name + " (" + c.Volume + ")"))}";

        public Mass Mass => Mixture.Mass;

        public Volume Volume => Mixture.Volume;

        public ImageSource Icon { get; }

        public MixtureItem(Mixture mixture)
        {
            Mixture = mixture;
            Icon = CompoundIconProvider.GetIcon(mixture.Components.First().Compound);
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
            return new MixtureItem(Mixture.MixWith(otherMixtureItem.Mixture));
        }
    }
}
