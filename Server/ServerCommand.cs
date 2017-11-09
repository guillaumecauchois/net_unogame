using System;
using System.Collections.Generic;
using Common;

namespace Server
{
    public static class ServerCommand
    {
        public static int ExectureCommand(string commandName,  Table table, GameCore core)
        {
            var dictionary =
                new Dictionary<string, Func<Table, GameCore, int>>
                {
                    {"start", ServerCommandStart},
                    {"reset", ServerCommandReset},
                    {"list players", ServerCommandListPlayers}
                };

            if (dictionary.ContainsKey(commandName))
            {
                try
                {
                    return dictionary[commandName].Invoke(table, core);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("[ERR] " + e.Message);
                    return (-1);
                }
            }
            Console.Error.WriteLine("[IGN] Unknowned command");
            return (-1);
        }
        
        private static int ServerCommandStart(Table table, GameCore core)
        {
            if (table.Status.Equals(GameStatus.NotStarted))
                core.LaunchNewGame();
            else
                Console.Error.WriteLine("[ERR] Sorry, the game is still running");
            return 0;
        }

        private static int ServerCommandReset(Table table, GameCore core)
        {
            if (!table.Status.Equals(GameStatus.Running))
            {
                table.ResetGamePlay();
                Console.WriteLine("[OK] Gameplay has been reset, you can now use 'start' for run new game");
            }
            else
                Console.Error.WriteLine("[ERR] Gameplay cannot be reset during game");
            return 0;
        }

        private static int ServerCommandListPlayers(Table table, GameCore core)
        {
            var listPlayers = table.Players;

            if (listPlayers.Count == 0)
            {
                Console.WriteLine("No player connected");
            }
            foreach (var player in listPlayers)
            {
                Console.WriteLine("{0}", player.ToString());
            }
            return (0);
        }
    }
}