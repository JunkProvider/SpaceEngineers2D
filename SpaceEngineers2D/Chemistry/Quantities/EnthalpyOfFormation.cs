using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct EnthalpyOfFormation
	{
		private const double Accuracy = 0.00001;
		
		public static IReadOnlyList<Unit> Units = new List<Unit>
		{
			new Unit(1, "J/mol "),
			new Unit(1000, "kJ/mol "),
			new Unit(1000000, "MJ/mol "),
		};
		
		public static readonly EnthalpyOfFormation Zero = new EnthalpyOfFormation(0);
		
		public static EnthalpyOfFormation FromJoulePerMol(double value)
		{
			return new EnthalpyOfFormation(value * 1);
		}
		
		public static EnthalpyOfFormation FromKiloJoulePerMol(double value)
		{
			return new EnthalpyOfFormation(value * 1000);
		}
		
		public static EnthalpyOfFormation FromMegaJoulePerMol(double value)
		{
			return new EnthalpyOfFormation(value * 1000000);
		}
		
		public static EnthalpyOfFormation Sum(IEnumerable<EnthalpyOfFormation> items)
		{
			return new EnthalpyOfFormation(items.Sum(item => item.Value));
		}
		
		public static EnthalpyOfFormation Average(IEnumerable<EnthalpyOfFormation> items)
		{
			return new EnthalpyOfFormation(items.Average(item => item.Value));
		}
		
		public static EnthalpyOfFormation Min(IEnumerable<EnthalpyOfFormation> items)
		{
			return new EnthalpyOfFormation(items.Min(item => item.Value));
		}
		
		public static EnthalpyOfFormation Min(params EnthalpyOfFormation[] items)
		{
			return new EnthalpyOfFormation(items.Min(item => item.Value));
		}
		
		public static EnthalpyOfFormation Max(IEnumerable<EnthalpyOfFormation> items)
		{
			return new EnthalpyOfFormation(items.Max(item => item.Value));
		}
		
		public static EnthalpyOfFormation Max(params EnthalpyOfFormation[] items)
		{
			return new EnthalpyOfFormation(items.Max(item => item.Value));
		}
		
		public static bool operator ==(EnthalpyOfFormation a, EnthalpyOfFormation b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(EnthalpyOfFormation a, EnthalpyOfFormation b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(EnthalpyOfFormation a, EnthalpyOfFormation b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(EnthalpyOfFormation a, EnthalpyOfFormation b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(EnthalpyOfFormation a, EnthalpyOfFormation b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(EnthalpyOfFormation a, EnthalpyOfFormation b)
		{
			return a.Value >= b.Value;
		}
		
		public static EnthalpyOfFormation operator +(EnthalpyOfFormation a, EnthalpyOfFormation b)
		{
			return new EnthalpyOfFormation(a.Value + b.Value);
		}
		
		public static EnthalpyOfFormation operator -(EnthalpyOfFormation a)
		{
			return new EnthalpyOfFormation(-a.Value);
		}
		
		public static EnthalpyOfFormation operator -(EnthalpyOfFormation a, EnthalpyOfFormation b)
		{
			return new EnthalpyOfFormation(a.Value - b.Value);
		}
		
		public static double operator /(EnthalpyOfFormation a, EnthalpyOfFormation b)
		{
			return a.Value / b.Value;
		}
		
		public static EnthalpyOfFormation operator *(EnthalpyOfFormation a, double b)
		{
			return new EnthalpyOfFormation(a.Value * b);
		}
		
		public static EnthalpyOfFormation operator *(double a, EnthalpyOfFormation b)
		{
			return new EnthalpyOfFormation(a * b.Value);
		}
		
		public static EnthalpyOfFormation operator /(EnthalpyOfFormation a, double b)
		{
			return new EnthalpyOfFormation(a.Value / b);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value.Equals(0);
		
		public double InJoulePerMol => Value / 1;
		
		public double InKiloJoulePerMol => Value / 1000;
		
		public double InMegaJoulePerMol => Value / 1000000;
		
		public EnthalpyOfFormation(double value)
		{
			Value = value;
		}
		
		public EnthalpyOfFormation Abs()
		{
			return new EnthalpyOfFormation(Math.Abs(Value));
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is EnthalpyOfFormation enthalpyOfFormation && Equals(enthalpyOfFormation);
		}
		
		public bool Equals(EnthalpyOfFormation other)
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
