using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Common
{
    [ProtoContract]
    public class StackCard
    {
        [ProtoMember(1)]
        private List<Card> _stack;

        public StackCard()
        {
            _stack = new List<Card>();
        }

        public void AddCard(Card card)
        {
            _stack.Add(card);
        }

        public Card GetRandomCard()
        {
            // Get Stack Lenght
            var lenght = _stack.Count;
            if (lenght == 0)
                throw new Exception("StackCard is empty");

            // Generate the index of the random card
            var random = new Random();
            var idxRandom = random.Next(0, lenght + 1);
            
            // Send the random card
            return (_stack.ToArray()[idxRandom]);
        }

        public Card GetTop()
        {
            return _stack.GetRange(1, 1)[0];
        }
        
        public Card PopRandomCard()
        {
            Card card = GetRandomCard();

            _stack.Remove(card);
            return (card);
        }

        public void GenerateDeck()
        {
            for (var color = CardColor.MinDefinedColor; color <= CardColor.MaxDefinedColor; color++)
            {
                AddCard(new Card(color, CardValue.Zero));
                for (var value = CardValue.One; value < CardValue.ChangeColor; value++)
                {
                    AddCard((new Card(color, value)));  
                    AddCard(new Card(color, value));
                }
            }
            for (var i = 0; i < 4; i++)
            {
                AddCard(new Card(CardColor.Undefined, CardValue.Plus4));
                AddCard(new Card(CardColor.Undefined, CardValue.ChangeColor));
            }
        }

        public void Clear()
        {
            _stack.Clear();
        }
    }
}