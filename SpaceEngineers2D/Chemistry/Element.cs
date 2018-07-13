﻿using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2D.Chemistry
{
    public class Element
    {
        public string Symbol { get; }

        public string Name { get; }

        public int Group { get; }

        public int Period { get; }

        public int Electrons { get; }

        public MolecularMass Mass { get; }

        public MolecularVolume Volume => Mass / Density;

        public Density Density { get; }

        public MolecularHeatCapacity HeatCapacity { get; }

        public ElementClassification Classification { get; }

        public bool IsMetal => Classification == ElementClassification.Metal;

        public Element(string symbol, string name, int @group, int period, int electrons, MolecularMass mass, Density density, ElementClassification classification, MolecularHeatCapacity heatCapacity)
        {
            Symbol = symbol;
            Name = name;
            Group = @group;
            Period = period;
            Electrons = electrons;
            Mass = mass;
            Density = density;
            Classification = classification;
            HeatCapacity = heatCapacity;
        }
    }
}
