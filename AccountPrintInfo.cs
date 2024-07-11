using Buchalter.Types;

namespace Buchalter
{
    internal class AccountPrintInfo(Date date, AccountName accountName, Summa summa, string remark, int runNumber)
    {
        public readonly Date Date = date;
        
        public readonly AccountName AccountName = accountName;
        
        public readonly Summa Summa = summa;
        
        public readonly string Remark = remark;
        
        public readonly int RunNumber = runNumber;
    }
}
