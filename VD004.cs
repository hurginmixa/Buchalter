using System.Collections.Generic;
using Buchalter.Types;

namespace Buchalter
{
    internal static class VD004
    {
        public static void Run(Dictionary<string, SctMoving> movingList)
        {
            List<Amount> amounts = Program.LoadAmounts();
            List<WiresFile> wiersFiles = Tools.LoadWiresFiles();

            string[] paramsKrd = "расходы ссуда".Split(' ');
            string[] paramsDbt = "доходы".Split(' ');

            foreach (string sctName in movingList.Keys)
            {
                bool bDbt = CheckIfExists(paramsDbt, sctName);
                bool bKrd = CheckIfExists(paramsKrd, sctName);

                if (!bDbt && !bKrd)
                {
                    continue;
                }

                Sum sKont = movingList[sctName].SKont;
                if(sKont.IsZero)
                {
                    continue;
                }

                if (bKrd)
                {
                    wiersFiles[0].Insert(new Wire((Date)32, "все расходы", sctName, sKont, "", wiersFiles.Count + 1000));
                    wiersFiles[0].Insert(new Wire((Date)32, "баланс", "все расходы", sKont, "", wiersFiles.Count + 1000));
                }
                else
                {
                    wiersFiles[0].Insert(new Wire((Date)32, sctName, "все доходы", sKont, "", wiersFiles.Count + 1000));
                    wiersFiles[0].Insert(new Wire((Date)32, "все доходы", "баланс", sKont, "", wiersFiles.Count + 1000));
                }
            }

            movingList = Tools.GetMovingList(amounts, wiersFiles);

            VD001.Run(movingList, "vd004.txt");
        }

        private static bool CheckIfExists(string[] checkParams, string testStrig)
        {
            bool b1 = false;
            for (int i = 0; !b1 && i < checkParams.Length; i++)
            {
                ParseString.Token[] tokens = ParseString.Parse(testStrig.ToLower(), ' ', '\t');

                b1 = tokens.Length > 0 && (tokens[0].Text.StartsWith(checkParams[i].ToLower()));
            }
            return b1;
        }
    }
}