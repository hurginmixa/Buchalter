using System.Text.RegularExpressions;
using Buchalter.Types;

namespace Buchalter
{
    class Wier : IWier
    {
        public readonly Date Date;
        public readonly string DebSct;
        public readonly string KrdSct;
        public readonly Sum Sum;
        public readonly string Remark;
        public readonly int RunNumber;

        public Wier(Date date, string debSct, string krdSct, Sum sum, string remark, int runNumber)
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
