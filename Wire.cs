using Buchalter.Types;

namespace Buchalter
{
    internal class Wire : IWire
    {
        public readonly Date Date;
        public readonly string DebSct;
        public readonly string KrdSct;
        public readonly Sum Sum;
        public readonly string Remark;
        public readonly int RunNumber;

        public Wire(Date date, string debSct, string krdSct, Sum sum, string remark, int runNumber)
        {
            Date = date;
            RunNumber = runNumber;
            DebSct = debSct;
            KrdSct = krdSct;
            Sum = sum;
            Remark = remark;
        }
    }
}
