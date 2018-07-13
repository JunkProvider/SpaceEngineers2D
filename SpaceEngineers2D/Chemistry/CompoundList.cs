using System.Collections.Generic;
using System.Linq;
using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2D.Chemistry
{
    public class CompoundList
    {
        private readonly Dictionary<Element, Compound> _elementCompounds = new Dictionary<Element, Compound>();

        private readonly HashSet<Compound> _all = new HashSet<Compound>();

        public Compound C { get; }

        public Compound H2O { get; }

        public Compound CO2 { get; }

        public Compound CH4 { get; }

        public Compound Fe3O4 { get; }

        public Compound Fe2O3 { get; }

        // public Compound Siderite { get; }

        public CompoundList(ElementList elements)
        {
            foreach (var element in elements.GetAll())
            {
                var compound = new Compound(element.Name, element.Symbol, new Dictionary<Element, int> {{element, 1}}, element.Density);
                _elementCompounds.Add(element, compound);
                _all.Add(compound);
            }

            C = _elementCompounds[elements.Carbon];

            H2O = new Compound("Water", "H2O", new Dictionary<Element, int>
            {
                { elements.Hydrogen, 2 }, { elements.Oxygen, 1 },
            }, Density.FromGramPerCubicCentimeter(0.9970474));

            CO2 = new Compound("Carbon Dyoxide", "CO2", new Dictionary<Element, int>
            {
                { elements.Carbon, 1 }, { elements.Oxygen, 2 },
            }, Density.FromGramPerLiter(1.977));

            // Hydrocarbon Gases

            CH4 = new Compound("Methane", "CH4", new Dictionary<Element, int>
            {
                { elements.Carbon, 1 }, { elements.Hydrogen, 4 }
            }, Density.FromGramPerLiter(0.0657));

            // Minerals - Iron Ore

            Fe3O4 = new Compound(
                "Magnetite",
                "Fe3O4",
                new Dictionary<Element, int>
                {
                    { elements.Iron, 3 }, { elements.Oxygen, 4 }
                },
                Density.FromGramPerCubicCentimeter(5.2)
            );

            Fe2O3 = new Compound(
                "Hematite",
                "Fe2O3",
                new Dictionary<Element, int>
                    {
                        { elements.Iron, 2 }, { elements.Oxygen, 3 }
                    },
                Density.FromGramPerCubicCentimeter(5.26)
            );

            foreach (var compound in GetType().GetProperties().Select(property => (Compound)property.GetValue(this)))
            {
                if (!_all.Contains(compound))
                {
                    _all.Add(compound);
                }
            }
        }

        public Compound GetForElement(Element element)
        {
            return _elementCompounds[element];
        }

        public IReadOnlyCollection<Compound> GetAll()
        {
            return _all;
        }
    }
}
