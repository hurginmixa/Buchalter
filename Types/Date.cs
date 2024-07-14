using System;

namespace Buchalter.Types;

internal record Date(int Value) : IComparable<Date>, IFormattable
{
    public int CompareTo(Date other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Value.CompareTo(other.Value);
    }

    string IFormattable.ToString(string format, IFormatProvider formatProvider) => Value.ToString(format, formatProvider);

    public override string ToString() => Value.ToString();

    public static Date Parse(string s) => new (int.Parse(s));
}