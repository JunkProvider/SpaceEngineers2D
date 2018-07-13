using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct Density
	{
		private const double Accuracy = 0.00001;
		
		public static readonly Density Zero = new Density(0);
		
		public static Density FromGramPerCubicCentimeter(double value)
		{
			return new Density(value);
		}
		
		public static Density FromGramPerLiter(double value)
		{
			return new Density(value/ 1000);
		}
		
		public static Density Sum(IEnumerable<Density> items)
		{
			return new Density(items.Sum(item => item.Value));
		}
		
		public static Density Average(IEnumerable<Density> items)
		{
			return new Density(items.Average(item => item.Value));
		}
		
		public static Density Min(IEnumerable<Density> items)
		{
			return new Density(items.Min(item => item.Value));
		}
		
		public static Density Max(IEnumerable<Density> items)
		{
			return new Density(items.Max(item => item.Value));
		}
		
		public static bool operator ==(Density a, Density b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(Density a, Density b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(Density a, Density b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(Density a, Density b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(Density a, Density b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(Density a, Density b)
		{
			return a.Value >= b.Value;
		}
		
		public static Density operator +(Density a, Density b)
		{
			return new Density(a.Value + b.Value);
		}
		
		public static Density operator -(Density a, Density b)
		{
			return new Density(a.Value - b.Value);
		}
		
		public static double operator /(Density a, Density b)
		{
			return a.Value / b.Value;
		}
		
		public static Density operator *(Density a, double b)
		{
			return new Density(a.Value * b);
		}
		
		public static Density operator *(double a, Density b)
		{
			return new Density(a * b.Value);
		}
		
		public static Density operator /(Density a, double b)
		{
			return new Density(a.Value / b);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value == 0;
		
		public double InGramPerCubicCentimeter => Value;
		
		public double InGramPerLiter => Value/ 1000;
		
		public Density(double value)
		{
			Value = value;
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is Density density && Equals(density);
		}
		
		public bool Equals(Density other)
		{
			return Math.Abs(Value - other.Value) < Accuracy;
		}
		
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		
		public override string ToString()
		{
			return Value.ToString("0.00", CultureInfo.InvariantCulture) + "g/cm³";
		}
		
	}
}
