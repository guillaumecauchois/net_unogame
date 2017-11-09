using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Common
{
    public class Hand
    {
        private List<Card> _cards;

        public Hand()
        {
            _cards = new List<Card>();   
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }
        
        public bool PutCardOnTable(Table table, Card card)
        {
            card.HandleUse();
            if (!this._cards.Contains(card))
                return (false);
            this._cards.Remove(card);
            table.AddCard(card);
            return (true);
        }

        public void DisplayHand(CardBeautifuler beautifuler)
        {
            Console.WriteLine("Your cards :");
            var index = 1;
            foreach (var card in _cards)
            {
                Console.WriteLine(index + " : " + beautifuler.GetStringCard(card));
                index++;
            }
        }
    }
}