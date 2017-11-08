using System;

namespace Common
{
    public enum EventType
    {
        InvalidCommand = -1,
        YourTurn = 0,
        PlayerTurn,
        PlayerHasPlayed,
        EndGame,
    }
    
    public class Event
    {
        private Table        _table;
        private Player       _player;
        private EventType    _type;
        private bool         _hasDraw;

        public Event(EventType type, Table table, Player player)
        {
            _player = player;
            _table = table;
            _type = type;
            _hasDraw = false;
        }

        public void SetTable(Table table)
        {
            _table = table;
        }

        public Table GetTable()
        {
            return _table;
        }

        public void SetPlayer(Player player)
        {
            _player = player;
        }

        public Player GetPlayer()
        {
            return _player;
        }

        public void SetType(EventType type)
        {
            _type = type;
        }

        public EventType GetEventType()
        {
            return _type;
        }

        public bool GetHasDraw()
        {
            return _hasDraw;
        }

        public void SetHasDraw(bool state)
        {
            _hasDraw = state;
        }
    }
}