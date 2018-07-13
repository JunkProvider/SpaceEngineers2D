using System.Collections.Generic;
using System.Linq;
using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2D.Chemistry
{
    public class ElementList
    {
        private readonly IList<Element> _all;

        public Element Hydrogen { get; } = new Element(
            symbol: "H",
            name: "Hydrogen",
            group: 1,
            period: 1,
            electrons: 1,
            mass: MolecularMass.FromGramPerMol(1.008),
            density: Density.FromGramPerLiter(0.08988),
            classification: ElementClassification.NonMetal,
            molecularHeatCapacity: MolecularHeatCapacity.FromJoulePerMolTimesKelvin(28.836));

        /* public Element Helium { get; } = new Element(
            symbol: "He",
            name: "Helium",
            group: 2,
            period: 1,
            electrons: 2,
            mass: MolecularMass.FromGramPerMol(4.002),
            density: Density.FromGramPerLiter(0.1786),
            classification: ElementClassification.NonMetal); */


        public Element Carbon { get; } = new Element(
            symbol: "C", 
            name: "Carbon", 
            @group: 14, 
            period: 2, 
            electrons: 6,
            mass: MolecularMass.FromGramPerMol(12.011), 
            density: Density.FromGramPerCubicCentimeter(2.267),
            classification: ElementClassification.NonMetal,
            molecularHeatCapacity: MolecularHeatCapacity.FromJoulePerMolTimesKelvin(8.517));

        // public Element Nitrogen { get; } = new Element("N", "Nitrogen", 15, 2, 7, MolecularMass.FromGramPerMol(14.0067), Density.FromGramPerLiter(1.2504), ElementClassification.NonMetal);

        public Element Oxygen { get; } = new Element(
            symbol: "O",
            name: "Oxygen", 
            @group: 16, 
            period: 2, 
            electrons: 8,
            mass: MolecularMass.FromGramPerMol(15.999),
            density: Density.FromGramPerLiter(1.429),
            classification: ElementClassification.NonMetal,
            molecularHeatCapacity: MolecularHeatCapacity.FromJoulePerMolTimesKelvin(29.378 / 2));


        public Element Iron { get; } = new Element(
            symbol: "Fe", 
            name: "Iron", 
            @group: 8, 
            period: 4, 
            electrons: 26, 
            mass: MolecularMass.FromGramPerMol(55.845),
            density: Density.FromGramPerCubicCentimeter(7.874),
            classification: ElementClassification.Metal,
            molecularHeatCapacity: MolecularHeatCapacity.FromJoulePerMolTimesKelvin(25.10));

        public ElementList()
        {
            _all = GetType().GetProperties().Select(property => (Element)property.GetValue(this)).ToList();
        }

        public IList<Element> GetAll()
        {
            return _all;
        }

        // Mass:    55.845g/mol
        // Density: 7.875g/cm³
        // Volume:  cm³/mol


        // Volume: 55.845g/mol / 7.876g/cm³
    }
}
