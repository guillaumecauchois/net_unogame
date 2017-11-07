namespace Common
{
    /* Enumeration definitions */
    public enum CardColor
    {
        Undefined,
        Green,
        Red,
        Yellow,
        Blue
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
        
        public virtual void handleUse()
        {
            
        }
    }
}