using System;

namespace CellularAutomata.Outputs.Vectors
{

	public struct DensityVector : IComparable<DensityVector>
	{

		public static DensityVector Zero {
			get { return new DensityVector (0D, 0D, 1D); }
		}

		public static DensityVector Empty {
			get { return new DensityVector (0D, 0D, 0D); }
		}

		public static DensityVector MaxValue {
			get { return new DensityVector (Double.MaxValue, Double.MaxValue, Double.MaxValue); }
		}

		public static DensityVector MaxSafeValue {
			get { return new DensityVector (Double.MaxValue, Double.MaxValue); }
		}

		public static DensityVector MinValue {
			get { return new DensityVector (Double.MinValue, Double.MinValue, Double.MinValue); }
		}

		public static DensityVector MinSafeValue {
			get { return new DensityVector (Double.MinValue, Double.MinValue); }
		}

		public static DensityVector operator + (DensityVector a, DensityVector b)
		{
			DensityVector result = new DensityVector ();
			result.x = a.x + b.x;
			result.y = a.y + b.y;
			result.d = a.d + b.d;
			return result;
		}

		public static DensityVector operator - (DensityVector a, DensityVector b)
		{
			DensityVector result = new DensityVector ();
			result.x = a.x - b.x;
			result.y = a.y - b.y;
			result.d = a.d - b.d;
			return result;
		}

		public static DensityVector operator * (DensityVector a, DensityVector b)
		{
			DensityVector result = new DensityVector ();
			result.x = a.x * b.x;
			result.y = a.y * b.y;
			result.d = a.d * b.d;
			return result;
		}

		public static DensityVector operator / (DensityVector a, DensityVector b)
		{
			DensityVector result = new DensityVector ();
			result.x = a.x / b.x;
			result.y = a.y / b.y;
			result.d = a.d / b.d;
			return result;
		}

		public static bool operator == (DensityVector a, DensityVector b)
		{
			return (0 == a.CompareTo (b));
		}

		public static bool operator != (DensityVector a, DensityVector b)
		{
			return !(a == b);
		}

		public static bool operator < (DensityVector a, DensityVector b)
		{
			return (0 < b.CompareTo (a));
		}

		public static bool operator > (DensityVector a, DensityVector b)
		{
			return (0 > b.CompareTo (a));
		}

		public static bool operator <= (DensityVector a, DensityVector b)
		{
			return !(a > b);
		}

		public static bool operator >= (DensityVector a, DensityVector b)
		{
			return !(a < b);
		}

		public static explicit operator double (DensityVector vector)
		{
			double magnitude = Magnitude (vector);
			return (magnitude * vector.D);
		}

		static public implicit operator DensityVector (double n)
		{
			return new DensityVector (n, n, n);
		}

		public static double Magnitude (DensityVector vector)
		{
			DensityVector square = Pow (vector, 2D);
			double magnitude = Math.Sqrt (square.x + square.y);
			return magnitude;
		}

		public static DensityVector Pow (DensityVector vector, double exponent)
		{
			DensityVector result = new DensityVector ();
			result.x = Math.Pow (vector.x, exponent);
			result.y = Math.Pow (vector.y, exponent);
			result.d = Math.Pow (vector.d, exponent);
			return result;
		}

		public static DensityVector Sqrt (DensityVector vector)
		{
			DensityVector result = new DensityVector ();
			result.x = Math.Sqrt (vector.x);
			result.y = Math.Sqrt (vector.y);
			result.d = Math.Sqrt (vector.d);
			return result;
		}

		public double X {
			get { return this.x; }
			set { x = value; }
		}

		public double Y {
			get { return this.y; }
			set { X = value; }
		}

		public double D {
			get { return this.d; }
			set { d = value; }
		}

		public DensityVector (double x, double y, double d)
		{
			this.x = x;
			this.y = y;
			this.d = d;
		}

		public DensityVector (double magnitude, double radians)
		{
			this.x = magnitude * Math.Cos (radians);
			this.y = magnitude * Math.Sin (radians);
			this.d = 1D;
		}

		public int CompareTo (DensityVector other)
		{
			return ((double)this).CompareTo ((double)other);
		}

		private double x;
		private double y;
		private double d;
	
	}

}

