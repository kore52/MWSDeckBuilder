using System;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWSDeckBuilder
{
    public class MWSDeckReader : DeckFileReader
    {
        public ObservableCollection<MagicDeckCard> Mainboard { get; private set; } = new ObservableCollection<MagicDeckCard>();
        public ObservableCollection<MagicDeckCard> Sideboard { get; private set; } = new ObservableCollection<MagicDeckCard>();

        public MWSDeckReader(List<MagicCardBase> cardSet) : base(cardSet) { }

        public void Open(StreamReader stream)
        {
            try
            {
                while (true)
                {
                    bool isSideboard = false;
                    var line = stream.ReadLine();
                    if (line == null) return;

                    var regComment = new Regex("^//.+");
                    var comment = regComment.Matches(line);
                    if (regComment.IsMatch(line)) continue;

                    var regSB = new Regex("SB:\\s+([0-9]+)\\s+(.+)");
                    var ms = regSB.Matches(line);
                    if (regSB.IsMatch(line))
                    {
                        isSideboard = true;
                        line = ms[0].Groups[1].Value + " " + ms[0].Groups[2].Value;
                    }

                    var regMain = new Regex("([0-9]+)\\s+\\[(.+)\\]\\s+(.+)");
                    var mm = regMain.Matches(line);
                    if (isSideboard)
                    {
                        Console.WriteLine("[SB] {0} {1} {2}", mm[0].Groups[1].Value, mm[0].Groups[2].Value, mm[0].Groups[3].Value);
                        var amount = int.Parse(mm[0].Groups[1].Value);
                        var edition = mm[0].Groups[2].Value;
                        var name = mm[0].Groups[3].Value;

                        AddTo(Mainboard, amount, edition, name);
                    }
                    else
                    {
                        Console.WriteLine("{0} {1} {2}", mm[0].Groups[1].Value, mm[0].Groups[2].Value, mm[0].Groups[3].Value);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
