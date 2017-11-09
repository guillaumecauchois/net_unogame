namespace Common
{
    public class TurnResponse
    {
        public Card Card { get; set; }
        public bool Uno { get; set; }

        public TurnResponse(Card card, bool uno)
        {
            Card = card;
            Uno = uno;
        }
    }
}