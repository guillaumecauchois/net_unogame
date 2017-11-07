using System.Collections.Generic;

namespace Common
{
    enum GameStatus
    {
        NotStarted,
        Running,
        End
    }
    
    public class Table
    {
        private Stack<Card>   _history;
        private StackCard     _stackCard;
        private GameStatus    _status;
        private Player        _winner;
        private List<Player>  _players;

        public bool PutCardOnTable(Player player, Card card)
        {
            return (player.GetHand().PutCardOnTable(this, card));
        }

        public void AddCard(Card card)
        {
            this._history.Push(card);
        }
    }
}