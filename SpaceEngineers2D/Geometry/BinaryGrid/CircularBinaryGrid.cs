﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<T> GetAll()
        {
            return Grid.GetAll();
        }

        public IEnumerable<T> GetAllWithin(IntRectangle rectangle)
        {
            var normalizedRectangles = GetNormalizedRectangles(rectangle);
            return normalizedRectangles.SelectMany(normalizedRectangle => Grid.GetAllWithin(normalizedRectangle));
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
            var front = rectangle.Front;

            if (rectangle.Width >= gridWidth)
                return new [] { new IntRectangle(
                    CoordinateSystem.MinX, top, front,
                    gridWidth, rectangle.Height, rectangle.Depth) };

            if (rectangle.Height >= gridHeight)
                return new[] { new IntRectangle(
                    left, CoordinateSystem.MinY, front,
                    rectangle.Width, gridHeight, rectangle.Depth) };

            var normalizedRectangles = new List<IntRectangle>(2);
            
            if (left > right)
            {
                if (top > bottom)
                {
                    throw new NotImplementedException();
                }
                else
                {
                    normalizedRectangles.Add(new IntRectangle(
                        CoordinateSystem.MinX, top, front,
                        right - CoordinateSystem.MinX, rectangle.Height, rectangle.Depth));
                    normalizedRectangles.Add(new IntRectangle(
                        left, top, front,
                        CoordinateSystem.MaxX - left, rectangle.Height, rectangle.Depth));
                }
            }
            else
            {
                if (top > bottom)
                {
                    normalizedRectangles.Add(new IntRectangle(
                        left, CoordinateSystem.MinY, front,
                        rectangle.Width, bottom - CoordinateSystem.MinY, rectangle.Depth));
                    normalizedRectangles.Add(new IntRectangle(
                        left, top, front,
                        rectangle.Width, CoordinateSystem.MaxY - top, rectangle.Depth));
                }
                else
                {
                    normalizedRectangles.Add(new IntRectangle(
                        left, rectangle.Top, front,
                        rectangle.Width, rectangle.Height, rectangle.Depth));
                }
            }

            return normalizedRectangles;
        }
    }
}
