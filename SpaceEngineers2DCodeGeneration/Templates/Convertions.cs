using System.Collections.Generic;

namespace SpaceEngineers2DCodeGeneration.Templates
{
    public static class Convertions
    {
        public static IEnumerable<QuantityGenerator.Convertion> Get()
        {
            return new List<QuantityGenerator.Convertion>
            {
                new QuantityGenerator.Convertion(
                    quantityA: "AmountOfSubstance",
                    @operator: "*",
                    quantityB: "MolecularMass",
                    result: "Mass",
                    formula: "return Mass.FromGram(amountOfSubstance.InMol * molecularMass.InGramPerMol);"),
                new QuantityGenerator.Convertion(
                    quantityA: "AmountOfSubstance",
                    @operator: "*",
                    quantityB: "MolecularVolume",
                    result: "Volume",
                    formula: "return Volume.FromCubicCentimeters(amountOfSubstance.InMol * molecularVolume.InCubicCentimetersPerMol);"),
                new QuantityGenerator.Convertion(
                    quantityA: "AmountOfSubstance",
                    @operator: "*",
                    quantityB: "MolecularHeatCapacity",
                    result: "HeatCapacity",
                    formula: "return HeatCapacity.FromJoulePerKelvin(amountOfSubstance.InMol * molecularHeatCapacity.InJoulePerMolTimesKelvin);"),
                new QuantityGenerator.Convertion(
                    quantityA: "MolecularMass",
                    @operator: "/",
                    quantityB: "Density",
                    result: "MolecularVolume",
                    formula: "return MolecularVolume.FromCubicCentimetersPerMol(molecularMass.InGramPerMol / density.InGramPerCubicCentimeter);"),

                new QuantityGenerator.Convertion(
                    quantityA: "Volume",
                    @operator: "/",
                    quantityB: "MolecularVolume",
                    result: "AmountOfSubstance",
                    formula: "return AmountOfSubstance.FromMol(volume.InCubicCentimeters/ molecularVolume.InCubicCentimetersPerMol);"),

                new QuantityGenerator.Convertion(
                    quantityA: "Volume",
                    @operator: "*",
                    quantityB: "Density",
                    result: "Mass",
                    formula: "return Mass.FromGram(volume.InCubicCentimeters / density.InGramPerCubicCentimeter);"),
                new QuantityGenerator.Convertion(
                    quantityA: "Mass",
                    @operator: "/",
                    quantityB: "Density",
                    result: "Volume",
                    formula: "return Volume.FromCubicCentimeters(mass.InGram / density.InGramPerCubicCentimeter);"),
                new QuantityGenerator.Convertion(
                    quantityA: "Energy",
                    @operator: "/",
                    quantityB: "HeatCapacity",
                    result: "Temperature",
                    formula: "return Temperature.FromKelvin(energy.InJoule / heatCapacity.InJoulePerKelvin);"),
                new QuantityGenerator.Convertion(
                    quantityA: "Temperature",
                    @operator: "*",
                    quantityB: "HeatCapacity",
                    result: "Energy",
                    formula: "return Energy.FromJoule(temperature.InKelvin * heatCapacity.InJoulePerKelvin);"),
                new QuantityGenerator.Convertion(
                    quantityA: "AmountOfSubstance",
                    @operator: "*",
                    quantityB: "EnthalpyOfFormation",
                    result: "Energy",
                    formula: "return Energy.FromJoule(amountOfSubstance.InMol * enthalpyOfFormation.InJoulePerMol);"),
                new QuantityGenerator.Convertion(
                    quantityA: "Energy",
                    @operator: "/",
                    quantityB: "EnthalpyOfFormation",
                    result: "AmountOfSubstance",
                    formula: "return AmountOfSubstance.FromMol(energy.InJoule / enthalpyOfFormation.InJoulePerMol);"),

                /* new QuantityGenerator.Convertion(
                    quantityA: "Distance",
                    @operator: "/",
                    quantityB: "Velocity",
                    result: "Time",
                    formula: "return Time.FromSeconds(distance.InMeters / velocity.InMetersPerSecond);"), */
            };
        }
    }
}
