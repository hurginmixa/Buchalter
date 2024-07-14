using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Buchalter.Types;

namespace Buchalter.ToolsClasses
{
    internal static class Tools
    {
        private class AccountEntryDelimiter : IAccountEntry
        {
            public readonly string Remark;

            public AccountEntryDelimiter(string remark)
            {
                Remark = remark;
            }
        }

        public static List<AccountEntriesFile> LoadAccountEntries()
        {
            string[] files = Directory.GetFiles(".", CommonData.AccountEntriesFileMask);

            List<AccountEntriesFile> list = new List<AccountEntriesFile>();

            foreach (string file in files)
            {
                list.Add(new AccountEntriesFile(file));
            }

            return list;
        }

        public static List<IAccountEntry> LoadAccountEntries(string accountEntriesFileName)
        {
            List<IAccountEntry> retValue = new List<IAccountEntry>();

            string[] strings = File.ReadAllLines(accountEntriesFileName, Encoding.UTF8);
            for (int i = 0; i < strings.Length; i++)
            {
                try
                {
                    string line = strings[i].Trim(' ', '\t');
                    if (line.Length == 0)
                    {
                        retValue.Add(null);
                        continue;
                    }

                    if (line == new string('=', line.Length))
                    {
                        retValue.Add(new AccountEntryDelimiter(line));
                        continue;
                    }

                    ParseString.Token[] tokens = ParseString.Parse(line, '!', '|');
                    if (tokens.Length < 4)
                    {
                        throw new Exception("Invalid field number");
                    }

                    Date date = Date.Parse(tokens[0].Text.Trim());
                    AccountName debSct = (AccountName)tokens[1].Text.Trim();
                    AccountName krdSct = (AccountName)tokens[2].Text.Trim();
                    Summa summa = Summa.Parse(tokens[3].Text.Trim().Replace(',', '.'));
                    string remark = tokens.Length >= 5 ? tokens[4].Text.Trim() : string.Empty;

                    AccountEntry accountEntry = new AccountEntry(date, debSct, krdSct, summa, remark, i);

                    retValue.Add(accountEntry);
                }
                catch (Exception e)
                {
                    throw new Exception("Exception occure at " + i + " line", e);
                }
            }

            return retValue;
        }

        public static Dictionary<AccountName, AccumulatedBalance> GetBalanceList(IEnumerable<OpeningBalance> amounts, IEnumerable<AccountEntriesFile> entriesFiles)
        {
            Dictionary<AccountName, AccumulatedBalance> balances = new Dictionary<AccountName, AccumulatedBalance>();

            foreach (OpeningBalance amount in amounts)
            {
                if (!balances.TryGetValue(amount.AccountName, out AccumulatedBalance accountAmount))
                {
                    accountAmount = new AccumulatedBalance();
                    balances[amount.AccountName] = accountAmount;
                }

                accountAmount.SBegin += amount.Summa;
            }

            foreach (AccountEntriesFile file in entriesFiles)
            {
                IEnumerable<AccountEntry> accountEntries = file.AccountEntries
                    .Where(entry => entry != null && entry.GetType() == typeof(AccountEntry))
                    .Select(entry => (AccountEntry)entry);

                foreach (AccountEntry entry in accountEntries)
                {
                    if (!balances.TryGetValue(entry.DebAccount, out AccumulatedBalance amount))
                    {
                        amount = new AccumulatedBalance();
                        balances[entry.DebAccount] = amount;
                    }
                    amount.DebEntries.Add(entry);

                    if (!balances.TryGetValue(entry.KrdAccount, out amount))
                    {
                        amount = new AccumulatedBalance();
                        balances[entry.KrdAccount] = amount;
                    }

                    amount.KrdEntries.Add(entry);
                }
            }

            return balances;
        }

        public static void RewriteAccountEntriesFiles(List<AccountEntriesFile> entriesFiles)
        {
            foreach (AccountEntriesFile entriesFile in entriesFiles)
            {
                string bakFileName = MakeBakFileName(entriesFile.FileName);
                File.Delete(bakFileName);
                File.Move(entriesFile.FileName, bakFileName);

                IReadOnlyList<IAccountEntry> accountEntries = entriesFile.AccountEntries;
                string[] lines = new string[accountEntries.Count];

                for (int i = 0; i < accountEntries.Count; i++)
                {
                    IAccountEntry accountEntryTmp = accountEntries[i];
                    if (accountEntryTmp == null)
                    {
                        lines[i] = string.Empty;
                        continue;
                    }

                    if (accountEntryTmp.GetType() == typeof(AccountEntryDelimiter))
                    {
                        lines[i] = ((AccountEntryDelimiter)accountEntryTmp).Remark;
                        continue;
                    }

                    AccountEntry accountEntry = (AccountEntry)accountEntryTmp;
                    lines[i] = $"{accountEntry.Date:00} ! {accountEntry.DebAccount,-20} ! {accountEntry.KrdAccount,-20} ! {accountEntry.Summa,12:0.00} ! {accountEntry.Remark}";
                }

                File.WriteAllLines(entriesFile.FileName, lines, Encoding.UTF8);
            }
        }

        private static string MakeBakFileName(string srcFileName)
        {
            string directoryName = Path.GetDirectoryName(srcFileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(srcFileName);

            return Path.Combine(directoryName, fileNameWithoutExtension) + ".bak";
        }

        public static List<OpeningBalance> LoadOpeningBalances()
        {
            List<OpeningBalance> retValue = new List<OpeningBalance>();

            foreach (string line in File.ReadAllLines(CommonData.AmountsFilePath, Encoding.UTF8))
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