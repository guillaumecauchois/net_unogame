using System.Collections;
using System.Dynamic;
using Common;

namespace Server
{
    public class CardFactory
    {
        private ArrayList _cards;

        public CardFactory()
        {
            _cards = new ArrayList();
        }

        public void GenerateDeck()
        {
            for (int i = 1; i < (int) Common.CardColor.Blue + 1; i++)
            {
                _cards.Add(new Common.Card((Common.CardColor)i, Common.CardValue.Zero));
                for (int y = 2; y < (int) Common.CardValue.ChangeColor; y++)
                {
                    _cards.Add(new Common.Card((Common.CardColor)i, (Common.CardValue)y));  
                    _cards.Add(new Common.Card((Common.CardColor)i, (Common.CardValue)y));
                }
            }
            for (int i = 0; i < 4; i++)
            {
                _cards.Add(new Common.Card(Common.CardColor.Undefined, Common.CardValue.Plus4));
                _cards.Add(new Common.Card(Common.CardColor.Undefined, Common.CardValue.ChangeColor));
            }
        }
    }
}