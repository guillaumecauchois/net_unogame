using System;
using Common;

namespace Client
{
    public class ClientGameHandler
    {
        private delegate void ClientCmdHandler(ClientEventHandler handler, string[] args);
        private readonly ClientCmdHandler[] _cmdsFunctions;
        private CardBeautifuler beautiful;

        private string[] _cmdsNames;
        
        public ClientGameHandler()
        {       
            _cmdsFunctions = new ClientCmdHandler[5];
            _cmdsFunctions[0] = new ClientCmdHandler(HandlePlayCmd);
            _cmdsFunctions[1] = new ClientCmdHandler(HandleDrawCmd);
            _cmdsNames = new string[] {"Play", "Draw"};
        }

        public void HandleClientCmd(ClientEventHandler handler, string line)
        {
            string[] args = line.Split(' ');
            for (var i = 0; i < 2; i++)
            {
                if (args[0] == _cmdsNames[i])
                {
                    _cmdsFunctions[i](handler, args);
               }
            }
        }

        public void HandlePlayCmd(ClientEventHandler handler, string[] args)
        {
            Console.WriteLine("handle PLAY cmd");
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("NOT YOUR TURN");
                return;
            }
        }
        
        public void HandleDrawCmd(ClientEventHandler handler, string[] args)
        {
            Console.WriteLine("handle DRAW cmd");
        }
        
    }
}