namespace Common
{
    public class TurnResponse
    {
        private Card _card;
        private bool _uno;

        public TurnResponse()
        {
            _card = null;
            _uno = false;
        }

        public TurnResponse(Card card, bool uno)
        {
            _card = card;
            _uno = uno;
        }

        public void Uno()
        {
            _uno = true;
        }

        public void SetCard(Card card)
        {
            _card = card;
        }

        public Card GetCard()
        {
            return _card;
        }
        
        public bool HasUno()
        {
            return (_uno);
        }
    }
}