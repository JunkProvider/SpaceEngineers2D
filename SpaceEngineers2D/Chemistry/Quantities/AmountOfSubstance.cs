using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct AmountOfSubstance
	{
		private const double Accuracy = 0.00001;
		
		public static readonly AmountOfSubstance Zero = new AmountOfSubstance(0);
		
		public static AmountOfSubstance FromMol(double value)
		{
			return new AmountOfSubstance(value);
		}
		
		public static AmountOfSubstance Sum(IEnumerable<AmountOfSubstance> items)
		{
			return new AmountOfSubstance(items.Sum(item => item.Value));
		}
		
		public static AmountOfSubstance Average(IEnumerable<AmountOfSubstance> items)
		{
			return new AmountOfSubstance(items.Average(item => item.Value));
		}
		
		public static AmountOfSubstance Min(IEnumerable<AmountOfSubstance> items)
		{
			return new AmountOfSubstance(items.Min(item => item.Value));
		}
		
		public static AmountOfSubstance Max(IEnumerable<AmountOfSubstance> items)
		{
			return new AmountOfSubstance(items.Max(item => item.Value));
		}
		
		public static bool operator ==(AmountOfSubstance a, AmountOfSubstance b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(AmountOfSubstance a, AmountOfSubstance b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(AmountOfSubstance a, AmountOfSubstance b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(AmountOfSubstance a, AmountOfSubstance b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(AmountOfSubstance a, AmountOfSubstance b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(AmountOfSubstance a, AmountOfSubstance b)
		{
			return a.Value >= b.Value;
		}
		
		public static AmountOfSubstance operator +(AmountOfSubstance a, AmountOfSubstance b)
		{
			return new AmountOfSubstance(a.Value + b.Value);
		}
		
		public static AmountOfSubstance operator -(AmountOfSubstance a, AmountOfSubstance b)
		{
			return new AmountOfSubstance(a.Value - b.Value);
		}
		
		public static double operator /(AmountOfSubstance a, AmountOfSubstance b)
		{
			return a.Value / b.Value;
		}
		
		public static AmountOfSubstance operator *(AmountOfSubstance a, double b)
		{
			return new AmountOfSubstance(a.Value * b);
		}
		
		public static AmountOfSubstance operator *(double a, AmountOfSubstance b)
		{
			return new AmountOfSubstance(a * b.Value);
		}
		
		public static AmountOfSubstance operator /(AmountOfSubstance a, double b)
		{
			return new AmountOfSubstance(a.Value / b);
		}
		
		public static Mass operator *(AmountOfSubstance amountOfSubstance, MolecularMass molecularMass)
		{
			return Mass.FromGram(amountOfSubstance.InMol * molecularMass.InGramPerMol);
		}
		
		public static Volume operator *(AmountOfSubstance amountOfSubstance, MolecularVolume molecularVolume)
		{
			return Volume.FromCubicCentimeters(amountOfSubstance.InMol * molecularVolume.InCubicCentimetersPerMol);
		}
		
		public static HeatCapacity operator *(AmountOfSubstance amountOfSubstance, MolecularHeatCapacity molecularHeatCapacity)
		{
			return HeatCapacity.FromJoulePerKelvin(amountOfSubstance.InMol * molecularHeatCapacity.InJoulePerMolTimesKelvin);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value == 0;
		
		public double InMol => Value;
		
		public AmountOfSubstance(double value)
		{
			Value = value;
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is AmountOfSubstance amountOfSubstance && Equals(amountOfSubstance);
		}
		
		public bool Equals(AmountOfSubstance other)
		{
			return Math.Abs(Value - other.Value) < Accuracy;
		}
		
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		
		public override string ToString()
		{
			return Value.ToString("0.00", CultureInfo.InvariantCulture) + "mol";
		}
		
	}
}
