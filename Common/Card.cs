namespace Common
{
    /* Enumeration definitions */
    public enum CardColor
    {
        MinDefinedColor = 0,
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
        MaxDefinedValue = Plus4,
    }
    
    /* Class Card */
    public class Card
    {
        /* Attributs */
        private CardColor _color;
        private CardValue _value;

        public Card(CardColor color, CardValue value)
        {
            this._color = color;
            this._value = value;
        }
        
        public virtual void HandleUse()
        {
            
        }

        public CardColor GetCardColor()
        {
            return _color;
        }

        public CardValue GetCardValue()
        {
            return _value;
        }
    }
}