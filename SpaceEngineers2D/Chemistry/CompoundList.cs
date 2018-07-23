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
                var compound = new Compound(
                    name: element.Name,
                    forumla: element.Symbol,
                    components: new Dictionary<Element, int> {{element, 1}},
                    density: element.Density,
                    enthalpyOfFormation: EnthalpyOfFormation.Zero);
                _elementCompounds.Add(element, compound);
                _all.Add(compound);
            }

            C = _elementCompounds[elements.Carbon];

            H2O = new Compound(
                name: "Water",
                forumla: "H2O",
                components: new Dictionary<Element, int>
                {
                    { elements.Hydrogen, 2 }, { elements.Oxygen, 1 },
                },
                density: Density.FromGramPerCubicCentimeter(0.9970474),
                enthalpyOfFormation: EnthalpyOfFormation.FromJoulePerMol(-242 * 1000));

            CO2 = new Compound(
                name: "Carbon Dyoxide",
                forumla: "CO2",
                components: new Dictionary<Element, int>
                {
                    { elements.Carbon, 1 }, { elements.Oxygen, 2 },
                },
                density: Density.FromGramPerLiter(1.977),
                enthalpyOfFormation: EnthalpyOfFormation.FromJoulePerMol(-393 * 1000));

            // Hydrocarbon Gases

            CH4 = new Compound(
                name: "Methane",
                forumla: "CH4",
                components: new Dictionary<Element, int>
                {
                    { elements.Carbon, 1 }, { elements.Hydrogen, 4 }
                },
                density:Density.FromGramPerLiter(0.0657),
                enthalpyOfFormation: EnthalpyOfFormation.FromJoulePerMol(-75 * 1000));

            // Minerals - Iron Ore

            Fe3O4 = new Compound(
                name: "Magnetite",
                forumla: "Fe3O4",
                components: new Dictionary<Element, int>
                {
                    { elements.Iron, 3 }, { elements.Oxygen, 4 }
                },
                density: Density.FromGramPerCubicCentimeter(5.2),
                enthalpyOfFormation: EnthalpyOfFormation.FromJoulePerMol(-1118 * 1000)
            );

            Fe2O3 = new Compound(
                name: "Hematite",
                forumla: "Fe2O3",
                components: new Dictionary<Element, int>
                {
                    { elements.Iron, 2 }, { elements.Oxygen, 3 }
                },
                density: Density.FromGramPerCubicCentimeter(5.26),
                enthalpyOfFormation: EnthalpyOfFormation.FromJoulePerMol(-824 * 1000)
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
