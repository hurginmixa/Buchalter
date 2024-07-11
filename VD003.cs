using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    internal static class VD003
    {
        public static void Run(Dictionary<AccountName, AccumulatedBalance> balances)
        {
            using StreamWriter tw = new StreamWriter("vd003.txt", false, Encoding.UTF8);
            
            List<AccountName> accountNames = new List<AccountName>(balances.Keys);
            accountNames.Sort();

            foreach (AccountName accountName in accountNames)
            {
                AccountReport(tw, accountName, balances[accountName]);
            }
        }

        private static void AccountReport(TextWriter tw, AccountName accountName, AccumulatedBalance accumulatedBalance)
        {
            Summa amount = accumulatedBalance.SBegin;

            tw.WriteLine("<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>");
            tw.WriteLine();
            tw.WriteLine($"{accountName,-52} {amount,20:0.00}");

            List<AccountPrintInfo> accountForPrintList = new List<AccountPrintInfo>();

            foreach(AccountEntry wier in accumulatedBalance.DebEntries)
            {
                AccountPrintInfo accountPrintInfo = new AccountPrintInfo(wier.Date, wier.KrdAccount, wier.Summa, wier.Remark, wier.RunNumber);
                accountForPrintList.Add(accountPrintInfo);
            }

            foreach(AccountEntry entry in accumulatedBalance.KrdEntries)
            {
                AccountPrintInfo accountPrintInfo = new AccountPrintInfo(entry.Date, entry.DebAccount, -entry.Summa, entry.Remark, entry.RunNumber);
                accountForPrintList.Add(accountPrintInfo);
            }

            if (accountForPrintList.Count == 0)
            {
                return;
            }

            tw.WriteLine("--------------------------------------------------------------------------");

            int Comparison(AccountPrintInfo p1, AccountPrintInfo p2)
            {
                int tmp = p1.Date.CompareTo(p2.Date);
                return tmp != 0 ? tmp : p1.RunNumber.CompareTo(p2.RunNumber);
            }

            accountForPrintList.Sort(Comparison);

            foreach (AccountPrintInfo accountForPrint in accountForPrintList)
            {
                amount += accountForPrint.Summa;

                tw.Write($"{accountForPrint.Date:00} {accountForPrint.AccountName,-31}");

                tw.WriteLine(!accountForPrint.Summa.IsMinus
                    ? $"{accountForPrint.Summa,12:0.00}  {amount,25:0.00}  {accountForPrint.Remark}"
                    : $"{-accountForPrint.Summa,25:0.00}  {amount,12:0.00}  {accountForPrint.Remark}");
            }
        }
    }
}