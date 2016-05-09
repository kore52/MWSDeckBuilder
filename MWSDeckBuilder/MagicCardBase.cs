using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CsvHelper;
using CsvHelper.Configuration;

namespace MWSDeckBuilder
{
    public class MagicCardBase
    {
        public MagicCardBase() { }
        public MagicCardBase(MagicCardBase c)
        {
            No = c.No;
            Name = c.Name;
            Type = c.Type;
            PT = c.PT;
            Mana = c.Mana;
            Rarity = c.Rarity;
            Artist = c.Artist;
            Edition = c.Edition;
            Effect = c.Effect;
            Flavor = c.Flavor;
        }

        public string No { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string PT { get; set; }
        public string Mana { get; set; }
        public string Rarity { get; set; }
        public string Artist { get; set; }
        public string Edition { get; set; }
        public string Effect { get; set; }
        public string Flavor { get; set; }
    }

    public class MagicDeckCard : MagicCardBase
    {
        public MagicDeckCard() { }
        public MagicDeckCard(MagicCardBase card) : base(card) { }
        public int Amount { get; set; } = 0;
    }

    public sealed class MagicCardClassMap : CsvClassMap<MagicCardBase>
    {
        public MagicCardClassMap()
        {
            Map(m => m.Edition).Index(0);
            Map(m => m.No).Index(1);
            Map(m => m.Name).Index(2);
            Map(m => m.Type).Index(3);
            Map(m => m.PT).Index(4);
            Map(m => m.Mana).Index(5);
            Map(m => m.Rarity).Index(6);
            Map(m => m.Artist).Index(7);
            Map(m => m.Effect).Index(8);
            Map(m => m.Flavor).Index(9);
        }
    }
}
