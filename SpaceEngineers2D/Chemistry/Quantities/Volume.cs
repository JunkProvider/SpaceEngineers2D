using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct Volume
	{
		private const double Accuracy = 0.00001;
		
		public static IReadOnlyList<Unit> Units = new List<Unit>
		{
			new Unit(1, "cm³"),
			new Unit(1000, "L"),
		};
		
		public static readonly Volume Zero = new Volume(0);
		
		public static Volume FromCubicCentimeters(double value)
		{
			return new Volume(value * 1);
		}
		
		public static Volume FromLiters(double value)
		{
			return new Volume(value * 1000);
		}
		
		public static Volume Sum(IEnumerable<Volume> items)
		{
			return new Volume(items.Sum(item => item.Value));
		}
		
		public static Volume Average(IEnumerable<Volume> items)
		{
			return new Volume(items.Average(item => item.Value));
		}
		
		public static Volume Min(IEnumerable<Volume> items)
		{
			return new Volume(items.Min(item => item.Value));
		}
		
		public static Volume Min(params Volume[] items)
		{
			return new Volume(items.Min(item => item.Value));
		}
		
		public static Volume Max(IEnumerable<Volume> items)
		{
			return new Volume(items.Max(item => item.Value));
		}
		
		public static Volume Max(params Volume[] items)
		{
			return new Volume(items.Max(item => item.Value));
		}
		
		public static bool operator ==(Volume a, Volume b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(Volume a, Volume b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(Volume a, Volume b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(Volume a, Volume b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(Volume a, Volume b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(Volume a, Volume b)
		{
			return a.Value >= b.Value;
		}
		
		public static Volume operator +(Volume a, Volume b)
		{
			return new Volume(a.Value + b.Value);
		}
		
		public static Volume operator -(Volume a)
		{
			return new Volume(-a.Value);
		}
		
		public static Volume operator -(Volume a, Volume b)
		{
			return new Volume(a.Value - b.Value);
		}
		
		public static double operator /(Volume a, Volume b)
		{
			return a.Value / b.Value;
		}
		
		public static Volume operator *(Volume a, double b)
		{
			return new Volume(a.Value * b);
		}
		
		public static Volume operator *(double a, Volume b)
		{
			return new Volume(a * b.Value);
		}
		
		public static Volume operator /(Volume a, double b)
		{
			return new Volume(a.Value / b);
		}
		
		public static AmountOfSubstance operator /(Volume volume, MolecularVolume molecularVolume)
		{
			return AmountOfSubstance.FromMol(volume.InCubicCentimeters/ molecularVolume.InCubicCentimetersPerMol);
		}
		
		public static Mass operator *(Volume volume, Density density)
		{
			return Mass.FromGram(volume.InCubicCentimeters / density.InGramPerCubicCentimeter);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value.Equals(0);
		
		public double InCubicCentimeters => Value / 1;
		
		public double InLiters => Value / 1000;
		
		public Volume(double value)
		{
			Value = value;
		}
		
		public Volume Abs()
		{
			return new Volume(Math.Abs(Value));
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is Volume volume && Equals(volume);
		}
		
		public bool Equals(Volume other)
		{
			return Math.Abs(Value - other.Value) < Accuracy;
		}
		
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		
		public override string ToString()
		{
			return UnitUtility.Format(Units, Value);
		}
		
		public class Unit : IUnit
		{
			public double Factor { get; }
			
			public string Symbol { get; }
			
			public Unit(double factor, string symbol)
			{
				Factor = factor;
				Symbol = symbol;
			}
		}
	}
}
