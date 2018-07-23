using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct Energy
	{
		private const double Accuracy = 0.00001;
		
		public static IReadOnlyList<Unit> Units = new List<Unit>
		{
			new Unit(1, "J "),
			new Unit(1000, "kJ "),
			new Unit(10000000, "MJ "),
		};
		
		public static readonly Energy Zero = new Energy(0);
		
		public static Energy FromJoule(double value)
		{
			return new Energy(value * 1);
		}
		
		public static Energy FromKiloJoule(double value)
		{
			return new Energy(value * 1000);
		}
		
		public static Energy FromMegaJoule(double value)
		{
			return new Energy(value * 10000000);
		}
		
		public static Energy Sum(IEnumerable<Energy> items)
		{
			return new Energy(items.Sum(item => item.Value));
		}
		
		public static Energy Average(IEnumerable<Energy> items)
		{
			return new Energy(items.Average(item => item.Value));
		}
		
		public static Energy Min(IEnumerable<Energy> items)
		{
			return new Energy(items.Min(item => item.Value));
		}
		
		public static Energy Min(params Energy[] items)
		{
			return new Energy(items.Min(item => item.Value));
		}
		
		public static Energy Max(IEnumerable<Energy> items)
		{
			return new Energy(items.Max(item => item.Value));
		}
		
		public static Energy Max(params Energy[] items)
		{
			return new Energy(items.Max(item => item.Value));
		}
		
		public static bool operator ==(Energy a, Energy b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(Energy a, Energy b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(Energy a, Energy b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(Energy a, Energy b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(Energy a, Energy b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(Energy a, Energy b)
		{
			return a.Value >= b.Value;
		}
		
		public static Energy operator +(Energy a, Energy b)
		{
			return new Energy(a.Value + b.Value);
		}
		
		public static Energy operator -(Energy a)
		{
			return new Energy(-a.Value);
		}
		
		public static Energy operator -(Energy a, Energy b)
		{
			return new Energy(a.Value - b.Value);
		}
		
		public static double operator /(Energy a, Energy b)
		{
			return a.Value / b.Value;
		}
		
		public static Energy operator *(Energy a, double b)
		{
			return new Energy(a.Value * b);
		}
		
		public static Energy operator *(double a, Energy b)
		{
			return new Energy(a * b.Value);
		}
		
		public static Energy operator /(Energy a, double b)
		{
			return new Energy(a.Value / b);
		}
		
		public static Temperature operator /(Energy energy, HeatCapacity heatCapacity)
		{
			return Temperature.FromKelvin(energy.InJoule / heatCapacity.InJoulePerKelvin);
		}
		
		public static AmountOfSubstance operator /(Energy energy, EnthalpyOfFormation enthalpyOfFormation)
		{
			return AmountOfSubstance.FromMol(energy.InJoule / enthalpyOfFormation.InJoulePerMol);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value.Equals(0);
		
		public double InJoule => Value / 1;
		
		public double InKiloJoule => Value / 1000;
		
		public double InMegaJoule => Value / 10000000;
		
		public Energy(double value)
		{
			Value = value;
		}
		
		public Energy Abs()
		{
			return new Energy(Math.Abs(Value));
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is Energy energy && Equals(energy);
		}
		
		public bool Equals(Energy other)
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
