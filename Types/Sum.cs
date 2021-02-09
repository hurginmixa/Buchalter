using System;

namespace Buchalter.Types
{
    internal struct Sum : IFormattable
    {
        private readonly double _value;

        private Sum(double value)
        {
            _value = value;
        }

        public static explicit operator Sum(double value)
        {
            return new Sum(value);
        }

        public static explicit operator double(Sum src)
        {
            return src._value;
        }

        public static Sum operator +(Sum src1, Sum src2)
        {
            return new Sum((double)src1 + (double)src2);
        }

        public static Sum operator -(Sum src1, Sum src2)
        {
            return new Sum((double)src1 - (double)src2);
        }

        public static Sum operator -(Sum src)
        {
            return new Sum(-(double)src);
        }

        public bool IsMinus
        {
            get { return _value < 0; }
        }

        public bool IsZero
        {
            get { return Math.Abs(_value) < 1e-6; }
        }

        public static Sum Parse(string input)
        {
            return (Sum) double.Parse(input);
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }
    }
}