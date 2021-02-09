using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    internal static class VD002
    {
        public static void Run(Dictionary<string, SctMoving> movings)
        {
            using (StreamWriter tw = new StreamWriter("vd002.txt", false, Encoding.UTF8))
            {
                Sum sNach = new Sum();
                Sum sDebt = new Sum();
                Sum sKred = new Sum();
                Sum sKont = new Sum();

                List<string> sctNames = new List<string>(movings.Keys);
                sctNames.Sort();

                foreach (string key in sctNames)
                {
                    SctMoving moving = movings[key];

                    tw.WriteLine("{0,-20} {1,12:0.00} {2,12:0.00} {3,12:0.00} {4,12:0.00}", key, moving.SNach,
                                 moving.SDebt, moving.SKred, moving.SKont);

                    sNach += moving.SNach;
                    sDebt += moving.SDebt;
                    sKred += moving.SKred;
                    sKont += moving.SKont;

                    List<WierPrint> wierPrintList = new List<WierPrint>();
                    foreach (Wier wier in moving.DebList)
                    {
                        wierPrintList.Add(new WierPrint(wier.Date, wier.KrdSct, wier.Sum, wier.Remark, wier.RunNumber));
                    }
                    foreach (Wier wier in moving.KrdList)
                    {
                        wierPrintList.Add(new WierPrint(wier.Date, wier.DebSct, -wier.Sum, wier.Remark, wier.RunNumber));
                    }

                    if (wierPrintList.Count > 0)
                    {
                        tw.WriteLine(new string('-', 72));

                        Comparison<WierPrint> comparison = delegate(WierPrint w1, WierPrint w2)
                                                           {
                                                               int compare = w1.Date.CompareTo(w2.Date);
                                                               return compare != 0 ? compare : w1.RunNumber.CompareTo(w2.RunNumber);
                                                           };

                        wierPrintList.Sort(comparison);

                        for (int i = 0; i < wierPrintList.Count; i++)
                        {
                            WierPrint wier = wierPrintList[i];

                            tw.Write("{0:00} {1,-31}", wier.Date, wier.Sct);
                            if (!wier.Sum.IsMinus)
                            {
                                tw.WriteLine("{0,12:0.00}                  {1}", wier.Sum, wier.Remark);
                            }
                            else
                            {
                                tw.WriteLine("{0,25:0.00}     {1}", -wier.Sum, wier.Remark);
                            }
                        }
                    }

                    tw.WriteLine(new string('=', 72));
                }

                tw.WriteLine("{0,-20} {1,12:0.00} {2,12:0.00} {3,12:0.00} {4,12:0.00}", "Amount", sNach, sDebt, sKred,
                             sKont);
            }
        }
    }
}