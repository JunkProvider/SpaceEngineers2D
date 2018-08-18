using System;
using System.Collections.Generic;
using System.Linq;
using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2D.Chemistry
{
    public class Compound
    {
        private readonly Dictionary<Element, CompountComponent> _componentDict;

        public string Name { get; }

        public string Forumla { get; }

        public IReadOnlyCollection<CompountComponent> Components { get; }

        public MolecularMass MolecularMass => MolecularMass.Sum(Components.Select(component => component.MolecularMass));

        public MolecularVolume MolecularVolume => MolecularMass / Density;

        public Density Density { get; }

        public EnthalpyOfFormation EnthalpyOfFormation { get; }

        public MolecularHeatCapacity MolecularHeatCapacity => MolecularHeatCapacity.Sum(Components.Select(component => component.MolecularHeatCapacity));

        public Temperature MeltingPoint => Temperature.Average(Components.Select(component => component.Element.MeltingPoint));

        public Temperature BoilingPoint => Temperature.Average(Components.Select(component => component.Element.BoilingPoint));

        public Compound(string name, string forumla, IReadOnlyDictionary<Element, int> components, Density density, EnthalpyOfFormation enthalpyOfFormation)
        {
            if (components == null || !components.Any())
            {
                throw new ArgumentNullException(nameof(components), @"Compunt must have at least one element.");
            }

            Name = name;
            Forumla = forumla;
            Components = components.Select(component => new CompountComponent(component.Key, component.Value)).ToList();
            _componentDict = Components.ToDictionary(component => component.Element);
            Density = density;
            EnthalpyOfFormation = enthalpyOfFormation;
        }

        public int GetElementCount(Element element)
        {
            return GetComponentByElement(element).Count;
        }

        public override string ToString()
        {
            return Forumla;
        }

        private CompountComponent GetComponentByElement(Element element)
        {
            return _componentDict[element];
        }
    }

    public class CompountComponent
    {
        public Element Element { get; }

        public int Count { get; }

        public MolecularMass MolecularMass => Element.Mass * Count;

        public MolecularHeatCapacity MolecularHeatCapacity => Element.MolecularHeatCapacity * Count;

        public CompountComponent(Element element, int count)
        {
            Element = element;
            Count = count;
        }
    }
}
