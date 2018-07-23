using System;
using System.Collections.Generic;
using System.Globalization;

namespace SpaceEngineers2D.Chemistry.Quantities
{
    public static class UnitUtility
    {
        public static string Format(IReadOnlyList<IUnit> units, double value)
        {
            var unit = GetBestMatching(units, value);
            return (value / unit.Factor).ToString("0.00", CultureInfo.InvariantCulture) + unit.Symbol;
        }

        public static IUnit GetBestMatching(IReadOnlyList<IUnit> units, double value)
        {
            if (units == null || units.Count == 0)
            {
                throw new ArgumentNullException(nameof(units), "Unit list can not be null or empty.");
            }

            value = Math.Abs(value);

            IUnit bestMatchingUnit = null;
            double bestMatchingDelta = 0;

            foreach (var unit in units)
            {
                var delta = Math.Abs(value / unit.Factor - 1);

                if (bestMatchingUnit == null || delta < bestMatchingDelta)
                {
                    bestMatchingUnit = unit;
                    bestMatchingDelta = delta;
                }
            }

            return bestMatchingUnit;
        }
    }
}
