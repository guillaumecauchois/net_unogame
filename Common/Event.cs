using ProtoBuf;

namespace Common
{
    public enum EventType
    {
        Error = 0,
        StartGame,
        YourTurn,
        PlayerTurn,
        PlayerHasPlayed,
        EndGame,
    }
    
    [ProtoContract]
    public class Event
    {
        public Event()
        {
            Player = null;
            Table = null;
            ErrorMsg = null;
            Type = EventType.Error;
        }
        
        public Event(string errorMsg)
        {
            ErrorMsg = errorMsg;
            Type = EventType.Error;
        }

        public Event(EventType type, Player player = null, Table table = null)
        {
            Type = type;
            Player = player;
            Table = table;
            ErrorMsg = null;
        }
        
        [ProtoMember(1)]
        public Player Player { get; set; }
        [ProtoMember(2)]
        public EventType Type { get; set; }
        [ProtoMember(3)]
        public string ErrorMsg { get; set; }
        [ProtoMember(4)]
        public Table Table { get; set; }        
    }
}