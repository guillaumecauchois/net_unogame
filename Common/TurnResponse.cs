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
        [ProtoMember(3)] public TurnType turn_type { get; set; }

        public TurnResponse(Card card, bool uno, TurnType type)
        {
            Card = card;
            Uno = uno;
            turn_type = type;
        }
    }
}