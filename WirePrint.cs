using Buchalter.Types;

namespace Buchalter
{
    internal class WirePrint
    {
        public readonly Date Date;
        public readonly string Sct;
        public readonly Sum Sum;
        public readonly string Remark;
        public readonly int RunNumber;

        public WirePrint(Date date, string sct, Sum sum, string remark, int runNumber)
        {
            Date = date;
            RunNumber = runNumber;
            Remark = remark;
            Sct = sct;
            Sum = sum;
        }
    }
}
