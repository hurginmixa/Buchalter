using System;
using System.Globalization;

namespace Buchalter.Types;

internal record Summa(double Value) : IFormattable
{
    public static explicit operator Summa(double value) => new(value);

    public static explicit operator double(Summa src) => src.Value;

    public static Summa operator +(Summa src1, Summa src2) => new((double)src1 + (double)src2);

    public static Summa operator -(Summa src1, Summa src2) => new((double)src1 - (double)src2);

    public static Summa operator -(Summa src) => new(-(double)src);

    public bool IsMinus => Value < 0;

    public bool IsZero => Math.Abs(Value) < 1e-6;

    public static Summa Parse(string input) => (Summa) double.Parse(input);

    string IFormattable.ToString(string format, IFormatProvider formatProvider) => Value.ToString(format, formatProvider);

    public override string ToString() => Value.ToString(CultureInfo.InvariantCulture);
}