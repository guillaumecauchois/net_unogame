using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace Common
{
    [ProtoContract]
    public class Hand
    {
        [ProtoMember(1)]
        private List<Card> _cards;

        public Hand()
        {
            
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
    }
}