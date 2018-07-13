using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct Velocity
	{
		private const double Accuracy = 0.00001;
		
		public static readonly Velocity Zero = new Velocity(0);
		
		public static Velocity FromMetersPerSecond(double value)
		{
			return new Velocity(value);
		}
		
		public static Velocity FromKiloMetersPerHour(double value)
		{
			return new Velocity(value/ 3.6);
		}
		
		public static Velocity Sum(IEnumerable<Velocity> items)
		{
			return new Velocity(items.Sum(item => item.Value));
		}
		
		public static Velocity Average(IEnumerable<Velocity> items)
		{
			return new Velocity(items.Average(item => item.Value));
		}
		
		public static Velocity Min(IEnumerable<Velocity> items)
		{
			return new Velocity(items.Min(item => item.Value));
		}
		
		public static Velocity Max(IEnumerable<Velocity> items)
		{
			return new Velocity(items.Max(item => item.Value));
		}
		
		public static bool operator ==(Velocity a, Velocity b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(Velocity a, Velocity b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(Velocity a, Velocity b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(Velocity a, Velocity b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(Velocity a, Velocity b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(Velocity a, Velocity b)
		{
			return a.Value >= b.Value;
		}
		
		public static Velocity operator +(Velocity a, Velocity b)
		{
			return new Velocity(a.Value + b.Value);
		}
		
		public static Velocity operator -(Velocity a, Velocity b)
		{
			return new Velocity(a.Value - b.Value);
		}
		
		public static double operator /(Velocity a, Velocity b)
		{
			return a.Value / b.Value;
		}
		
		public static Velocity operator *(Velocity a, double b)
		{
			return new Velocity(a.Value * b);
		}
		
		public static Velocity operator *(double a, Velocity b)
		{
			return new Velocity(a * b.Value);
		}
		
		public static Velocity operator /(Velocity a, double b)
		{
			return new Velocity(a.Value / b);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value == 0;
		
		public double InMetersPerSecond => Value;
		
		public double InKiloMetersPerHour => Value/ 3.6;
		
		public Velocity(double value)
		{
			Value = value;
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is Velocity velocity && Equals(velocity);
		}
		
		public bool Equals(Velocity other)
		{
			return Math.Abs(Value - other.Value) < Accuracy;
		}
		
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		
		public override string ToString()
		{
			return Value.ToString("0.00", CultureInfo.InvariantCulture) + "m/s";
		}
		
	}
}
