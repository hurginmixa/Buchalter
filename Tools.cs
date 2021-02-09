using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    static class Tools
    {
        private class WierDelimiter : IWier
        {
            public readonly string Remark;

            public WierDelimiter(string remark)
            {
                Remark = remark;
            }
        }

        public static List<WiersFile> LoadWiersFiles()
        {
            string[] files = Directory.GetFiles(".", Program.WiersFileMask);

            List<WiersFile> list = new List<WiersFile>();

            foreach (string file in files)
            {
                list.Add(new WiersFile(file));                
            }

            return list;
        }

        public static List<IWier> LoadWiers(string wiersFileName)
        {
            List<IWier> retValue = new List<IWier>();

            string[] strings = File.ReadAllLines(wiersFileName, Encoding.UTF8);
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
                        retValue.Add(new WierDelimiter(line));
                        continue;
                    }

                    ParseString.Token[] tokens = ParseString.Parse(line, '!', '|');
                    if (tokens.Length < 4)
                    {
                        throw new Exception("Invalid field number");
                    }

                    Date date = Date.Parse(tokens[0].Text.Trim());
                    string debSct = tokens[1].Text.Trim();
                    string krdSct = tokens[2].Text.Trim();
                    Sum summa = Sum.Parse(tokens[3].Text.Trim().Replace(',', '.'));
                    string remark = (tokens.Length >= 5) ? tokens[4].Text.Trim() : String.Empty;

                    Wier wier = new Wier(date, debSct, krdSct, summa, remark, i);

                    retValue.Add(wier);
                }
                catch (Exception e)
                {
                    throw new Exception("Exception occure at " + i + " line", e);
                }
            }

            return retValue;
        }

        public static Dictionary<string, SctMoving> GetMovings(IList<Amount> amounts, IList<WiersFile> wierFiles)
        {
            Dictionary<string, SctMoving> movings = new Dictionary<string, SctMoving>();

            foreach (Amount amount in amounts)
            {
                SctMoving sctMovie;
                if (!movings.TryGetValue(amount.Sct, out sctMovie))
                {
                    sctMovie = new SctMoving();
                    movings[amount.Sct] = sctMovie;
                }

                sctMovie.SNach += amount.Sum;
            }

            foreach (WiersFile file in wierFiles)
            {
                foreach (Wier wier in file.Wiers.Where(wier => wier != null && wier.GetType() == typeof(Wier)).Select(wier => (Wier)wier))
                {
                    SctMoving sctMovie;
                    if (!movings.TryGetValue(wier.DebSct, out sctMovie))
                    {
                        sctMovie = new SctMoving();
                        movings[wier.DebSct] = sctMovie;
                    }
                    sctMovie.DebList.Add(wier);

                    if (!movings.TryGetValue(wier.KrdSct, out sctMovie))
                    {
                        sctMovie = new SctMoving();
                        movings[wier.KrdSct] = sctMovie;
                    }
                    sctMovie.KrdList.Add(wier);
                }
            }

            return movings;
        }

        public static void RewriteWiersFiles(List<WiersFile> wierFiles)
        {
            foreach (WiersFile wierFile in wierFiles)
            {
                string bakFileName = MakeBakFileName(wierFile.FileName);
                File.Delete(bakFileName);
                File.Move(wierFile.FileName, bakFileName);

                var wiers = wierFile.Wiers;
                string[] lines = new string[wiers.Count];

                for (int i = 0; i < wiers.Count; i++)
                {
                    IWier wierTmp = wiers[i];
                    if (wierTmp == null)
                    {
                        lines[i] = string.Empty;
                        continue;
                    }

                    if (wierTmp.GetType() == typeof(WierDelimiter))
                    {
                        lines[i] = ((WierDelimiter)wierTmp).Remark;
                        continue;
                    }

                    Wier wier = (Wier) wierTmp;
                    //lines[i] = $"{wier.Date:00} ! {wier.DebSct,-20} ! {wier.KrdSct,-20} ! {wier.Sum,12:0.00} ! {wier.Remark}";
                    lines[i] = string.Format("{0:00} ! {1,-20} ! {2,-20} ! {3,12:0.00} ! {4}", wier.Date, wier.DebSct,
                        wier.KrdSct, wier.Sum, wier.Remark);
                }

                File.WriteAllLines(wierFile.FileName, lines, Encoding.UTF8);
            }
        }

        private static string MakeBakFileName(string srcFileName)
        {
            string directoryName = Path.GetDirectoryName(srcFileName);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(srcFileName);

            return Path.Combine(directoryName, fileNameWithoutExtension) + ".bak";
        }
    }
}