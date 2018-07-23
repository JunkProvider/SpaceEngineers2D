using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct HeatCapacity
	{
		private const double Accuracy = 0.00001;
		
		public static IReadOnlyList<Unit> Units = new List<Unit>
		{
			new Unit(1, "J/K "),
			new Unit(1000, "kJ/K "),
			new Unit(10000000, "MJ/K "),
		};
		
		public static readonly HeatCapacity Zero = new HeatCapacity(0);
		
		public static HeatCapacity FromJoulePerKelvin(double value)
		{
			return new HeatCapacity(value * 1);
		}
		
		public static HeatCapacity FromKiloJoulePerKelvin(double value)
		{
			return new HeatCapacity(value * 1000);
		}
		
		public static HeatCapacity FromMegaPerKelvin(double value)
		{
			return new HeatCapacity(value * 10000000);
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
		
		public static HeatCapacity Min(params HeatCapacity[] items)
		{
			return new HeatCapacity(items.Min(item => item.Value));
		}
		
		public static HeatCapacity Max(IEnumerable<HeatCapacity> items)
		{
			return new HeatCapacity(items.Max(item => item.Value));
		}
		
		public static HeatCapacity Max(params HeatCapacity[] items)
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
		
		public static HeatCapacity operator -(HeatCapacity a)
		{
			return new HeatCapacity(-a.Value);
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
		
		public bool IsZero => Value.Equals(0);
		
		public double InJoulePerKelvin => Value / 1;
		
		public double InKiloJoulePerKelvin => Value / 1000;
		
		public double InMegaPerKelvin => Value / 10000000;
		
		public HeatCapacity(double value)
		{
			Value = value;
		}
		
		public HeatCapacity Abs()
		{
			return new HeatCapacity(Math.Abs(Value));
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
