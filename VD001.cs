using System.Collections.Generic;
using System.IO;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    static class VD001
    {
        public static void Run(Dictionary<AccountName, AccumulatedBalance> balances)
        {
            Run(balances, "vd001.txt");
        }

        public static void Run(Dictionary<AccountName, AccumulatedBalance> balances, string outputFilePath)
        {
            using (StreamWriter tw = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            {
                Summa nOst = new Summa(0);
                Summa deb = new Summa(0);
                Summa krd = new Summa(0);
                Summa kOst = new Summa(0);

                List<AccountName> keys = new List<AccountName>(balances.Keys);
                keys.Sort();

                foreach (AccountName key in keys)
                {
                    AccumulatedBalance balance = balances[key];

                    tw.WriteLine("{0,-20} {1,12:0.00} {2,12:0.00} {3,12:0.00} {4,12:0.00}", key, balance.SBegin, balance.SDebt, balance.SKred, balance.SEnd);

                    nOst += balance.SBegin;
                    deb += balance.SDebt;
                    krd += balance.SKred;
                    kOst += balance.SEnd;
                }

                tw.WriteLine(new string('-', 72));
                tw.WriteLine("{0,-20} {1,12:0.00} {2,12:0.00} {3,12:0.00} {4,12:0.00}", "Amount", nOst, deb, krd, kOst);
            }
        }
    }
}