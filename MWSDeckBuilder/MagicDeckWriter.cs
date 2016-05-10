using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace MWSDeckBuilder
{
    class MagicDeckWriter
    {
        private List<MagicCardBase> cardSet;

        public MagicDeckWriter(List<MagicCardBase> cardSet)
        {
            this.cardSet = cardSet;
        }

        public void SaveDeckFile(string filename, Tuple<ObservableCollection<MagicDeckCard>, ObservableCollection<MagicDeckCard>> deck)
        {
            if (filename != "")
            {
                using (StreamWriter sr = new StreamWriter(new FileStream(filename, FileMode.Create, FileAccess.Write)))
                {
                    var extension = Path.GetExtension(filename).ToLower();
                    switch (extension)
                    {
                        case ".txt":
                            var mowriter = new MODeckWriter(sr, cardSet);
                            mowriter.WriteFile(deck);
                            break;
                        case ".mwdeck":
                            var mwswriter = new MWSDeckWriter(sr, cardSet);
                            mwswriter.WriteFile(deck);
                            break;
                    }
                    sr.Flush();
                }
            }
        }
    }
}
