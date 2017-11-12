using System;
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
            Uno,
        }
        
        [ProtoMember(1)] public Card Card { get; set; }
        [ProtoMember(2)] public TurnType Type { get; set; }

        public TurnResponse()
        {
            Card = null;
            Type = TurnType.Play;
        }
        
        public TurnResponse(Card card, TurnType type)
        {
            Card = card;
            Type = type;
        }
    }
}