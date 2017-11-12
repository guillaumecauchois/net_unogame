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

        public bool CardIsValidToBePut(Card cardToPut, Card cardTable)
        {
            if (cardToPut.Color.Equals(CardColor.Undefined) &&
                (cardToPut.Value.Equals(CardValue.Plus4) ||
                 cardToPut.Value.Equals(CardValue.ChangeColor)))
                return true;
            if (cardTable.Color == cardToPut.Color)
                return true;
            if (cardTable.Value == cardToPut.Value)
                return true;
            return cardTable.JokerColor == cardToPut.Color;
        }
        
        public bool PutCardOnTable(Table table, Card card)
        {   
            try
            {
                var player = table.Players.Find(x => x.Hand == this);
                
                if (!CardIsValidToBePut(card, table.GetTopStackCard()))
                {
                    Console.Error.WriteLine("Invalid à la pose");
                    return (false);
                }
                var cardInHand = Cards.Find(x =>
                    x.Value.Equals(card.Value) && x.Color.Equals(card.Color));
                cardInHand.HandleUse(player, table);
                if (cardInHand.Color == CardColor.Undefined)
                {
                    cardInHand.JokerColor = card.JokerColor;
                }
                Cards.Remove(cardInHand);
                table.AddCard(cardInHand);
                if (Cards.Count == 0)
                {
                    table.SetGameEnd(player);
                }
                table.TurnToNextPlayer();
                table.NotifyTurnToAllPlayers();
            }
            catch (Exception)
            {
                if (Cards.Count == 0)
                    table.SetGameEnd(null);
            }
            return true;
        }

        public void DisplayHand()
        {         
            var index = 0;
            foreach (var card in Cards)
            {
                Console.WriteLine(index + " : " + CardBeautifuler.GetStringCard(card));
                index++;
            }
        }
    }
}