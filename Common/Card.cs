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
        [ProtoMember(1)]
        public CardColor Color { get; set; }
        [ProtoMember(2)]
        public CardValue Value { get; set; }

        public Card()
        {
            Color = CardColor.Undefined;
            Value = CardValue.Undefined;
        }
        
        public Card(CardColor color, CardValue value)
        {
            Color = color;
            Value = value;
        }

        public virtual void HandleUse(Player player)
        {
            Console.WriteLine("[OK] Player {0} put on table a {1} card",
                player.Id, CardBeautifuler.GetStringCard(this));
        }
    }
}