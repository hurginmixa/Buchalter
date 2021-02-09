using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    internal static class Program
    {
        public const string WiersFileMask = "wiers*.txt";
        const string AmountsFilePath = "amounts.txt";

        static void Main()
        {
            List<Amount> amounts = LoadAmounts();
            List<WiresFile> wiersFiles = Tools.LoadWiresFiles();

            Tools.RewriteWiresFiles(wiersFiles);

            Dictionary<string, SctMoving> movingList = Tools.GetMovingList(amounts, wiersFiles);

            VD001.Run(movingList);
            VD002.Run(movingList);
            VD003.Run(movingList);
            VD004.Run(movingList);

            Console.WriteLine(movingList.Count);
        }

        public static List<Amount> LoadAmounts()
        {
            List<Amount> retValue = new List<Amount>();
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

                string sct = tokens[0].Text.Trim();
                Sum sum = Sum.Parse(tokens[1].Text.Trim().Replace(',', '.'));
                Amount amount = new Amount(sct, sum);

                retValue.Add(amount);
            }

            return retValue;
        }
    }
}
