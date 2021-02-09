using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Buchalter.Types;

namespace Buchalter
{
    internal static class Tools
    {
        private class WireDelimiter : IWire
        {
            public readonly string Remark;

            public WireDelimiter(string remark)
            {
                Remark = remark;
            }
        }

        public static List<WiresFile> LoadWiresFiles()
        {
            string[] files = Directory.GetFiles(".", Program.WiersFileMask);

            List<WiresFile> list = new List<WiresFile>();

            foreach (string file in files)
            {
                list.Add(new WiresFile(file));                
            }

            return list;
        }

        public static List<IWire> LoadWires(string wiresFileName)
        {
            List<IWire> retValue = new List<IWire>();

            string[] strings = File.ReadAllLines(wiresFileName, Encoding.UTF8);
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
                        retValue.Add(new WireDelimiter(line));
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

                    Wire wire = new Wire(date, debSct, krdSct, summa, remark, i);

                    retValue.Add(wire);
                }
                catch (Exception e)
                {
                    throw new Exception("Exception occure at " + i + " line", e);
                }
            }

            return retValue;
        }

        public static Dictionary<string, SctMoving> GetMovingList(IEnumerable<Amount> amounts, IEnumerable<WiresFile> wireFiles)
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

            foreach (WiresFile file in wireFiles)
            {
                foreach (Wire wier in file.Wires.Where(wier => wier != null && wier.GetType() == typeof(Wire)).Select(wier => (Wire)wier))
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

        public static void RewriteWiresFiles(List<WiresFile> wireFiles)
        {
            foreach (WiresFile wierFile in wireFiles)
            {
                string bakFileName = MakeBakFileName(wierFile.FileName);
                File.Delete(bakFileName);
                File.Move(wierFile.FileName, bakFileName);

                var wiers = wierFile.Wires;
                string[] lines = new string[wiers.Count];

                for (int i = 0; i < wiers.Count; i++)
                {
                    IWire wireTmp = wiers[i];
                    if (wireTmp == null)
                    {
                        lines[i] = string.Empty;
                        continue;
                    }

                    if (wireTmp.GetType() == typeof(WireDelimiter))
                    {
                        lines[i] = ((WireDelimiter)wireTmp).Remark;
                        continue;
                    }

                    Wire wire = (Wire) wireTmp;
                    //lines[i] = $"{wire.Date:00} ! {wire.DebSct,-20} ! {wire.KrdSct,-20} ! {wire.Sum,12:0.00} ! {wire.Remark}";
                    lines[i] = string.Format("{0:00} ! {1,-20} ! {2,-20} ! {3,12:0.00} ! {4}", wire.Date, wire.DebSct,
                        wire.KrdSct, wire.Sum, wire.Remark);
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