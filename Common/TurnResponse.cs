using ProtoBuf;

namespace Common
{
    [ProtoContract]
    public class TurnResponse
    {
        public enum TurnType
        {
            Play,
            Pass,
            Draw,
            Uno
        }
        
        [ProtoMember(1)] public Card Card { get; set; }
        [ProtoMember(2)] public bool Uno { get; set; }
        [ProtoMember(3)] public TurnType Type { get; set; }
        [ProtoMember(4)] public CardColor JokerColor { get; set; }

        public TurnResponse()
        {
            Card = null;
            Uno = false;
            Type = TurnType.Play;
            JokerColor = CardColor.Undefined;
        }
        
        public TurnResponse(Card card, bool uno, TurnType type, CardColor color)
        {
            Card = card;
            Uno = uno;
            Type = type;
            JokerColor = color;
        }
    }
}