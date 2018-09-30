namespace SpaceEngineers2DTest.Geometry.BinaryGrid
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using SpaceEngineers2D.Geometry;
    using SpaceEngineers2D.Geometry.BinaryGrid;

    [TestClass]
    public class BinaryGridTest
    {
        [TestMethod]
        public void Test1()
        {
            var grid = new BinaryGrid<string>();

            grid.Set(new IntVector(0, 0, 0), "a");

            Assert.AreEqual("a", grid.Get(new IntVector(0, 0, 0)));
        }

        [TestMethod]
        public void Test2()
        {
            var grid = new BinaryGrid<string>();

            grid.Set(new IntVector(10, 0, 0), "a");

            Assert.AreEqual("a", grid.Get(new IntVector(10, 0, 0)));
        }

        [TestMethod]
        public void Test3()
        {
            var grid = new BinaryGrid<string>();

            grid.Set(new IntVector(0, 10, 0), "a");

            Assert.AreEqual("a", grid.Get(new IntVector(0, 10, 0)));
        }

        [TestMethod]
        public void Test4()
        {
            var grid = new BinaryGrid<string>();

            grid.Set(new IntVector(10, 10, 0), "a");

            Assert.AreEqual("a", grid.Get(new IntVector(10, 10, 0)));
        }

        [TestMethod]
        public void Test5()
        {
            var grid = new BinaryGrid<string>();

            var result = grid.Set(new IntVector(-10, 0, 0), "a");

            Assert.AreEqual(new IntVector(-12, 0, 0), result.OffsetShift);
            Assert.AreEqual("a", grid.Get(new IntVector(-10, 0, 0)));
        }

        [TestMethod]
        public void Test6()
        {
            var grid = new BinaryGrid<string>();

            var result = grid.Set(new IntVector(0, -10, 0), "a");

            Assert.AreEqual(new IntVector(0, -12, 0), result.OffsetShift);
            Assert.AreEqual("a", grid.Get(new IntVector(0, -10, 0)));
        }

        [TestMethod]
        public void Test7()
        {
            var grid = new BinaryGrid<string>();

            var result = grid.Set(new IntVector(-10, -10, 0), "a");

            Assert.AreEqual(new IntVector(-12, -12, 0), result.OffsetShift);
            Assert.AreEqual("a", grid.Get(new IntVector(-10, -10, 0)));
        }

        [TestMethod]
        public void Test8()
        {
            var grid = new BinaryGrid<string>();

            var itemsToEnumerate = new Dictionary<IntVector, string>
            {
                { new IntVector(1, 0, 0), "a" },
                { new IntVector(2, 0, 0), "b" },
                { new IntVector(2, 1, 0), "c" },
                { new IntVector(3, 1, 0), "d" }
            };

            var itemsToIgnore = new Dictionary<IntVector, string>
            {
                { new IntVector(0, 1, 0), "a" },
                { new IntVector(1, 2, 0), "b" },
                { new IntVector(2, 2, 0), "c" },
                { new IntVector(3, 2, 0), "d" }
            };

            foreach (var pair in itemsToEnumerate.Concat(itemsToIgnore))
            {
                grid.Set(pair.Key, pair.Value);
            }

            var bounds = IntRectangle.FromXYZWidthHeightDepth(1, 0, 0, 3, 2, 1);

            var enumeratedItems = new Dictionary<IntVector, string>();

            grid.ForEachWithin(bounds, (item, position) => enumeratedItems.Add(position, item));

            foreach (var pair in itemsToEnumerate)
            {
                Assert.IsTrue(enumeratedItems.ContainsKey(pair.Key), "Item at position " + pair.Key + " was not enumerated.");
                Assert.AreEqual(pair.Value, enumeratedItems[pair.Key]);
            }

            Assert.AreEqual(itemsToEnumerate.Count, enumeratedItems.Count, "Count of enumerated items does not match.");
        }
    }
}
