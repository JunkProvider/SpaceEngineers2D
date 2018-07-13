using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct Mass
	{
		private const double Accuracy = 0.00001;
		
		public static readonly Mass Zero = new Mass(0);
		
		public static Mass FromGram(double value)
		{
			return new Mass(value);
		}
		
		public static Mass FromKiloGram(double value)
		{
			return new Mass(value* 1000);
		}
		
		public static Mass Sum(IEnumerable<Mass> items)
		{
			return new Mass(items.Sum(item => item.Value));
		}
		
		public static Mass Average(IEnumerable<Mass> items)
		{
			return new Mass(items.Average(item => item.Value));
		}
		
		public static Mass Min(IEnumerable<Mass> items)
		{
			return new Mass(items.Min(item => item.Value));
		}
		
		public static Mass Max(IEnumerable<Mass> items)
		{
			return new Mass(items.Max(item => item.Value));
		}
		
		public static bool operator ==(Mass a, Mass b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(Mass a, Mass b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(Mass a, Mass b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(Mass a, Mass b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(Mass a, Mass b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(Mass a, Mass b)
		{
			return a.Value >= b.Value;
		}
		
		public static Mass operator +(Mass a, Mass b)
		{
			return new Mass(a.Value + b.Value);
		}
		
		public static Mass operator -(Mass a, Mass b)
		{
			return new Mass(a.Value - b.Value);
		}
		
		public static double operator /(Mass a, Mass b)
		{
			return a.Value / b.Value;
		}
		
		public static Mass operator *(Mass a, double b)
		{
			return new Mass(a.Value * b);
		}
		
		public static Mass operator *(double a, Mass b)
		{
			return new Mass(a * b.Value);
		}
		
		public static Mass operator /(Mass a, double b)
		{
			return new Mass(a.Value / b);
		}
		
		public static Volume operator /(Mass mass, Density density)
		{
			return Volume.FromCubicCentimeters(mass.InGram / density.InGramPerCubicCentimeter);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value == 0;
		
		public double InGram => Value;
		
		public double InKiloGram => Value/ 1000;
		
		public Mass(double value)
		{
			Value = value;
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is Mass mass && Equals(mass);
		}
		
		public bool Equals(Mass other)
		{
			return Math.Abs(Value - other.Value) < Accuracy;
		}
		
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		
		public override string ToString()
		{
			return Value.ToString("0.00", CultureInfo.InvariantCulture) + "g";
		}
		
	}
}