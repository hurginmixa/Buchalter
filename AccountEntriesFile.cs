using System.Collections.Generic;
using Buchalter.ToolsClasses;

namespace Buchalter
{
    internal class AccountEntriesFile
    {
        private readonly List<IAccountEntry> _accountEntries;
        private readonly string _fileName;

        public AccountEntriesFile(string fileName)
        {
            _fileName = fileName;

            _accountEntries = Tools.LoadAccountEntries(_fileName);
        }

        public IReadOnlyList<IAccountEntry> AccountEntries => _accountEntries;

        public string FileName => _fileName;

        public void Insert(AccountEntry accountEntry)
        {
            _accountEntries.Insert(0, accountEntry);
        }
    }
}