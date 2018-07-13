using System.Collections.Generic;

namespace SpaceEngineers2DCodeGeneration.Templates
{
    public static class Quantities
    {
        public static IEnumerable<QuantityGenerator.Quantity> Get()
        {
            var quantities = new List<QuantityGenerator.Quantity>();

            quantities.Add(new QuantityGenerator.Quantity(
                name: "AmountOfSubstance",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "mol",
                        name: "Mol",
                        factor: "")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "MolecularMass",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "g/mol",
                        name: "GramPerMol",
                        factor: "")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "MolecularVolume",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "cm³/mol",
                        name: "CubicCentimetersPerMol",
                        factor: "")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "MolecularHeatCapacity",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "J/(mol·K) ",
                        name: "JoulePerMolTimesKelvin",
                        factor: "")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "Mass",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "g",
                        name: "Gram",
                        factor: ""),
                    new QuantityGenerator.Unit(symbol:
                        "kg", name:
                        "KiloGram",
                        factor: "* 1000")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "Volume",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "cm³",
                        name: "CubicCentimeters",
                        factor: ""),
                    new QuantityGenerator.Unit(symbol:
                        "L", name:
                        "Liters",
                        factor: "* 1000")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "Density",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "g/cm³",
                        name: "GramPerCubicCentimeter",
                        factor: ""),
                    new QuantityGenerator.Unit(symbol:
                        "g/L", name:
                        "GramPerLiter",
                        factor: "* 1000")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "Energy",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "J ",
                        name: "Joule",
                        factor: "")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "Temperature",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "K ",
                        name: "Kelvin",
                        factor: "")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "HeatCapacity",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "J/K ",
                        name: "JoulePerKelvin",
                        factor: "")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "Distance",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "m",
                        name: "Meters",
                        factor: ""),
                    new QuantityGenerator.Unit(symbol:
                        "km", name:
                        "KiloMeters",
                        factor: "* 1000")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "Time",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "s",
                        name: "Seconds",
                        factor: ""),
                    new QuantityGenerator.Unit(symbol:
                        "min", name:
                        "Minutes",
                        factor: "* 60"),
                    new QuantityGenerator.Unit(symbol:
                        "h", name:
                        "Hours",
                        factor: "* 3600")
                }));

            quantities.Add(new QuantityGenerator.Quantity(
                name: "Velocity",
                accuracy: "0.00001",
                units: new List<QuantityGenerator.Unit>
                {
                    new QuantityGenerator.Unit(
                        symbol: "m/s",
                        name: "MetersPerSecond",
                        factor: ""),
                    new QuantityGenerator.Unit(symbol:
                        "km/h", name:
                        "KiloMetersPerHour",
                        factor: "/ 3.6")
                }));

            return quantities;
        }
    }
}
