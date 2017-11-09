using System;
using ProtoBuf;

namespace Common
{
    /* Enumeration definitions */
    public enum CardColor
    {
        Undefined = -1,
        MinDefinedColor = 0,
        Green = 0,
        Red,
        Yellow,
        Blue,
        MaxDefinedColor = Blue,
    }

    public enum CardValue
    {
        Undefined,
        Zero,
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
        Plus4
    }
    
    /* Class Card */
    [ProtoContract]
    public class Card
    {
        /* Attributs */
        [ProtoMember(1)]
        private CardColor _color;
        [ProtoMember(2)]
        private CardValue _value;

        public Card(CardColor color, CardValue value)
        {
            this._color = color;
            this._value = value;
        }
        
        public virtual void HandleUse()
        {
            
        }
    }
}