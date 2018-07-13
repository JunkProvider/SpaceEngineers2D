using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpaceEngineers2D.Chemistry.Quantities
{
	public struct MolecularMass
	{
		private const double Accuracy = 0.00001;
		
		public static readonly MolecularMass Zero = new MolecularMass(0);
		
		public static MolecularMass FromGramPerMol(double value)
		{
			return new MolecularMass(value);
		}
		
		public static MolecularMass Sum(IEnumerable<MolecularMass> items)
		{
			return new MolecularMass(items.Sum(item => item.Value));
		}
		
		public static MolecularMass Average(IEnumerable<MolecularMass> items)
		{
			return new MolecularMass(items.Average(item => item.Value));
		}
		
		public static MolecularMass Min(IEnumerable<MolecularMass> items)
		{
			return new MolecularMass(items.Min(item => item.Value));
		}
		
		public static MolecularMass Max(IEnumerable<MolecularMass> items)
		{
			return new MolecularMass(items.Max(item => item.Value));
		}
		
		public static bool operator ==(MolecularMass a, MolecularMass b)
		{
			return a.Equals(b);
		}
		
		public static bool operator !=(MolecularMass a, MolecularMass b)
		{
			return !a.Equals(b);
		}
		
		public static bool operator <(MolecularMass a, MolecularMass b)
		{
			return a.Value < b.Value;
		}
		
		public static bool operator <=(MolecularMass a, MolecularMass b)
		{
			return a.Value <= b.Value;
		}
		
		public static bool operator >(MolecularMass a, MolecularMass b)
		{
			return a.Value > b.Value;
		}
		
		public static bool operator >=(MolecularMass a, MolecularMass b)
		{
			return a.Value >= b.Value;
		}
		
		public static MolecularMass operator +(MolecularMass a, MolecularMass b)
		{
			return new MolecularMass(a.Value + b.Value);
		}
		
		public static MolecularMass operator -(MolecularMass a, MolecularMass b)
		{
			return new MolecularMass(a.Value - b.Value);
		}
		
		public static double operator /(MolecularMass a, MolecularMass b)
		{
			return a.Value / b.Value;
		}
		
		public static MolecularMass operator *(MolecularMass a, double b)
		{
			return new MolecularMass(a.Value * b);
		}
		
		public static MolecularMass operator *(double a, MolecularMass b)
		{
			return new MolecularMass(a * b.Value);
		}
		
		public static MolecularMass operator /(MolecularMass a, double b)
		{
			return new MolecularMass(a.Value / b);
		}
		
		public static MolecularVolume operator /(MolecularMass molecularMass, Density density)
		{
			return MolecularVolume.FromCubicCentimetersPerMol(molecularMass.InGramPerMol / density.InGramPerCubicCentimeter);
		}
		
		public readonly double Value;
		
		public bool IsZero => Value == 0;
		
		public double InGramPerMol => Value;
		
		public MolecularMass(double value)
		{
			Value = value;
		}
		
		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
			{
				return false;
			}
			
			return obj is MolecularMass molecularMass && Equals(molecularMass);
		}
		
		public bool Equals(MolecularMass other)
		{
			return Math.Abs(Value - other.Value) < Accuracy;
		}
		
		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}
		
		public override string ToString()
		{
			return Value.ToString("0.00", CultureInfo.InvariantCulture) + "g/mol";
		}
		
	}
}
