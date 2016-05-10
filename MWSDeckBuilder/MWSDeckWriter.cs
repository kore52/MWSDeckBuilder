using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace MWSDeckBuilder
{
    internal class MWSDeckWriter
    {
        private List<MagicCardBase> cardSet;
        private StreamWriter sr;

        public MWSDeckWriter(StreamWriter sr, List<MagicCardBase> cardSet)
        {
            this.sr = sr;
            this.cardSet = cardSet;
        }

        internal void WriteFile(Tuple<ObservableCollection<MagicDeckCard>, ObservableCollection<MagicDeckCard>> deck)
        {
            throw new NotImplementedException();
        }
    }
}