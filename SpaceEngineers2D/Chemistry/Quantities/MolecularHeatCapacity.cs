using System;
using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct MolecularHeatCapacity
	{
		private const double Accuracy = 0.00001;
		
		public static IReadOnlyList<Unit> Units = new List<Unit>
		{
			new Unit(1, "J/(mol·K) "),
			new Unit(1000, "kJ/(mol·K) "),
			new Unit(1000000, "MJ/(mol·K) "),
		};
		
		public static readonly MolecularHeatCapacity Zero = new MolecularHeatCapacity(0);
		
		public static MolecularHeatCapacity FromJoulePerMolTimesKelvin(double value)
		{
			return new MolecularHeatCapacity(value * 1);
		}
		
		public static MolecularHeatCapacity FromKiloJoulePerMolTimesKelvin(double value)
		{
			return new MolecularHeatCapacity(value * 1000);
		}
		
		public static MolecularHeatCapacity FromMegaJoulePerMolTimesKelvin(double value)
		{
			return new MolecularHeatCapacity(value * 1000000);
		}
		
		public static MolecularHeatCapacity Sum(IEnumerable<MolecularHeatCapacity> items)
		{
			return new MolecularHeatCapacity(items.Sum(item => item.Value));
		}
		
		public static MolecularHeatCapacity Average(IEnumerable<MolecularHeatCapacity> items)
		{
			return new MolecularHeatCapacity(items.Average(item => item.Value));
		}
		
		public static MolecularHeatCapacity Min(IEnumerable<MolecularHeatCapacity> items)
		{
			return new MolecularHeatCapacity(items.Min(item => item.Value));
		}
		
		public static MolecularHeatCapacity Min(params MolecularHeatCapacity[] items)
		{
			return new MolecularHeatCapacity(items.Min(item => item.Value));
		}
		
		public static MolecularHeatCapacity Max(IEnumerable<MolecularHeatCapacity> items)
		{
			return new MolecularHeatCapacity(items.Max(item => item.Value));
		}
		
		public static MolecularHeatCapacity Max(params MolecularHeatCapacity[] items)
		{
			return new MolecularHeatCapacity(items.Max(item => item.Value));
		}
		
		public static bool operator ==(MolecularHeatCapacity a, MolecularHeatCapacity b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(MolecularHeatCapacity a, MolecularHeatCapacity b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(MolecularHeatCapacity a, MolecularHeatCapacity b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(MolecularHeatCapacity a, MolecularHeatCapacity b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(MolecularHeatCapacity a, MolecularHeatCapacity b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(MolecularHeatCapacity a, MolecularHeatCapacity b)
		{
			return a.Value >= b.Value;
		}
		
		public static MolecularHeatCapacity operator +(MolecularHeatCapacity a, MolecularHeatCapacity b)
		{
			return new MolecularHeatCapacity(a.Value + b.Value);
		}
		
		public static MolecularHeatCapacity operator -(MolecularHeatCapacity a)
		{
			return new MolecularHeatCapacity(-a.Value);
		}
		
		public static MolecularHeatCapacity operator -(MolecularHeatCapacity a, MolecularHeatCapacity b)
		{
			return new MolecularHeatCapacity(a.Value - b.Value);
		}
		
		public static double operator /(MolecularHeatCapacity a, MolecularHeatCapacity b)
		{
			return a.Value / b.Value;
		}
		
		public static MolecularHeatCapacity operator *(MolecularHeatCapacity a, double b)
		{
			return new MolecularHeatCapacity(a.Value * b);
		}
		
		public static MolecularHeatCapacity operator *(double a, MolecularHeatCapacity b)
		{
			return new MolecularHeatCapacity(a * b.Value);
		}
		
		public static MolecularHeatCapacity operator /(MolecularHeatCapacity a, double b)
		{
			return new MolecularHeatCapacity(a.Value / b);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value.Equals(0);
		
		public double InJoulePerMolTimesKelvin => Value / 1;
		
		public double InKiloJoulePerMolTimesKelvin => Value / 1000;
		
		public double InMegaJoulePerMolTimesKelvin => Value / 1000000;
		
		public MolecularHeatCapacity(double value)
		{
			Value = value;
		}
		
		public MolecularHeatCapacity Abs()
		{
			return new MolecularHeatCapacity(Math.Abs(Value));
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is MolecularHeatCapacity molecularHeatCapacity && Equals(molecularHeatCapacity);
		}
		
		public bool Equals(MolecularHeatCapacity other)
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
