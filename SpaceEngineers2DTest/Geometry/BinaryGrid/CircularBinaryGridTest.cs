using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceEngineers2D.Geometry;
using SpaceEngineers2D.Geometry.BinaryGrid;
using SpaceEngineers2D.Model;

namespace SpaceEngineers2DTest.Geometry.BinaryGrid
{
    [TestClass]
    public class CircularBinaryGridTest
    {
        [TestMethod]
        public void Test1()
        {
            var coordinateSystem = CoordinateSystem.CreateHorizontalCircular(0, 10);
            var grid = new CircularBinaryGrid<string>(coordinateSystem);
            var rect = new IntRectangle(0, 0, 2, 2);
            var calls = new Dictionary<IntVector, string>();
            EnumerateItemDelegate<string> func = (item, position) => calls.Add(position, item);
            
            AddItems(grid, new IntRectangle(-1, -1, 4, 4));

            grid.ForEachWithin(rect, func);

            Assert.AreEqual(4, calls.Count);
            AssertItem(calls, new IntVector(0, 0));
            AssertItem(calls, new IntVector(1, 0));
            AssertItem(calls, new IntVector(0, 1));
            AssertItem(calls, new IntVector(1, 1));
        }

        [TestMethod]
        public void Test2()
        {
            var coordinateSystem = CoordinateSystem.CreateHorizontalCircular(0, 10);
            var grid = new CircularBinaryGrid<string>(coordinateSystem);
            var rect = new IntRectangle(-1, 0, 2, 2);
            var calls = new Dictionary<IntVector, string>();
            EnumerateItemDelegate<string> func = (item, position) =>
            {
                calls.Add(position, item);
            };

            AddItems(grid, new IntRectangle(-2, -1, 4, 4));

            grid.ForEachWithin(rect, func);

            Assert.AreEqual(4, calls.Count);
            AssertItem(calls, new IntVector(0, 0));
            AssertItem(calls, new IntVector(0, 1));
            AssertItem(calls, new IntVector(9, 0));
            AssertItem(calls, new IntVector(9, 1));
        }

        [TestMethod]
        public void Test3()
        {
            var coordinateSystem = CoordinateSystem.CreateHorizontalCircular(0, 4);
            var grid = new CircularBinaryGrid<string>(coordinateSystem);
            var rect = new IntRectangle(2, 0, 5, 1);
            var calls = new Dictionary<IntVector, string>();
            EnumerateItemDelegate<string> func = (item, position) =>
            {
                calls.Add(position, item);
            };

            AddItems(grid, new IntRectangle(0, 0, 4, 4));

            grid.ForEachWithin(rect, func);

            Assert.AreEqual(4, calls.Count);
            AssertItem(calls, new IntVector(0, 0));
            AssertItem(calls, new IntVector(1, 0));
            AssertItem(calls, new IntVector(2, 0));
            AssertItem(calls, new IntVector(3, 0));
        }

        private void AddItems(CircularBinaryGrid<string> grid, IntRectangle rect)
        {
            for (var x = rect.Left; x < rect.Right; x++)
            {
                for (var y = rect.Top; y < rect.Bottom; y++)
                {
                    AddItem(grid, new IntVector(x, y));
                }
            }
        }

        private void AddItem(CircularBinaryGrid<string> grid, IntVector position)
        {
            grid.Set(position, grid.CoordinateSystem.Normalize(position).ToString());
        }

        private void AssertItem(Dictionary<IntVector, string> items, IntVector position)
        {
            Assert.AreEqual(position.ToString(), items[position]);
        }
    }
}
