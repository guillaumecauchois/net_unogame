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
    }
}