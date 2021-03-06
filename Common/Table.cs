﻿using System;
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
        [ProtoMember(2, IsRequired = true)] public StackCard  StackCard;
        [ProtoMember(3)] public GameStatus                     Status;
        [ProtoMember(4)] public Player                         Winner { get; set; }
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
            var idx = Players.IndexOf(CurrentPlayer);
            idx++;
            if (idx >= Players.Count)
                idx = 0;
            return Players[idx];
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
            try
            {
                var e = new Event(EventType.StartGame, CurrentPlayer, this);
                var serObj = SerializeHandler.SerializeObj(e);
                foreach (var player in Players)
                {
                    player.Context.WriteAndFlushAsync(serObj + "\r\n");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
        
        public void SetGameEnd(Player winner)
        {
            if (winner != null && !winner.HasUno)
            {
                var card = StackCard.PopRandomCard();
                winner.Hand.AddCard(card);
                card = StackCard.PopRandomCard();
                winner.Hand.AddCard(card);
                NotifyTurnToAllPlayers();
                return;
            }
            Winner = winner;
            foreach (var player in Players)
            {
                try
                {
                    var eventEnd = new Event(EventType.EndGame, winner, this);
                    var serObj = SerializeHandler.SerializeObj(eventEnd);
                    player.Context.WriteAndFlushAsync(serObj + "\r\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            if (winner != null)
                Console.WriteLine("[OK] End of the game the winner is : {0}!\n$> ", winner.Id);
            else
                Console.WriteLine("[OK] End of the game, there is no winner\n$> ");
            Status = GameStatus.End;
        }

        public void NotifyYourTurnToCurrentPlayer()
        {
            /* Notify current player that is now his turn */
            var e = new Event(EventType.YourTurn, CurrentPlayer, this);
            var serObj = SerializeHandler.SerializeObj(e);
            CurrentPlayer.Context.WriteAndFlushAsync(serObj + "\r\n");
        }
        
        public void NotifyTurnToAllPlayers()
        {
            /* Notify current player that is now his turn */
            NotifyYourTurnToCurrentPlayer();
                
            /* Notify other players that is turn of "current_player" */
            var e = new Event(EventType.PlayerTurn, CurrentPlayer, this);
            SendObjectToOtherPlayers(e, CurrentPlayer);
        }

        public void NotifyHasPlayerHasPlayed(Player player)
        {
            var e = new Event(EventType.PlayerHasPlayed, player, this);
            var serObj = SerializeHandler.SerializeObj(e);
            
            // Cannot use SendObjectToOtherPlayers() because use CurrentPlayer
            // but it's changed
            foreach (var uplayer in Players)
            {
                if (uplayer != player)
                {
                    uplayer.Context.WriteAndFlushAsync(serObj);
                }
            }
        }
        
        public void TurnToNextPlayer()
        {
            CurrentPlayer = GetNextPlayer();
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