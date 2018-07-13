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

        // public AmountOfSubstance AmountOfSubstance => new AmountOfSubstance(Components.Sum(component => component.Count.Value));

        public MolecularMass Mass => MolecularMass.Sum(Components.Select(component => component.Mass));

        public MolecularVolume Volume => Mass / Density;

        public Density Density { get; }

        public HeatCapacity HeatCapacity => new HeatCapacity(Components.Sum(component => component.Element.HeatCapacity.Value));

        public Compound(string name, string forumla, IReadOnlyDictionary<Element, int> components, Density density)
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

        public MolecularMass Mass => Element.Mass * Count;

        public CompountComponent(Element element, int count)
        {
            Element = element;
            Count = count;
        }
    }
}
