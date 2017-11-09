using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace Common
{
    public enum EventType
    {
        InvalidCommand = -1,
        YouTurn = 0,
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
            Type = EventType.InvalidCommand;
        }
        
        public Event(string errorMsg)
        {
            ErrorMsg = errorMsg;
            Type = EventType.InvalidCommand;
        }

        public Event(EventType type, Player player = null, Table table = null)
        {
            Type = type;
            Player = player;
            Table = table;
            ErrorMsg = null;
        }
        
        [ProtoMember(1)]
        private Player Player { get; set; }
        [ProtoMember(2)]
        public EventType Type { get; set; }
        [ProtoMember(3)]
        public string ErrorMsg { get; set; }
        [ProtoMember(4)]
        private Table Table { get; set; }
        
    }
}