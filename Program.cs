using System;
using System.Collections.Generic;
using Buchalter;
using Buchalter.ToolsClasses;
using Buchalter.Types;

List<OpeningBalance> openingBalances = Tools.LoadOpeningBalances();
List<AccountEntriesFile> entriesFiles = Tools.LoadAccountEntries();

Tools.RewriteAccountEntriesFiles(entriesFiles);

Dictionary<AccountName, AccumulatedBalance> balances = Tools.GetBalanceList(openingBalances, entriesFiles);

VD001.Run(balances);
VD002.Run(balances);
VD003.Run(balances);
VD004.Run(balances);
VD005.Run(balances);

Console.WriteLine(balances.Count);
