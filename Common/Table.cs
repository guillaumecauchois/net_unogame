using System;
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
        private StackCard     _history;
        private StackCard     _stackCard;
        private GameStatus    _status;
        private Player        _winner;
        private List<Player>  _players;

        public Table()
        {
            _history = new StackCard();
            _stackCard = new StackCard();
            _status = GameStatus.NotStarted;
            _winner = null;
            _players = new List<Player>();
        }
        
        public bool PutCardOnTable(Player player, Card card)
        {
            return (player.GetHand().PutCardOnTable(this, card));
        }

        public void AddCard(Card card)
        {
            _history.AddCard(card);
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public void StartGame()
        {
            _history.Clear();
            _winner = null;
        }
        
        public void SetGameEnd(Player winner)
        {
            _winner = winner;
            _status = GameStatus.End;
        }

        public void DistributeCardToPlayers()
        {
            if (!_status.Equals(GameStatus.NotStarted))
                throw new Exception("The game must be not started for distribute cards.");
            foreach (var player in _players)
            {
                player.GetHand().AddCard(_stackCard.PopRandomCard());
            }
        }
    }
}