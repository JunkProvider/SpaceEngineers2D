using System;
using SpaceEngineers2D.Geometry;

namespace SpaceEngineers2D.Model
{
    public interface ICoordinateSystem
    {
        int MinX { get; }

        int MaxX { get; }

        int MinY { get; }

        int MaxY { get; }

        int MinZ { get; }

        int MaxZ { get; }
    }

    public sealed class CoordinateSystem : ICoordinateSystem
    {
        public static CoordinateSystem CreateHorizontalCircular(int minX, int maxX)
        {
            return new CoordinateSystem(minX, maxX, int.MinValue / 2 + 1, int.MaxValue / 2 - 1, int.MinValue / 2 + 1, int.MaxValue / 2 - 1);
        }

        public int MinX { get; }

        public int MaxX { get; }

        public int MinY { get; }

        public int MaxY { get; }

        public int MinZ { get; }

        public int MaxZ { get; }

        public CoordinateSystem(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
        {
            if (minX < int.MinValue / 2 + 1)
                throw new ArgumentOutOfRangeException();

            if (maxX > int.MaxValue / 2 - 1)
                throw new ArgumentOutOfRangeException();

            if (minY < int.MinValue / 2 + 1)
                throw new ArgumentOutOfRangeException();

            if (maxY > int.MaxValue / 2 - 1)
                throw new ArgumentOutOfRangeException();

            if (minZ < int.MinValue / 2 + 1)
                throw new ArgumentOutOfRangeException();

            if (maxZ > int.MaxValue / 2 - 1)
                throw new ArgumentOutOfRangeException();

            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
            MinZ = minZ;
            MaxZ = maxZ;
        }
    }

    public static class CoordinateSystemExtensions
    {
        public static IntVector Normalize(this ICoordinateSystem coordinateSystem, IntVector position)
        {
            return new IntVector(coordinateSystem.NormalizeX(position.X), coordinateSystem.NormalizeY(position.Y), position.Z);
        }

        public static int NormalizeX(this ICoordinateSystem coordinateSystem, int x)
        {
            return Normalize(coordinateSystem.MinX, coordinateSystem.MaxX, x);
        }

        public static int NormalizeY(this ICoordinateSystem coordinateSystem, int y)
        {
            return Normalize(coordinateSystem.MinY, coordinateSystem.MaxY, y);
        }

        public static IntRectangle Denormalize(this ICoordinateSystem coordinateSystem, IntRectangle bounds, IntRectangle referenceBounds)
        {
            return coordinateSystem.Denormalize(bounds.Position, bounds.Size, referenceBounds.Position, referenceBounds.Size);
        }

        public static IntRectangle Denormalize(this ICoordinateSystem coordinateSystem, IntVector position, IntVector size, IntVector referencePosition, IntVector referenceSize)
        {
            // TODO: consider size
            return new IntRectangle(
                coordinateSystem.DenormalizeX(position.X, referencePosition.X),
                coordinateSystem.DenormalizeY(position.Y, referencePosition.Y),
                position.Z,
                size.X,
                size.Y,
                size.Z);
        }

        public static IntVector Denormalize(this ICoordinateSystem coordinateSystem, IntVector position, IntVector referencePosition)
        {
            return new IntVector(coordinateSystem.DenormalizeX(position.X, referencePosition.X), coordinateSystem.DenormalizeY(position.Y, referencePosition.Y), position.Z);
        }

        public static int DenormalizeX(this ICoordinateSystem coordinateSystem, int x, int relativeTo)
        {
            return Denormalize(coordinateSystem.MinX, coordinateSystem.MaxX, x, relativeTo);
        }

        public static int DenormalizeY(this ICoordinateSystem coordinateSystem, int y, int relativeTo)
        {
            return Denormalize(coordinateSystem.MinY, coordinateSystem.MaxY, y, relativeTo);
        }

        private static int Normalize(int min, int max, int value)
        {
            var range = max - min;

            while (value < min)
            {
                value += range;
            }

            while (value >= max)
            {
                value -= range;
            }

            return value;
        }

        private static int Denormalize(int min, int max, int value, int leftReference)
        {
            var range = max - min;
            var offset = value - leftReference;
            var distance = Math.Abs(offset);

            if (offset < 0)
            {
                while (true)
                {
                    var newValue = value + range;
                    var newDistance = Math.Abs(newValue - leftReference);

                    if (newDistance >= distance)
                    {
                        break;
                    }

                    value = newValue;
                    distance = newDistance;
                }
            }
            else
            {
                while (true)
                {
                    var newValue = value - range;
                    var newDistance = Math.Abs(newValue - leftReference);

                    if (newDistance >= distance)
                    {
                        break;
                    }

                    value = newValue;
                    distance = newDistance;
                }
            }

            return value;
        }
    }
}