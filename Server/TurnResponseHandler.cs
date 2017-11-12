using System;
using Common;

namespace Server
{
    public static class TurnResponseHandler
    {
        private static bool CheckCardPossession(Player player, Card card)
        {
            return player.Hand.Cards.Contains(card);
        }

        private static bool CheckPlayerTurn(Player player, Table table)
        {
            return (table.CurrentPlayer == player);
        }

        private static bool CheckGameIsRunning(Table table)
        {
            return (table.Status == GameStatus.Running);
        }
        
        public static int Handle(TurnResponse response, Player player, Table table)
        {
            if (!CheckCardPossession(player, response.Card) ||
                !CheckPlayerTurn(player, table) ||
                !CheckGameIsRunning(table))
            {
                return (-1);   
            }
            table.PutCardOnTable(player, response.Card);
            return (0);
        }
    }
}