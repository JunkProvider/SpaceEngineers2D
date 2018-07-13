using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct Energy
	{
		private const double Accuracy = 0.00001;
		
		public static readonly Energy Zero = new Energy(0);
		
		public static Energy FromJoule(double value)
		{
			return new Energy(value);
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
		
		public static Energy Max(IEnumerable<Energy> items)
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
		
		public readonly double Value;
		
		public bool IsZero => Value == 0;
		
		public double InJoule => Value;
		
		public Energy(double value)
		{
			Value = value;
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
			return Value.ToString("0.00", CultureInfo.InvariantCulture) + "J ";
		}
		
	}
}
