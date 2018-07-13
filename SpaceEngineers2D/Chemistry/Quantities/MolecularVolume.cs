using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct MolecularVolume
	{
		private const double Accuracy = 0.00001;
		
		public static readonly MolecularVolume Zero = new MolecularVolume(0);
		
		public static MolecularVolume FromCubicCentimetersPerMol(double value)
		{
			return new MolecularVolume(value);
		}
		
		public static MolecularVolume Sum(IEnumerable<MolecularVolume> items)
		{
			return new MolecularVolume(items.Sum(item => item.Value));
		}
		
		public static MolecularVolume Average(IEnumerable<MolecularVolume> items)
		{
			return new MolecularVolume(items.Average(item => item.Value));
		}
		
		public static MolecularVolume Min(IEnumerable<MolecularVolume> items)
		{
			return new MolecularVolume(items.Min(item => item.Value));
		}
		
		public static MolecularVolume Max(IEnumerable<MolecularVolume> items)
		{
			return new MolecularVolume(items.Max(item => item.Value));
		}
		
		public static bool operator ==(MolecularVolume a, MolecularVolume b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(MolecularVolume a, MolecularVolume b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(MolecularVolume a, MolecularVolume b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(MolecularVolume a, MolecularVolume b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(MolecularVolume a, MolecularVolume b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(MolecularVolume a, MolecularVolume b)
		{
			return a.Value >= b.Value;
		}
		
		public static MolecularVolume operator +(MolecularVolume a, MolecularVolume b)
		{
			return new MolecularVolume(a.Value + b.Value);
		}
		
		public static MolecularVolume operator -(MolecularVolume a, MolecularVolume b)
		{
			return new MolecularVolume(a.Value - b.Value);
		}
		
		public static double operator /(MolecularVolume a, MolecularVolume b)
		{
			return a.Value / b.Value;
		}
		
		public static MolecularVolume operator *(MolecularVolume a, double b)
		{
			return new MolecularVolume(a.Value * b);
		}
		
		public static MolecularVolume operator *(double a, MolecularVolume b)
		{
			return new MolecularVolume(a * b.Value);
		}
		
		public static MolecularVolume operator /(MolecularVolume a, double b)
		{
			return new MolecularVolume(a.Value / b);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value == 0;
		
		public double InCubicCentimetersPerMol => Value;
		
		public MolecularVolume(double value)
		{
			Value = value;
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is MolecularVolume molecularVolume && Equals(molecularVolume);
		}
		
		public bool Equals(MolecularVolume other)
		{
			return Math.Abs(Value - other.Value) < Accuracy;
		}
		
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		
		public override string ToString()
		{
			return Value.ToString("0.00", CultureInfo.InvariantCulture) + "cm³/mol";
		}
		
	}
}
