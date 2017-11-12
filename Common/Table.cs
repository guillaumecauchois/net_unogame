using System;
using System.Collections.Generic;
using System.Linq;
using DotNetty.Transport.Channels;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Common
{
    public enum GameStatus
    {
        NotStarted,
        Running,
        End
    }
    
    [ProtoContract]
    public class Table
    {
        [ProtoMember(1, IsRequired = true)] private StackCard  History;
        [ProtoMember(2, IsRequired = true)] private StackCard  StackCard;
        [ProtoMember(3)] public GameStatus                     Status;
        [ProtoMember(4)] private Player                        Winner { get; set; }
        [ProtoMember(5)] public List<Player>                   Players;
        [ProtoMember(6)] public Player                         CurrentPlayer { set; get; }

        public Table()
        {
            Status = GameStatus.NotStarted;
            History = new StackCard();
            StackCard = new StackCard();
            Winner = null;
            Players = new List<Player>();
        }
        
        public bool PutCardOnTable(Player player, Card card)
        {
            return (player.Hand.PutCardOnTable(this, card));
        }

        public void AddCard(Card card)
        {
            History.AddCard(card);
        }

        public void AddPlayer(Player player)
        {
            if (Status == GameStatus.NotStarted)
                Players.Add(player);
            else
                throw new Exception("Sorry but the game is already started ...");
        }

        public Player GetNextPlayer()
        {
            return Players.Find(x => x == CurrentPlayer);
        }

        public bool IsValidGame()
        {
            return (Players.Count >= 2);
        }
        
        public void RemovePlayer(Player player)
        {
            
            foreach (var card in player.Hand.Cards)
            {
                StackCard.AddCard(card);
            }
            player.Hand.Cards.RemoveAll(x => true); 
            Console.WriteLine("\n[INFO] The player {0} left the game.", player.Id);
            Console.Write("$> ");
            Players.Remove(player);
            if (Status != GameStatus.Running) return;
            if (CurrentPlayer == player)
            {
                try
                {
                    CurrentPlayer = GetNextPlayer();
                }
                catch (ArgumentNullException)
                {
                    SetGameEnd(null);
                }
            }
            if (!IsValidGame())
                SetGameEnd(null);
        }
        
        public void StartGame()
        {
            if (Players.Count < 2 || Players.Count > 10)
                throw new Exception("Invalid team size");
            ResetGamePlay();
            StackCard.GenerateDeck();
            DistributeCardToPlayers();
            History.AddCard(StackCard.PopRandomCard());
            Status = GameStatus.Running;
            CurrentPlayer = Players.First();
        }
        
        public void SetGameEnd(Player winner)
        {
            Winner = winner;
            Status = GameStatus.End;
        }

        public void ResetGamePlay()
        {
            History.Clear();
            StackCard.Clear();
            Winner = null;
            CurrentPlayer = null;
            Status = GameStatus.NotStarted;
        }

        private void DistributeCardToPlayers()
        {
            if (!Status.Equals(GameStatus.NotStarted))
                throw new Exception("The game must be not started for distribute cards.");
            foreach (var player in Players)
            {
                for (var i = 0; i != 7; ++i)
                    player.Hand.AddCard(StackCard.PopRandomCard());
            }
        }

        public Card GetTopStackCard()
        {
            return History.GetLastCard();
        }

        public void SendObjectToOtherPlayers<T>(T obj, Player currentPlayer)
        {
            var serObj = SerializeHandler.SerializeObj(obj);

            foreach (var player in Players)
            {
                if (currentPlayer != player)
                {
                    player.Context.WriteAndFlushAsync(serObj + "\r\n");
                }
            }
        }

        public Player GetPlayerByContext(IChannelHandlerContext context)
        {
            foreach (var player in Players)
            {
                if (player.Context == context)
                    return player;
            }
            throw new Exception("Cannot find player associate to TurnResponse");
        }
    }
}