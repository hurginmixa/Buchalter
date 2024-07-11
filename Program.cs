using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    internal static class Program
    {
        public const string AccountEntriesFileMask = "wiers*.txt";
        const string AmountsFilePath = "amounts.txt";

        static void Main()
        {
            List<OpeningBalance> openingBalances = LoadOpeningBalances();
            List<AccountEntriesFile> entriesFiles = Tools.LoadAccountEntries();

            Tools.RewriteAccountEntriesFiles(entriesFiles);

            Dictionary<AccountName, AccumulatedBalance> balances = Tools.GetBalanceList(openingBalances, entriesFiles);

            VD001.Run(balances);
            VD002.Run(balances);
            VD003.Run(balances);
            VD004.Run(balances);
            VD005.Run(balances);

            Console.WriteLine(balances.Count);
        }

        public static List<OpeningBalance> LoadOpeningBalances()
        {
            List<OpeningBalance> retValue = new List<OpeningBalance>();
            foreach (string line in File.ReadAllLines(AmountsFilePath, Encoding.UTF8))
            {
                string rline = line.Trim(' ', '\t');
                if (rline.Length == 0)
                {
                    break;
                }

                ParseString.Token[] tokens = ParseString.Parse(rline, '!', '|');

                if (tokens.Length < 2)
                {
                    throw new Exception("Invalid field number");
                }

                AccountName accountName = (AccountName) tokens[0].Text.Trim();
                Summa summa = Summa.Parse(tokens[1].Text.Trim().Replace(',', '.'));
                OpeningBalance openingBalance = new OpeningBalance(accountName, summa);

                retValue.Add(openingBalance);
            }

            return retValue;
        }
    }
}
