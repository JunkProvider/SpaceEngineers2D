using System;
using System.Collections.Generic;
using SpaceEngineers2D.Model;

namespace SpaceEngineers2D.Geometry.BinaryGrid
{
    public class CircularBinaryGrid<T>
    {
        public ICoordinateSystem CoordinateSystem { get; }

        private BinaryGrid<T> Grid { get; } = new BinaryGrid<T>();

        public CircularBinaryGrid(ICoordinateSystem coordinateSystem)
        {
            CoordinateSystem = coordinateSystem;
        }

        public T Get(IntVector position)
        {
            return Grid.Get(CoordinateSystem.Normalize(position));
        }

        public SetItemResult<T> Set(IntVector position, T item)
        {
            return Grid.Set(CoordinateSystem.Normalize(position), item);
        }

        public void ForEach(EnumerateItemDelegate<T> func)
        {
            Grid.ForEach(func);
        }

        public void ForEachWithin(IntRectangle rectangle, EnumerateItemDelegate<T> func)
        {
            var normalizedRectangles = GetNormalizedRectangles(rectangle);

            foreach (var normalizedRectangle in normalizedRectangles)
            {
                Grid.ForEachWithin(normalizedRectangle, func);
            }
        }

        private IEnumerable<IntRectangle> GetNormalizedRectangles(IntRectangle rectangle)
        {
            if (rectangle.Width == 0 || rectangle.Height == 0)
                return new List<IntRectangle>();

            var gridWidth = CoordinateSystem.MaxX - CoordinateSystem.MinX;
            var gridHeight = CoordinateSystem.MaxY - CoordinateSystem.MinY;

            var left = CoordinateSystem.NormalizeX(rectangle.Left);
            var right = CoordinateSystem.NormalizeX(rectangle.Right);
            var top = CoordinateSystem.NormalizeY(rectangle.Top);
            var bottom = CoordinateSystem.NormalizeY(rectangle.Bottom);

            if (rectangle.Width >= gridWidth)
                return new [] { new IntRectangle(CoordinateSystem.MinX, top, gridWidth, rectangle.Height) };

            if (rectangle.Height >= gridHeight)
                return new[] { new IntRectangle(left, CoordinateSystem.MinY, rectangle.Width, gridHeight) };

            var normalizedRectangles = new List<IntRectangle>(2);
            
            if (left > right)
            {
                if (top > bottom)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    normalizedRectangles.Add(new IntRectangle(CoordinateSystem.MinX, top, right - CoordinateSystem.MinX, rectangle.Height));
                    normalizedRectangles.Add(new IntRectangle(left, top, CoordinateSystem.MaxX - left, rectangle.Height));
                }
            }
            else
            {
                if (top > bottom)
                {
                    normalizedRectangles.Add(new IntRectangle(left, CoordinateSystem.MinY, rectangle.Width, bottom - CoordinateSystem.MinY));
                    normalizedRectangles.Add(new IntRectangle(left, top, rectangle.Width, CoordinateSystem.MaxY - top));
                }
                else
                {
                    normalizedRectangles.Add(new IntRectangle(left, rectangle.Top, rectangle.Width, rectangle.Height));
                }
            }

            return normalizedRectangles;
        }
    }
}
