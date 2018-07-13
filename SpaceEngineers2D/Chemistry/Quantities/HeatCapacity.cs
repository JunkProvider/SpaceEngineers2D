using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct HeatCapacity
	{
		private const double Accuracy = 0.00001;
		
		public static readonly HeatCapacity Zero = new HeatCapacity(0);
		
		public static HeatCapacity FromJoulePerKelvin(double value)
		{
			return new HeatCapacity(value);
		}
		
		public static HeatCapacity Sum(IEnumerable<HeatCapacity> items)
		{
			return new HeatCapacity(items.Sum(item => item.Value));
		}
		
		public static HeatCapacity Average(IEnumerable<HeatCapacity> items)
		{
			return new HeatCapacity(items.Average(item => item.Value));
		}
		
		public static HeatCapacity Min(IEnumerable<HeatCapacity> items)
		{
			return new HeatCapacity(items.Min(item => item.Value));
		}
		
		public static HeatCapacity Max(IEnumerable<HeatCapacity> items)
		{
			return new HeatCapacity(items.Max(item => item.Value));
		}
		
		public static bool operator ==(HeatCapacity a, HeatCapacity b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(HeatCapacity a, HeatCapacity b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(HeatCapacity a, HeatCapacity b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(HeatCapacity a, HeatCapacity b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(HeatCapacity a, HeatCapacity b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(HeatCapacity a, HeatCapacity b)
		{
			return a.Value >= b.Value;
		}
		
		public static HeatCapacity operator +(HeatCapacity a, HeatCapacity b)
		{
			return new HeatCapacity(a.Value + b.Value);
		}
		
		public static HeatCapacity operator -(HeatCapacity a, HeatCapacity b)
		{
			return new HeatCapacity(a.Value - b.Value);
		}
		
		public static double operator /(HeatCapacity a, HeatCapacity b)
		{
			return a.Value / b.Value;
		}
		
		public static HeatCapacity operator *(HeatCapacity a, double b)
		{
			return new HeatCapacity(a.Value * b);
		}
		
		public static HeatCapacity operator *(double a, HeatCapacity b)
		{
			return new HeatCapacity(a * b.Value);
		}
		
		public static HeatCapacity operator /(HeatCapacity a, double b)
		{
			return new HeatCapacity(a.Value / b);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value == 0;
		
		public double InJoulePerKelvin => Value;
		
		public HeatCapacity(double value)
		{
			Value = value;
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is HeatCapacity heatCapacity && Equals(heatCapacity);
		}
		
		public bool Equals(HeatCapacity other)
		{
			return Math.Abs(Value - other.Value) < Accuracy;
		}
		
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		
		public override string ToString()
		{
			return Value.ToString("0.00", CultureInfo.InvariantCulture) + "J/K ";
		}
		
	}
}
