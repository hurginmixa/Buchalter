using Buchalter.Types;

namespace Buchalter
{
    class Amount
    {
        public readonly string Sct;
        public readonly Sum Sum;

        public Amount(string sct, Sum sum)
        {
            Sct = sct;
            Sum = sum;
        }
    }
}
