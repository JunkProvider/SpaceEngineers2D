using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct Distance
	{
		private const double Accuracy = 0.00001;
		
		public static readonly Distance Zero = new Distance(0);
		
		public static Distance FromMeters(double value)
		{
			return new Distance(value);
		}
		
		public static Distance FromKiloMeters(double value)
		{
			return new Distance(value* 1000);
		}
		
		public static Distance Sum(IEnumerable<Distance> items)
		{
			return new Distance(items.Sum(item => item.Value));
		}
		
		public static Distance Average(IEnumerable<Distance> items)
		{
			return new Distance(items.Average(item => item.Value));
		}
		
		public static Distance Min(IEnumerable<Distance> items)
		{
			return new Distance(items.Min(item => item.Value));
		}
		
		public static Distance Max(IEnumerable<Distance> items)
		{
			return new Distance(items.Max(item => item.Value));
		}
		
		public static bool operator ==(Distance a, Distance b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(Distance a, Distance b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(Distance a, Distance b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(Distance a, Distance b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(Distance a, Distance b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(Distance a, Distance b)
		{
			return a.Value >= b.Value;
		}
		
		public static Distance operator +(Distance a, Distance b)
		{
			return new Distance(a.Value + b.Value);
		}
		
		public static Distance operator -(Distance a, Distance b)
		{
			return new Distance(a.Value - b.Value);
		}
		
		public static double operator /(Distance a, Distance b)
		{
			return a.Value / b.Value;
		}
		
		public static Distance operator *(Distance a, double b)
		{
			return new Distance(a.Value * b);
		}
		
		public static Distance operator *(double a, Distance b)
		{
			return new Distance(a * b.Value);
		}
		
		public static Distance operator /(Distance a, double b)
		{
			return new Distance(a.Value / b);
		}
		
		public static Time operator /(Distance distance, Velocity velocity)
		{
			return Time.FromSeconds(distance.InMeters / velocity.InMetersPerSecond);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value == 0;
		
		public double InMeters => Value;
		
		public double InKiloMeters => Value/ 1000;
		
		public Distance(double value)
		{
			Value = value;
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is Distance distance && Equals(distance);
		}
		
		public bool Equals(Distance other)
		{
			return Math.Abs(Value - other.Value) < Accuracy;
		}
		
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		
		public override string ToString()
		{
			return Value.ToString("0.00", CultureInfo.InvariantCulture) + "m";
		}
		
	}
}
