using System;
using System.Collections.Generic;
using Common;

namespace Server
{
    public static class TurnResponseHandler
    {
        private static bool CheckCardPossession(Player player, Card card)
        {
            return (true);
        }

        private static bool CheckPlayerTurn(Player player, Table table)
        {
            return (true);
        }

        private static bool CheckGameIsRunning(Table table)
        {
            return (table.Status == GameStatus.Running);
        }
        
        public static int Handle(TurnResponse response, Player player, Table table)
        {
            if (!CheckCardPossession(player, response.Card) ||
                !CheckPlayerTurn(player, table))
                return (-1);
            Console.WriteLine("{0} {1}", response.Card.Color, response.Card.Value);
            return (0);
        }
    }
}