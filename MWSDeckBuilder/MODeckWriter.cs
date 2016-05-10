using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace MWSDeckBuilder
{
    internal class MODeckWriter
    {
        private List<MagicCardBase> cardSet;
        private StreamWriter sr;

        public MODeckWriter(StreamWriter sr, List<MagicCardBase> cardSet)
        {
            this.sr = sr;
            this.cardSet = cardSet;
        }

        internal void WriteFile(Tuple<ObservableCollection<MagicDeckCard>, ObservableCollection<MagicDeckCard>> deck)
        {
            var mainboard = deck.Item1;
            var sideboard = deck.Item2;

            foreach (var card in mainboard)
            {
                sr.WriteLine($"{card.Amount} {card.Name}");
            }
            sr.WriteLine($"");
            sr.WriteLine($"");
            foreach (var card in sideboard)
            {
                sr.WriteLine($"{card.Amount} {card.Name}");
            }
        }
    }
}