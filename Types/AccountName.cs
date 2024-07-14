using System;

namespace Buchalter.Types;

internal record AccountName(string Value) : IComparable
{
    public static explicit operator string(AccountName src) => src.Value;

    public static explicit operator AccountName(string src) => new(src);

    public override string ToString() => Value;

    public int CompareTo(object obj) => string.Compare(Value, ((AccountName) obj)?.Value, StringComparison.Ordinal);
}