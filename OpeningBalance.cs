using Buchalter.Types;

namespace Buchalter
{
    internal class OpeningBalance(AccountName accountName, Summa summa)
    {
        public readonly AccountName AccountName = accountName;
        public readonly Summa Summa = summa;
    }
}
