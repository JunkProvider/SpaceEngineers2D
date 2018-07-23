using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct Temperature
	{
		private const double Accuracy = 0.00001;
		
		public static IReadOnlyList<Unit> Units = new List<Unit>
		{
			new Unit(1, "K "),
		};
		
		public static readonly Temperature Zero = new Temperature(0);
		
		public static Temperature FromKelvin(double value)
		{
			return new Temperature(value * 1);
		}
		
		public static Temperature Sum(IEnumerable<Temperature> items)
		{
			return new Temperature(items.Sum(item => item.Value));
		}
		
		public static Temperature Average(IEnumerable<Temperature> items)
		{
			return new Temperature(items.Average(item => item.Value));
		}
		
		public static Temperature Min(IEnumerable<Temperature> items)
		{
			return new Temperature(items.Min(item => item.Value));
		}
		
		public static Temperature Min(params Temperature[] items)
		{
			return new Temperature(items.Min(item => item.Value));
		}
		
		public static Temperature Max(IEnumerable<Temperature> items)
		{
			return new Temperature(items.Max(item => item.Value));
		}
		
		public static Temperature Max(params Temperature[] items)
		{
			return new Temperature(items.Max(item => item.Value));
		}
		
		public static bool operator ==(Temperature a, Temperature b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(Temperature a, Temperature b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(Temperature a, Temperature b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(Temperature a, Temperature b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(Temperature a, Temperature b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(Temperature a, Temperature b)
		{
			return a.Value >= b.Value;
		}
		
		public static Temperature operator +(Temperature a, Temperature b)
		{
			return new Temperature(a.Value + b.Value);
		}
		
		public static Temperature operator -(Temperature a)
		{
			return new Temperature(-a.Value);
		}
		
		public static Temperature operator -(Temperature a, Temperature b)
		{
			return new Temperature(a.Value - b.Value);
		}
		
		public static double operator /(Temperature a, Temperature b)
		{
			return a.Value / b.Value;
		}
		
		public static Temperature operator *(Temperature a, double b)
		{
			return new Temperature(a.Value * b);
		}
		
		public static Temperature operator *(double a, Temperature b)
		{
			return new Temperature(a * b.Value);
		}
		
		public static Temperature operator /(Temperature a, double b)
		{
			return new Temperature(a.Value / b);
		}
		
		public static Energy operator *(Temperature temperature, HeatCapacity heatCapacity)
		{
			return Energy.FromJoule(temperature.InKelvin * heatCapacity.InJoulePerKelvin);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value.Equals(0);
		
		public double InKelvin => Value / 1;
		
		public Temperature(double value)
		{
			Value = value;
		}
		
		public Temperature Abs()
		{
			return new Temperature(Math.Abs(Value));
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is Temperature temperature && Equals(temperature);
		}
		
		public bool Equals(Temperature other)
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
