using System;
using System.Threading;
using System.Threading.Tasks;
using Common;

namespace Server
{
    public class GameCore
    {
        public Table Table { get; set; }
        
        public GameCore()
        {
            Table = new Table();
        }

        public void RunContainers()
        {
            var taskRunServerCommands = new Task(ReadServerCommands);
            taskRunServerCommands.Start();
            
            try
            {
                taskRunServerCommands.Wait();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        private static void DisplayHelpCommands()
        {
            Console.WriteLine("You can use these commands for admin. your UNO-Game:");
            Console.WriteLine(" 'start'\t->\tStart a new game (2 <= player <= 10)");
            Console.WriteLine(" 'reset'\t->\tReset gameplay of an ended game");
            Console.WriteLine("ENJOY!");
            Console.WriteLine("");
            
        }
        
        private void ReadServerCommands()
        {
            Console.WriteLine("");
            DisplayHelpCommands();
            Console.Write("$> ");
            var line = Console.ReadLine();
            while (line != null)
            {
                /**
                 * TODO: Transform this part in Array Membe fuctions
                 */
                if (line.Equals("start"))
                {
                    if (Table.Status.Equals(GameStatus.NotStarted))
                    {
                        var taskGame = new Task(LaunchNewGame);
                        taskGame.Start();
                        try
                        {
                            taskGame.Wait();
                        }
                        catch (Exception e)
                        {
                            Console.Error.WriteLine(e.Message);
                        }
                    }
                    else
                        Console.Error.WriteLine("[ERR] Sorry, the game is still running");
                }
                else if (line.Equals("reset"))
                {
                    if (!Table.Status.Equals(GameStatus.Running))
                    {
                        Table.ResetGamePlay();
                        Console.WriteLine("[OK] Gameplay has been reset, you can now use 'start' for run new game");
                    }
                    else
                    {
                        Console.Error.WriteLine("[KO] Gameplay cannot be reset during game");
                    }
                }
                /**
                 *   ^^^^^^^^^^^^^^^^^^
                 */
                Console.Write("$> ");
                line = Console.ReadLine();
            }
        }
        
        private void LaunchNewGame()
        {
            Table.StartGame();
            var currentPlayer = Table.CurrentPlayer;
            
            while (Table.Status.Equals(GameStatus.Running))
            {
                /* Notify current player that is now his turn */
                var e = new Event(EventType.YourTurn, currentPlayer, Table);
                var serObj = SerializeHandler.SerializeObj(e);
                currentPlayer.Context.WriteAndFlushAsync(serObj);
                
                /* Notify other players that is turn of "current_player" */
                e = new Event(EventType.PlayerTurn, currentPlayer, Table);
                Table.SendObjectToOtherPlayers(e, currentPlayer);
                
                /* TODO: Determine and of the game */
                Table.SetGameEnd(currentPlayer);
            }
        }
    }
}