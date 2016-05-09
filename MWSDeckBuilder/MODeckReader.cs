using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MWSDeckBuilder
{
    public class MODeckReader : DeckFileReader
    {
        public ObservableCollection<MagicDeckCard> Mainboard { get; private set; } = new ObservableCollection<MagicDeckCard>();
        public ObservableCollection<MagicDeckCard> Sideboard { get; private set; } = new ObservableCollection<MagicDeckCard>();

        public MODeckReader(List<MagicCardBase> cardSet) : base(cardSet) { }

        public void Open(StreamReader stream)
        {
            try
            {
                Mainboard = null;
                Sideboard = null;

                bool isSideboard = false;
                while (true)
                {
                    var line = stream.ReadLine();
                    if (line == null) break;
                    if (line == "") isSideboard = true;

                    var regex = new Regex(@"([0-9]+)\s(.+)");
                    foreach (Match match in regex.Matches(line))
                    {
                        int amount = int.Parse(match.Groups[1].Value);
                        string name = match.Groups[2].Value;

                        var src = cardSet.Where(x => x.Name == name).Distinct().First();
                        MagicDeckCard card = new MagicDeckCard(src);
                        card.Amount = amount;

                        if (!isSideboard)
                            Mainboard.Add(card);
                        else
                            Sideboard.Add(card);
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
