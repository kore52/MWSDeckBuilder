using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWSDeckBuilder
{
    public abstract class DeckFileReader
    {
        protected List<MagicCardBase> cardSet;

        public DeckFileReader(List<MagicCardBase> cardSet)
        {
            this.cardSet = cardSet;
        }

        protected void AddTo(ObservableCollection<MagicDeckCard> deck, int amount, string name, string edition = "")
        {
            var card = cardSet.Where(x => x.Name == name && x.Edition == edition).FirstOrDefault();
            if (card == null)
            {
                card = cardSet.Where(x => x.Name == name).First();
                // if it doesn't find card, then throws exception.
            }
            var cardInDeck = new MagicDeckCard(card);
            cardInDeck.Amount = amount;
            deck.Add(cardInDeck);
        }
    }
}
