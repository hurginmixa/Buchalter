using System;

namespace Buchalter.Types
{
    internal struct Date : IFormattable, IComparable<Date>
    {
        private readonly int _value;

        private Date(int value)
        {
            _value = value;
        }

        public static explicit operator Date(int value)
        {
            return new Date(value);
        }

        public static explicit operator int(Date src)
        {
            return src._value;
        }

        public static Date Parse(string input)
        {
            return (Date) double.Parse(input);
        }

        string IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }

        public int CompareTo(Date date)
        {
            return _value.CompareTo(date._value);
        }
    }
}