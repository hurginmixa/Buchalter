using System;

namespace Buchalter.Types
{
    internal struct Summa : IFormattable
    {
        private readonly double _value;

        private Summa(double value)
        {
            _value = value;
        }

        public static explicit operator Summa(double value)
        {
            return new Summa(value);
        }

        public static explicit operator double(Summa src)
        {
            return src._value;
        }

        public static Summa operator +(Summa src1, Summa src2)
        {
            return new Summa((double)src1 + (double)src2);
        }

        public static Summa operator -(Summa src1, Summa src2)
        {
            return new Summa((double)src1 - (double)src2);
        }

        public static Summa operator -(Summa src)
        {
            return new Summa(-(double)src);
        }

        public bool IsMinus
        {
            get { return _value < 0; }
        }

        public bool IsZero
        {
            get { return Math.Abs(_value) < 1e-6; }
        }

        public static Summa Parse(string input)
        {
            return (Summa) double.Parse(input);
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }
    }
}