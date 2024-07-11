using Buchalter.Types;

namespace Buchalter
{
    internal class AccountEntry(Date date, AccountName debAccount, AccountName krdAccount, Summa summa, string remark, int runNumber) : IAccountEntry
    {
        public readonly Date Date = date;
        
        public readonly AccountName DebAccount = debAccount;
        
        public readonly AccountName KrdAccount = krdAccount;
        
        public readonly Summa Summa = summa;
        
        public readonly string Remark = remark;

        public readonly int RunNumber = runNumber;
    }
}
