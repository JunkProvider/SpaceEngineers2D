using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineers2D.Chemistry;
using SpaceEngineers2D.Chemistry.Quantities;

namespace SpaceEngineers2DTest.Chemistry.ReactionService
{
    [TestClass]
    public class IronOxideReductionTest
    {
        private Temperature Temperature => Temperature.FromKelvin(1000);

        private TimeSpan TimeSpan => TimeSpan.FromDays(365);

        [TestMethod]
        public void TestFullFe2O3Reduction()
        {
            var elements = new ElementList();
            var compounds = new CompoundList(elements);
            var service = new SpaceEngineers2D.Chemistry.ReactionService(elements, compounds);

            var originalMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.Fe2O3, new AmountOfSubstance(2)},
                {compounds.GetForElement(elements.Carbon), new AmountOfSubstance(3)},
            }, Energy.FromKiloJoule(2000));

            var reactedMixture = service.Check(originalMixture, Temperature, TimeSpan);

            var expectedReactedMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.GetForElement(elements.Iron), new AmountOfSubstance(4)},
                {compounds.CO2, new AmountOfSubstance(3)},
            }, Energy.FromKiloJoule(2000));

            Assert.AreEqual(expectedReactedMixture, reactedMixture);
        }

        /*[TestMethod]
        public void TestFe2O3ReductionWithExcessiveOxide()
        {
            var elements = new ElementList();
            var compounds = new CompoundList(elements);
            var service = new SpaceEngineers2D.Chemistry.ReactionService(elements, compounds);

            var originalMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.Fe2O3, new AmountOfSubstance(3)},
                {compounds.GetForElement(elements.Carbon), new AmountOfSubstance(3)},
            });

            var reactedMixture = service.Check(originalMixture, Temperature, TimeSpan);

            var expectedReactedMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.Fe2O3, new AmountOfSubstance(1)},
                {compounds.GetForElement(elements.Iron), new AmountOfSubstance(4)},
                {compounds.CO2, new AmountOfSubstance(3)},
            });

            Assert.AreEqual(expectedReactedMixture, reactedMixture);
        }

        [TestMethod]
        public void TestFe2O3ReductionWithExcessiveReducer()
        {
            var elements = new ElementList();
            var compounds = new CompoundList(elements);
            var service = new SpaceEngineers2D.Chemistry.ReactionService(elements, compounds);

            var originalMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.Fe2O3, new AmountOfSubstance(2)},
                {compounds.GetForElement(elements.Carbon), new AmountOfSubstance(5)},
            });

            var reactedMixture = service.Check(originalMixture, Temperature, TimeSpan);

            var expectedReactedMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.GetForElement(elements.Carbon), new AmountOfSubstance(2)},
                {compounds.GetForElement(elements.Iron), new AmountOfSubstance(4)},
                {compounds.CO2, new AmountOfSubstance(3)},
            });

            Assert.AreEqual(expectedReactedMixture, reactedMixture);
        }

        [TestMethod]
        public void TestFullFe3O4Reduction()
        {
            var elements = new ElementList();
            var compounds = new CompoundList(elements);
            var service = new SpaceEngineers2D.Chemistry.ReactionService(elements, compounds);

            var originalMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.Fe3O4, new AmountOfSubstance(2)},
                {compounds.GetForElement(elements.Carbon), new AmountOfSubstance(4)},
            });

            var reactedMixture = service.Check(originalMixture, Temperature, TimeSpan);

            var expectedReactedMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.GetForElement(elements.Iron), new AmountOfSubstance(6)},
                {compounds.CO2, new AmountOfSubstance(4)},
            });

            Assert.AreEqual(expectedReactedMixture, reactedMixture);
        }

        [TestMethod]
        public void TestFe3O4ReductionWithExcessiveOxide()
        {
            var elements = new ElementList();
            var compounds = new CompoundList(elements);
            var service = new SpaceEngineers2D.Chemistry.ReactionService(elements, compounds);

            var originalMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.Fe3O4, new AmountOfSubstance(3)},
                {compounds.GetForElement(elements.Carbon), new AmountOfSubstance(4)},
            });

            var reactedMixture = service.Check(originalMixture, Temperature, TimeSpan);

            var expectedReactedMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.Fe3O4, new AmountOfSubstance(1)},
                {compounds.GetForElement(elements.Iron), new AmountOfSubstance(6)},
                {compounds.CO2, new AmountOfSubstance(4)},
            });

            Assert.AreEqual(expectedReactedMixture, reactedMixture);
        }

        [TestMethod]
        public void TestFe3O4ReductionWithExcessiveReducer()
        {
            var elements = new ElementList();
            var compounds = new CompoundList(elements);
            var service = new SpaceEngineers2D.Chemistry.ReactionService(elements, compounds);

            var originalMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.Fe3O4, new AmountOfSubstance(2)},
                {compounds.GetForElement(elements.Carbon), new AmountOfSubstance(5)},
            });

            var reactedMixture = service.Check(originalMixture, Temperature, TimeSpan);

            var expectedReactedMixture = Mixture.FromAbsoluteAmounts(new Dictionary<Compound, AmountOfSubstance>
            {
                {compounds.GetForElement(elements.Carbon), new AmountOfSubstance(1)},
                {compounds.GetForElement(elements.Iron), new AmountOfSubstance(6)},
                {compounds.CO2, new AmountOfSubstance(4)},
            });

            Assert.AreEqual(expectedReactedMixture, reactedMixture);
        }*/
    }
}
