using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SpaceEngineers2D.Model.Chemicals
{
    public class CompoundList
    {
        private List<Compound> _all = new List<Compound>();

        public Compound Water { get; }

        public Compound Methane { get; }

        public Compound Magnetite { get; }

        public Compound Hematite { get; }

        public Compound Siderite { get; }

        public CompoundList(ElementList elements)
        {
            foreach (var element in elements.GetAll())
            {
                _all.Add(new Compound(element.Name, element.Symbol, new Dictionary<Element, int> { { element, 1 } }));
            }

            Water = new Compound("Water", "H2O", new Dictionary<Element, int>
            {
                { elements.Hydrogen, 2 }, { elements.Oxygen, 1 }
            });

            // Hydrocarbon Gases

            Methane = new Compound("Methane", "CH4", new Dictionary<Element, int>
            {
                { elements.Carbon, 1 }, { elements.Hydrogen, 4 }
            });

            // Minerals - Iron Ore

            Magnetite = new Compound("Magnetite", "Fe3O4", new Dictionary<Element, int>
            {
                { elements.Iron, 3 }, { elements.Hydrogen, 4 }
            });

            Hematite = new Compound("Hematite", "Fe2O3", new Dictionary<Element, int>
            {
                { elements.Iron, 2 }, { elements.Hydrogen, 3 }
            });

            /* Goethite = new Compound("Goethite", "FeO(OH)", new Dictionary<Element, int>
            {
                { elements.Iron, 1 }, { elements.Oxygen, 2 }, { elements.Hydrogen, 1 }
            });

            Limonite = new Compound("Limonite", "FeO(OH)·n(H2O)", new Dictionary<Element, int>
            {
                { elements.Iron, 1 }, { elements.Oxygen, 3 }, { elements.Hydrogen, 3 }
            }); */

            Siderite = new Compound("Siderite", "FeCO3", new Dictionary<Element, int>
            {
                { elements.Iron, 1 }, { elements.Carbon, 1 }, { elements.Oxygen, 3 }
            });

            _all = _all.Concat(GetType().GetProperties().Select(property => (Compound)property.GetValue(this))).ToList();
        }

        public IList<Compound> GetAll()
        {
            return _all;
        }
    }
}
