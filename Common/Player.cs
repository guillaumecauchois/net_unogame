namespace Common
{
    public class Player
    {
        private static int      _idGenerator = 0;
        private readonly int    _id;
        private string          _name;
        private Hand            _hand;

        public Player(string name)
        {
            this._id = _idGenerator;
            this._name = name;
            this._hand = new Hand();
            _idGenerator++;
        }

        public void SetName(string name)
        {
            this._name = name;
        }

        public string GetName()
        {
            return (this._name);
        }

        public int GetId()
        {
            return (this._id);
        }

        public Hand GetHand()
        {
            return (this._hand);
        }
    }
}