using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DotNetty.Transport.Channels;
using ProtoBuf;

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
        [ProtoMember(3)] public GameStatus                     Status = GameStatus.NotStarted;
        [ProtoMember(4)] private Player                        Winner { get; set; }
        [ProtoMember(6)] public Player                         CurrentPlayer { set; get; }
        [ProtoMember(5)] public List<Player>                   Players;

        public Table()
        {
            History = new StackCard();
            StackCard = new StackCard();
            Status = GameStatus.NotStarted;
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

        public void StartGame()
        {
            if (Players.Count < 2 || Players.Count > 10)
                throw new Exception("Invalid team size");
            ResetGamePlay();
            StackCard.GenerateDeck();
            DistributeCardToPlayers();
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
            return History.GetTop();
        }

        public void SendObjectToOtherPlayers<T>(T obj, Player currentPlayer)
        {
            var serObj = SerializeHandler.SerializeObj(obj);

            foreach (var player in Players)
            {
                if (currentPlayer != player)
                {
                    player.Context.WriteAndFlushAsync(serObj);
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