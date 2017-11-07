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
    
    public class Event
    {
        private Table        _table;
        private Player       _player;
        private EventType    _type;

        public Event(EventType type, Table table, Player player)
        {
            _player = player;
            _table = table;
            _type = type;
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
    }
}