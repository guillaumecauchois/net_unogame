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
            
            /* Notify current player that is now his turn */
            var e = new Event(EventType.YourTurn, currentPlayer, Table);
            var serObj = SerializeHandler.SerializeObj(e);
            /* var eClone = SerializeHandler.DeserializeObject<Event>(serObj);
            var cb = new CardBeautifuler();
            eClone.Player.Hand.DisplayHand(cb); */
            currentPlayer.Context.WriteAndFlushAsync(serObj + "\r\n");
                
            /* Notify other players that is turn of "current_player" */
            e = new Event(EventType.PlayerTurn, currentPlayer, Table);
            Table.SendObjectToOtherPlayers(e, currentPlayer);
        }

        public void HandleTurnResponse(IChannelHandlerContext context, string msg)
        {
            try
            {
                var tResponse =
                    SerializeHandler.DeserializeObject<TurnResponse>(msg); 
                
                var currentPlayer = Table.GetPlayerByContext(context);
                if (TurnResponseHandler.Handle(tResponse, currentPlayer, Table) == -1)
                {
                    Console.Error.WriteLine("[ERR] Receive information from unknowned player");
                }
            }
            catch (SerializeHandlerException)
            {
                Console.Error.WriteLine("[ERR] Receive invalid information from a client");
            }
        }
    }
}