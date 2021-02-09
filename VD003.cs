using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    internal static class VD003
    {
        public static void Run(Dictionary<string, SctMoving> movingList)
        {
            using (StreamWriter tw = new StreamWriter("vd003.txt", false, Encoding.UTF8))
            {
                List<string> sctNames = new List<string>(movingList.Keys);
                sctNames.Sort();

                foreach (string sctName in sctNames)
                {
                    SctReport(sctName, tw, movingList[sctName]);
                }
            }
        }

        private static void SctReport(string sctName, TextWriter tw, SctMoving sctMoving)
        {
            Sum amount = sctMoving.SNach;

            tw.WriteLine("<><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>");
            tw.WriteLine();
            tw.WriteLine("{0,-52} {1,20:0.00}", sctName, amount);

            List<WirePrint> wiers = new List<WirePrint>();

            foreach(Wire wier in sctMoving.DebList)
            {
                WirePrint wirePrint = new WirePrint(wier.Date, wier.KrdSct, wier.Sum, wier.Remark, wier.RunNumber);
                wiers.Add(wirePrint);
            }

            foreach(Wire wier in sctMoving.KrdList)
            {
                WirePrint wirePrint = new WirePrint(wier.Date, wier.DebSct, -wier.Sum, wier.Remark, wier.RunNumber);
                wiers.Add(wirePrint);
            }

            if (wiers.Count == 0)
            {
                return;
            }

            tw.WriteLine("--------------------------------------------------------------------------");

            Comparison<WirePrint> comparison = delegate(WirePrint p1, WirePrint p2)
            {
                int tmp = p1.Date.CompareTo(p2.Date);
                return tmp != 0 ? tmp : p1.RunNumber.CompareTo(p2.RunNumber);
            };
            wiers.Sort(comparison);

            foreach(WirePrint wier in wiers)
            {
                amount += wier.Sum;

                tw.Write("{0:00} {1,-31}", wier.Date, wier.Sct);
                if(!wier.Sum.IsMinus)
                {
                    tw.WriteLine("{0,12:0.00}  {1,25:0.00}  {2}", wier.Sum, amount, wier.Remark);
                }
                else
                {
                    tw.WriteLine("{0,25:0.00}  {1,12:0.00}  {2}", -wier.Sum, amount, wier.Remark);
                }
            }
        }
    }
}