using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct Time
	{
		private const double Accuracy = 0.00001;
		
		public static readonly Time Zero = new Time(0);
		
		public static Time FromSeconds(double value)
		{
			return new Time(value);
		}
		
		public static Time FromMinutes(double value)
		{
			return new Time(value* 60);
		}
		
		public static Time FromHours(double value)
		{
			return new Time(value* 3600);
		}
		
		public static Time Sum(IEnumerable<Time> items)
		{
			return new Time(items.Sum(item => item.Value));
		}
		
		public static Time Average(IEnumerable<Time> items)
		{
			return new Time(items.Average(item => item.Value));
		}
		
		public static Time Min(IEnumerable<Time> items)
		{
			return new Time(items.Min(item => item.Value));
		}
		
		public static Time Max(IEnumerable<Time> items)
		{
			return new Time(items.Max(item => item.Value));
		}
		
		public static bool operator ==(Time a, Time b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(Time a, Time b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(Time a, Time b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(Time a, Time b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(Time a, Time b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(Time a, Time b)
		{
			return a.Value >= b.Value;
		}
		
		public static Time operator +(Time a, Time b)
		{
			return new Time(a.Value + b.Value);
		}
		
		public static Time operator -(Time a, Time b)
		{
			return new Time(a.Value - b.Value);
		}
		
		public static double operator /(Time a, Time b)
		{
			return a.Value / b.Value;
		}
		
		public static Time operator *(Time a, double b)
		{
			return new Time(a.Value * b);
		}
		
		public static Time operator *(double a, Time b)
		{
			return new Time(a * b.Value);
		}
		
		public static Time operator /(Time a, double b)
		{
			return new Time(a.Value / b);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value == 0;
		
		public double InSeconds => Value;
		
		public double InMinutes => Value/ 60;
		
		public double InHours => Value/ 3600;
		
		public Time(double value)
		{
			Value = value;
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is Time time && Equals(time);
		}
		
		public bool Equals(Time other)
		{
			return Math.Abs(Value - other.Value) < Accuracy;
		}
		
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		
		public override string ToString()
		{
			return Value.ToString("0.00", CultureInfo.InvariantCulture) + "s";
		}
		
	}
}
