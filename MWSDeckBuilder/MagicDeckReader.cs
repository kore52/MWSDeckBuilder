using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MWSDeckBuilder
{
    public class MagicDeckReader
    {
        private List<MagicCardBase> cardSet;

        public MagicDeckReader(List<MagicCardBase> cardSet)
        {
            this.cardSet = cardSet;
        }

        public Tuple<ObservableCollection<MagicDeckCard>, ObservableCollection<MagicDeckCard>> OpenDeckFile(string filename)
        {
            using (var stream = new StreamReader(new FileStream(filename, FileMode.Open)))
            {
                var extension = Path.GetExtension(filename).ToLower();
                switch (extension)
                {
                    case "txt":
                        var moreader = new MODeckReader(cardSet);
                        moreader.Open(stream);
                        return new Tuple<ObservableCollection<MagicDeckCard>, ObservableCollection<MagicDeckCard>>(moreader.Mainboard, moreader.Sideboard);
                    case "mwdeck":
                        var mwsreader = new MWSDeckReader(cardSet);
                        mwsreader.Open(stream);
                        return new Tuple<ObservableCollection<MagicDeckCard>, ObservableCollection<MagicDeckCard>>(mwsreader.Mainboard, mwsreader.Sideboard);
                    default:
                        throw new Exception("Invalid Deck Format");
                }
            }
        }

    }
}
