using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    internal static class VD002
    {
        public static void Run(Dictionary<string, SctMoving> movingList)
        {
            using (StreamWriter tw = new StreamWriter("vd002.txt", false, Encoding.UTF8))
            {
                Sum sNach = new Sum();
                Sum sDebt = new Sum();
                Sum sKred = new Sum();
                Sum sKont = new Sum();

                List<string> sctNames = new List<string>(movingList.Keys);
                sctNames.Sort();

                foreach (string key in sctNames)
                {
                    SctMoving moving = movingList[key];

                    tw.WriteLine("{0,-20} {1,12:0.00} {2,12:0.00} {3,12:0.00} {4,12:0.00}", key, moving.SNach,
                                 moving.SDebt, moving.SKred, moving.SKont);

                    sNach += moving.SNach;
                    sDebt += moving.SDebt;
                    sKred += moving.SKred;
                    sKont += moving.SKont;

                    List<WirePrint> wierPrintList = new List<WirePrint>();
                    foreach (Wire wier in moving.DebList)
                    {
                        wierPrintList.Add(new WirePrint(wier.Date, wier.KrdSct, wier.Sum, wier.Remark, wier.RunNumber));
                    }
                    foreach (Wire wier in moving.KrdList)
                    {
                        wierPrintList.Add(new WirePrint(wier.Date, wier.DebSct, -wier.Sum, wier.Remark, wier.RunNumber));
                    }

                    if (wierPrintList.Count > 0)
                    {
                        tw.WriteLine(new string('-', 72));

                        Comparison<WirePrint> comparison = delegate(WirePrint w1, WirePrint w2)
                                                           {
                                                               int compare = w1.Date.CompareTo(w2.Date);
                                                               return compare != 0 ? compare : w1.RunNumber.CompareTo(w2.RunNumber);
                                                           };

                        wierPrintList.Sort(comparison);

                        for (int i = 0; i < wierPrintList.Count; i++)
                        {
                            WirePrint wire = wierPrintList[i];

                            tw.Write("{0:00} {1,-31}", wire.Date, wire.Sct);
                            if (!wire.Sum.IsMinus)
                            {
                                tw.WriteLine("{0,12:0.00}                  {1}", wire.Sum, wire.Remark);
                            }
                            else
                            {
                                tw.WriteLine("{0,25:0.00}     {1}", -wire.Sum, wire.Remark);
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