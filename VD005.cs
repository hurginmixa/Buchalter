using Buchalter.Types;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Buchalter
{
    internal class VD005
    {
        public static void Run(Dictionary<AccountName, AccumulatedBalance> balances)
        {
            using StreamWriter tw = new StreamWriter("vd005.txt", false, Encoding.UTF8);
            
            List<AccountName> accountNames = new List<AccountName>(balances.Keys);
            accountNames.Sort();

            foreach (AccountName accountName in accountNames)
            {
                AccountReport(tw, accountName, balances[accountName]);
            }
        }

        private static void AccountReport(StreamWriter tw, AccountName accountName, AccumulatedBalance balance)
        {
            tw.WriteLine();
            tw.WriteLine("<><><><><><><><>><><><><><><><><><><><><><><><><><><><><>");
            tw.WriteLine($"{accountName,-20} {balance.SBegin,20:0.00}");
            tw.WriteLine();

            var debList = balance.DebEntries.Select(e => new {e.Date, e.Summa, Dbt = true}).ToArray();
            var krdList = balance.KrdEntries.Select(e => new {e.Date, e.Summa, Dbt = false}).ToArray();
            var all = debList.Concat(krdList).GroupBy(i => i.Date).OrderBy(i => i.Key).ToArray();


            Summa total = (Summa) balance.SBegin;

            foreach (var dateGroup in all)
            {
                Date date = dateGroup.Key;

                Summa dbt = (Summa) 0;
                Summa krd = (Summa) 0;

                foreach (var fe in dateGroup)
                {
                    if (fe.Dbt)
                    {
                        dbt += fe.Summa;
                    }
                    else
                    {
                        krd += fe.Summa;
                    }
                }

                total += dbt - krd;

                tw.WriteLine($"{date:00} {dbt,12:0.00} {krd,12:0.00} {total,12:0.00}");
            }
        }
    }
}
