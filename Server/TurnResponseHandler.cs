using System;
using System.Collections.Generic;
using Common;
using Microsoft.Extensions.DependencyInjection;

namespace Server
{
    public static class TurnResponseHandler
    {
        private static bool CheckCardPossession(Player player, Card card, Table table)
        {
            var ret = player.Hand.Cards.Exists(x =>
                ((x.Color.Equals(card.Color) || x.Color.Equals(card.JokerColor))) &&
                x.Value.Equals(card.Value));
            return ret;
        }

        private static bool CheckPlayerTurn(Player player, Table table)
        {
            var ret = table.CurrentPlayer == player;
            return ret;
        }

        private static bool CheckGameIsRunning(Table table)
        {
            var ret = table.Status == GameStatus.Running;
            return (ret);
        }

        private static bool HandlePlayResponse(TurnResponse response, Player player,
            Table table)
        {
            if (!CheckCardPossession(player, response.Card, table) ||
                !CheckPlayerTurn(player, table) ||
                !CheckGameIsRunning(table))
            {
                player.SendError("Received invalid informations, try again");
                table.NotifyYourTurnToCurrentPlayer();
            }
            if (table.PutCardOnTable(player, response.Card))
            {
                player.HasDraw = false;
                return true;
            }
            player.SendError("The card cannot be put on the table, check and try again");
            table.NotifyYourTurnToCurrentPlayer();
            return (false);

        }

        public static bool HandleDrawResponse(TurnResponse response,
            Player player,
            Table table)
        {
            if (!player.HasDraw)
            {
                var card = table.StackCard.PopRandomCard();
                player.Hand.AddCard(card);
                player.HasDraw = true;
                table.NotifyYourTurnToCurrentPlayer();
                return true;
            }
            player.SendError("You cannot draw more than once");
            table.NotifyYourTurnToCurrentPlayer();
            return false;
        }
        
        public static bool HandlePassResponse(TurnResponse response,
            Player player,
            Table table)
        {
            if (player.HasDraw)
            {
                table.TurnToNextPlayer();
                table.NotifyTurnToAllPlayers();
                player.HasDraw = false;
                return (true);
            }
            player.SendError("You need to play or draw a card");
            table.NotifyYourTurnToCurrentPlayer();
            return (false);
        }
        
        public static bool HandleUnoResponse(TurnResponse response,
            Player player,
            Table table)
        {
            if (table.CurrentPlayer != player)
            {
                player.SendError("It's not your turn, please wait.");
                return (false);
            }
            if (player.Hand.Cards.Count != 2)
            {
                player.SendError("Sorry, but you UNO wrong ! :(");
                for (var i = 0; i != 2; i++)
                {
                    var card = table.StackCard.PopRandomCard();
                    player.Hand.AddCard(card);
                }
                table.NotifyYourTurnToCurrentPlayer();
                return (false);
            }
            player.HasUno = true;
            return (true);
        }
        
        public static bool Handle(TurnResponse response, Player player, Table table)
        {
            try
            {
                var handlers =
                    new Dictionary<TurnResponse.TurnType,
                        Func<TurnResponse, Player, Table, bool>>
                    {
                        {TurnResponse.TurnType.Play, HandlePlayResponse},
                        {TurnResponse.TurnType.Draw, HandleDrawResponse},
                        {TurnResponse.TurnType.Pass, HandlePassResponse},
                        {TurnResponse.TurnType.Uno, HandleUnoResponse}
                    };

                return handlers[response.Type](response, player, table);
            }
            catch (Exception e)
            {
                player.SendError("Cannot translate you reponse, please try again.");
                Console.Error.WriteLine($"[ERR] {e.Message}");
            }
            return (false);
        }
    }
}