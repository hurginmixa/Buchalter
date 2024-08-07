using System.Collections.Generic;
using System.Linq;
using Buchalter.ToolsClasses;
using Buchalter.Types;

namespace Buchalter
{
    internal static class VD004
    {
        public static void Run(Dictionary<AccountName, AccumulatedBalance> accumulatedBalances)
        {
            List<OpeningBalance> openingBalances = Tools.LoadOpeningBalances();
            List<AccountEntriesFile> accountEntriesFiles = Tools.LoadAccountEntries();

            AccountName[] paramsKrd = "расходы ссуда".Split(' ').Select(s => (AccountName)s).ToArray();
            AccountName[] paramsDbt = "доходы".Split(' ').Select(s => (AccountName)s).ToArray();

            foreach (AccountName accountName in accumulatedBalances.Keys)
            {
                bool bDbt = CheckIfExists(paramsDbt, accountName);
                bool bKrd = CheckIfExists(paramsKrd, accountName);

                if (!bDbt && !bKrd)
                {
                    continue;
                }

                Summa sEnd = accumulatedBalances[accountName].SEnd;
                if (sEnd.IsZero)
                {
                    continue;
                }

                if (bKrd)
                {
                    accountEntriesFiles[0].Insert(new AccountEntry(new Date(32), (AccountName) "все расходы", accountName, sEnd, "", accountEntriesFiles.Count + 1000));
                    accountEntriesFiles[0].Insert(new AccountEntry(new Date(32), (AccountName) "баланс", (AccountName)"все расходы", sEnd, "", accountEntriesFiles.Count + 1000));
                }
                else
                {
                    accountEntriesFiles[0].Insert(new AccountEntry(new Date(32), accountName, (AccountName)"все доходы", sEnd, "", accountEntriesFiles.Count + 1000));
                    accountEntriesFiles[0].Insert(new AccountEntry(new Date(32), (AccountName)"все доходы", (AccountName)"баланс", sEnd, "", accountEntriesFiles.Count + 1000));
                }
            }

            accumulatedBalances = Tools.GetBalanceList(openingBalances, accountEntriesFiles);

            VD001.Run(accumulatedBalances, "vd004.txt");
        }

        private static bool CheckIfExists(AccountName[] accountNamesToCheck, AccountName accountName)
        {
            bool result = false;

            for (int i = 0; !result && i < accountNamesToCheck.Length; i++)
            {
                ParseString.Token[] tokens = ParseString.Parse(((string)accountName).ToLower(), ' ', '\t');

                result = tokens.Length > 0 && tokens[0].Text.StartsWith(((string)accountNamesToCheck[i]).ToLower());
            }

            return result;
        }
    }
}