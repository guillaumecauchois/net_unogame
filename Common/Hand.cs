using System;
using System.Collections.Generic;
using System.Linq;
using ProtoBuf;

namespace Common
{
    [ProtoContract]
    public class Hand
    {
        [ProtoMember(1)] public List<Card> Cards;

        public Hand()
        {
            Cards = new List<Card>();
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public void PopCard(Card card)
        {
            Cards.Remove(card);
        }

        public bool CardIsValidToPut(Card cardToPut, Card cardTable)
        {
            if (cardTable.Color == cardToPut.Color)
            {
                return true;
            }
            else if (cardTable.Value == cardToPut.Value)
            {
                return true;
            }
            return false;
        }
        
        public bool PutCardOnTable(Table table, Card card)
        {   
            try
            {
                var player = table.Players.Find(x => x.Hand == this);
                if (!Cards.Contains(card))
                    return (false);
                if (!CardIsValidToPut(card, table.GetTopStackCard()))
                    return (false);
                card.HandleUse(player);
                Cards.Remove(card);
                table.AddCard(card);
                if (Cards.Count == 0)
                {
                    table.SetGameEnd(player);
                }
                table.CurrentPlayer = table.GetNextPlayer();
            }
            catch (Exception)
            {
                if (Cards.Count == 0)
                    table.SetGameEnd(null);
            }
            return true;
        }

        public void DisplayHand(CardBeautifuler beautifuler)
        {
            Console.WriteLine("Your cards :");

            var index = 0;
            foreach (var card in Cards)
            {
                Console.WriteLine(index + " : " + CardBeautifuler.GetStringCard(card));
                index++;
            }
        }
    }
}