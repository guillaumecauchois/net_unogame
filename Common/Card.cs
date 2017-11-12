using System;
using ProtoBuf;

namespace Common
{
    /* Enumeration definitions */
    public enum CardColor
    {
        MinDefinedColor = Green,
        Undefined = 0,
        Green,
        Red,
        Yellow,
        Blue,
        MaxDefinedColor = Blue,
    }

    public enum CardValue
    {
        Undefined = -1,
        MinDefinedValue = 0,
        Zero = 0,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Plus2,
        Revert,
        PassTurn,
        ChangeColor,
        Plus4,
        MaxDefinedValue = PassTurn,
    }
    
    /* Class Card */
    [ProtoContract]
    public class Card
    {
        /* Attributs */
        [ProtoMember(1)] public CardColor Color { get; set; }
        [ProtoMember(2)] public CardValue Value { get; set; }
        [ProtoMember(3)] public CardColor JokerColor { get; set; }

        public Card()
        {
            Color = CardColor.Undefined;
            Value = CardValue.Undefined;
            JokerColor = CardColor.Undefined;
        }


        public Card(CardColor color, CardValue value)
        {
            Color = color;
            Value = value;
            JokerColor = CardColor.Undefined;
        }
        
        public void HandleUse(Player player, Table table)
        {
            Console.WriteLine("[OK] Player {0} put on table a {1} card",
                player.Id, CardBeautifuler.GetStringCard(this));

            switch (Value)
            {
                case CardValue.ChangeColor:
                    break;
                case CardValue.PassTurn:
                    table.TurnToNextPlayer();
                    break;
                case CardValue.Revert:
                    table.Players.Reverse();
                    break;
                case CardValue.Plus2:
                case CardValue.Plus4:
                    var nb = (Value == CardValue.Plus2) ? 2 : 4;
                    var next = table.GetNextPlayer();
                    for (var i = 0; i != nb; ++i)
                    {
                        var card = table.StackCard.PopRandomCard();
                        next.Hand.AddCard(card);
                    }
                    player.HasUno = false;
                    break;
            }
        }
    }
}