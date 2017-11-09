using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace Common
{
    [ProtoContract]
    public class Hand
    {
        [ProtoMember(1)] public List<Card> cards;

        public Hand()
        {
            cards = new List<Card>();   
        }

        public void AddCard(Card card)
        {
            cards.Add(card);
        }
        
        public bool PutCardOnTable(Table table, Card card)
        {
            card.HandleUse();
            if (!this.cards.Contains(card))
                return (false);
            this.cards.Remove(card);
            table.AddCard(card);
            return (true);
        }

        public void DisplayHand(CardBeautifuler beautifuler)
        {
            Console.WriteLine("Your cards :");
            foreach (var card in cards)
            {
                Console.WriteLine(beautifuler.GetStringCard(card));
            }
        }
    }
}