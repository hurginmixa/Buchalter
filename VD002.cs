using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    internal static class VD002
    {
        public static void Run(Dictionary<AccountName, AccumulatedBalance> amountList)
        {
            Summa sNach = new Summa(0);
            Summa sDebt = new Summa(0);
            Summa sKred = new Summa(0);
            Summa sKont = new Summa(0);

            List<AccountName> sctNames = new List<AccountName>(amountList.Keys);
            sctNames.Sort();

            using StreamWriter tw = new StreamWriter("vd002.txt", false, Encoding.UTF8);

            foreach (AccountName key in sctNames)
            {
                AccumulatedBalance balance = amountList[key];

                tw.WriteLine($"{key,-20} {balance.SBegin,12:0.00} {balance.SDebt,12:0.00} {balance.SKred,12:0.00} {balance.SEnd,12:0.00}");

                sNach += balance.SBegin;
                sDebt += balance.SDebt;
                sKred += balance.SKred;
                sKont += balance.SEnd;

                List<AccountPrintInfo> accountForPrintList = new List<AccountPrintInfo>();
                foreach (AccountEntry entry in balance.DebEntries)
                {
                    accountForPrintList.Add(new AccountPrintInfo(entry.Date, entry.KrdAccount, entry.Summa, entry.Remark, entry.RunNumber));
                }

                foreach (AccountEntry entry in balance.KrdEntries)
                {
                    accountForPrintList.Add(new AccountPrintInfo(entry.Date, entry.DebAccount, -entry.Summa, entry.Remark, entry.RunNumber));
                }

                if (accountForPrintList.Count > 0)
                {
                    tw.WriteLine(new string('-', 72));

                    int Comparison(AccountPrintInfo w1, AccountPrintInfo w2)
                    {
                        int compare = w1.Date.CompareTo(w2.Date);
                        return compare != 0 ? compare : w1.RunNumber.CompareTo(w2.RunNumber);
                    }

                    accountForPrintList.Sort(Comparison);

                    foreach (var accountFor in accountForPrintList)
                    {
                        tw.Write("{0:00} {1,-31}", accountFor.Date, accountFor.AccountName);
                        tw.WriteLine(!accountFor.Summa.IsMinus
                            ? $"{accountFor.Summa,12:0.00}                  {accountFor.Remark}"
                            : $"{-accountFor.Summa,25:0.00}     {accountFor.Remark}");
                    }
                }

                tw.WriteLine(new string('=', 72));
            }

            tw.WriteLine($"{"Amount",-20} {sNach,12:0.00} {sDebt,12:0.00} {sKred,12:0.00} {sKont,12:0.00}");
        }
    }
}