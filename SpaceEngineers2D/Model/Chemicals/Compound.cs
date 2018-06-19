using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Model.Chemicals
{
    public class Compound
    {
        public string Name { get; }

        public string Forumla { get; }

        public IReadOnlyDictionary<Element, int> Elements { get; }

        public double Mass => Elements.Sum(element => element.Value * element.Key.Mass);

        public Compound(string name, string forumla, IReadOnlyDictionary<Element, int> elements)
        {
            if (elements == null || !elements.Any())
            {
                throw new ArgumentNullException(nameof(elements), @"Compunt must have at least one element.");
            }

            Name = name;
            Forumla = forumla;
            Elements = elements;
        }
    }
}
