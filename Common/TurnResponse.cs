namespace Common
{
    public class TurnResponse
    {
        public enum TurnType
        {
            Play,
            Pass,
            Draw,
            Uno
        }
        
        public Card Card { get; set; }
        public bool Uno { get; set; }
        public TurnType turn_type { get; set; }

        public TurnResponse(Card card, bool uno, TurnType type)
        {
            Card = card;
            Uno = uno;
            turn_type = type;
        }
    }
}