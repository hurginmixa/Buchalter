using System;

namespace Buchalter.Types
{
    internal struct AccountName : IComparable
    {
        private readonly string _value;

        private AccountName(string value)
        {
            _value = value;
        }

        public static explicit operator AccountName(string value)
        {
            return new AccountName(value);
        }

        public static explicit operator string(AccountName src)
        {
            return src._value;
        }

        public bool Equals(AccountName other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            return obj is AccountName other && Equals(other);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public override string ToString() => _value;

        public int CompareTo(object obj) => String.Compare(_value, ((AccountName) obj)._value, StringComparison.Ordinal);
    }
}
