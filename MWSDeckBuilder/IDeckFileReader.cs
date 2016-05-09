using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWSDeckBuilder
{
    interface IDeckFileReader
    {
        void Open(StreamReader stream, List<MagicCardBase> cardSet);
    }
}
