using System;
using System.Collections.Generic;

namespace Common
{
    public class StackCard
    {
        private List<Card> _stack;
        
        public StackCard()
        {
            
        }

        public void AddCard(Card card)
        {
            this._stack.Add(card);
        }

        public Card GetRandomCard()
        {
            // Get Stack Lenght
            var lenght = this._stack.Count;
            if (lenght == 0)
                throw new Exception("StackCard is empty");

            // Generate the index of the random card
            Random random = new Random();
            var idxRandom = random.Next(0, lenght + 1);
            
            // Send the random card
            return (this._stack.ToArray()[idxRandom]);
        }

        public void GenerateDeck()
        {
            for (int i = 1; i < (int) Common.CardColor.Blue + 1; i++)
            {
                AddCard(new Common.Card((Common.CardColor)i, Common.CardValue.Zero));
                for (int y = 2; y < (int) Common.CardValue.ChangeColor; y++)
                {
                    AddCard((new Common.Card((Common.CardColor)i, (Common.CardValue)y)));  
                    AddCard(new Common.Card((Common.CardColor)i, (Common.CardValue)y));
                }
            }
            for (int i = 0; i < 4; i++)
            {
                AddCard(new Common.Card(Common.CardColor.Undefined, Common.CardValue.Plus4));
                AddCard(new Common.Card(Common.CardColor.Undefined, Common.CardValue.ChangeColor));
            }
        }
    }
}