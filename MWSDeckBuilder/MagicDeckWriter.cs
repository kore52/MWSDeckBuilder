using System;
using System.IO;
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

        public void SaveDeckFile(string filename)
        {
            if (filename == "")
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.Filter = "MWS Format|*.mwDeck|MO Format|*.txt|";
                bool? result = saveFileDialog.ShowDialog();
                if (result == true)
                {
                    string newFileName = saveFileDialog.SafeFileName;
                    using (Stream fileStream = saveFileDialog.OpenFile())
                    using (StreamWriter sr = new StreamWriter(fileStream))
                    {
                        WriteFile(sr);
                    }
                }
            }
        }
    }
}
