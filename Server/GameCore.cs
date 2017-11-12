using System;
using System.Threading.Tasks;
using Common;
using DotNetty.Transport.Channels;

namespace Server
{
    public class GameCore
    {
        public Table Table { get; set; }
        private bool read;
        
        public GameCore()
        {
            Table = new Table();
            read = true;
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
            Console.WriteLine(" 'list players'\t->\tDisplay all players informations");
            Console.WriteLine("ENJOY!"); 
            Console.WriteLine(""); 
        }
        
        private void ReadServerCommands()
        {
            Console.WriteLine("");
            DisplayHelpCommands();
            Console.Write("$> ");
            var line = Console.ReadLine();
            
            while (line != null && read)
            {
                ServerCommand.ExecuteCommand(line, Table, this);
                Console.Write("$> ");
                line = Console.ReadLine();
            }
        }

        public void LaunchNewGame()
        {
            Table.StartGame();
            var currentPlayer = Table.CurrentPlayer;
            
            Table.NotifyTurnToAllPlayers();
        }

        public void HandleTurnResponse(IChannelHandlerContext context, string msg)
        {
            try
            {
                var tResponse =
                    SerializeHandler.DeserializeObject<TurnResponse>(msg);

                var currentPlayer = Table.GetPlayerByContext(context);
                if (!TurnResponseHandler.Handle(tResponse, currentPlayer,
                        Table))
                {
                    Console.WriteLine("[KO] Player play an invalid card, we will inform him");
                    if (currentPlayer == Table.CurrentPlayer)
                    {
                        Console.WriteLine("[OK] Ask to player a new turn");
                    }
                }
            }
            catch (SerializeHandlerException)
            {
                var e = new Event(EventType.Error, null,
                    Table) {ErrorMsg = "The card choose is unvalid"};
                var serObj = SerializeHandler.SerializeObj(e);
                context.WriteAndFlushAsync(serObj + "\r\n");
            }
            catch (Exception)
            {
                var e = new Event(EventType.Error, null,
                    Table)
                {
                    ErrorMsg = "The game conditions don't permit you to game now, wait to be invited."
                };
                var serObj = SerializeHandler.SerializeObj(e);
                context.WriteAndFlushAsync(serObj + "\r\n");
            }
        }
    }
}