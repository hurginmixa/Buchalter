using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    static class Program
    {
        public const string WiersFileMask = "wiers*.txt";
        const string AmountsFilePath = "amounts.txt";

        static void Main()
        {
            List<Amount> amounts = LoadAmounts();
            List<WiersFile> wiersFiles = Tools.LoadWiersFiles();

            Tools.RewriteWiersFiles(wiersFiles);

            Dictionary<string, SctMoving> movings = Tools.GetMovings(amounts, wiersFiles);

            VD001.Run(movings);
            VD002.Run(movings);
            VD003.Run(movings);
            VD004.Run(movings);

            Console.WriteLine(movings.Count);
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
