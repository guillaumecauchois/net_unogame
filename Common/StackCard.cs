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
            
        }
    }
}