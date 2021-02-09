using System.Collections.Generic;
using System.IO;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    static class VD001
    {
        public static void Run(Dictionary<string, SctMoving> movings)
        {
            Run(movings, "vd001.txt");
        }

        public static void Run(Dictionary<string, SctMoving> movings, string outputFilePath)
        {
            using (StreamWriter tw = new StreamWriter(outputFilePath, false, Encoding.UTF8))
            {
                Sum nOst = new Sum();
                Sum deb = new Sum();
                Sum krd = new Sum();
                Sum kOst = new Sum();

                List<string> keys = new List<string>(movings.Keys);
                keys.Sort();

                foreach (string key in keys)
                {
                    SctMoving moving = movings[key];

                    tw.WriteLine("{0,-20} {1,12:0.00} {2,12:0.00} {3,12:0.00} {4,12:0.00}", key, moving.SNach, moving.SDebt, moving.SKred, moving.SKont);

                    nOst += moving.SNach;
                    deb += moving.SDebt;
                    krd += moving.SKred;
                    kOst += moving.SKont;
                }

                tw.WriteLine(new string('-', 72));
                tw.WriteLine("{0,-20} {1,12:0.00} {2,12:0.00} {3,12:0.00} {4,12:0.00}", "Amount", nOst, deb, krd, kOst);
            }
        }
    }
}