namespace Common
{
    /* Enumeration definitions */
    public enum CardColor
    {
        Undefined,
        Green,
        Red,
        Yellow
    }

    public enum CardValue
    {
        Undefined,
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
        Plus4,
        Revert,
        PassTurn,
        ChangeLog
    }
    
    /* Class Card */
    public abstract class Card
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