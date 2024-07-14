using System.Collections.Generic;
using Buchalter.Types;

namespace Buchalter
{
    internal class AccumulatedBalance
    {
        private readonly List<AccountEntry> _debEntries = [];
        private readonly List<AccountEntry> _krdEntries = [];
        private Summa _sBegin = (Summa) 0;

        public Summa SBegin
        {
            get => _sBegin;
            set => _sBegin = value;
        }

        public Summa SDebt
        {
            get
            {
                Summa ret = new Summa(0);
                foreach (AccountEntry entry in _debEntries)
                {
                    ret += entry.Summa;
                }

                return ret;
            }
        }

        public Summa SKred
        {
            get
            {
                Summa ret = new Summa(0);
                foreach (AccountEntry entry in _krdEntries)
                {
                    ret += entry.Summa;
                }

                return ret;
            }
        }

        public Summa SEnd => SBegin + SDebt - SKred;

        public List<AccountEntry> DebEntries => _debEntries;

        public List<AccountEntry> KrdEntries => _krdEntries;
    }
}
