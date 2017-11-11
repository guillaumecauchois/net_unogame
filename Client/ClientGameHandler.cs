using System;
using System.Runtime.Remoting.Contexts;
using Common;

namespace Client
{
    public class ClientGameHandler
    {
        private delegate TurnResponse ClientCmdHandler(ClientEventHandler handler, string[] args);
        private readonly ClientCmdHandler[] _cmdsFunctions;
        private CardBeautifuler beautiful;

        private string[] _cmdsNames;
        
        public ClientGameHandler()
        {       
            _cmdsFunctions = new ClientCmdHandler[4];
            _cmdsFunctions[0] = new ClientCmdHandler(HandlePlayCmd);
            _cmdsFunctions[1] = new ClientCmdHandler(HandleDrawCmd);
            _cmdsFunctions[2] = new ClientCmdHandler(HandleUnoCmd);
            _cmdsFunctions[3] = new ClientCmdHandler(HandlePassCmd);
            _cmdsNames = new string[] {"Play", "Draw", "Uno", "Pass"};
        }

        public TurnResponse HandleClientCmd(ClientEventHandler handler, string line)
        {
            string[] args = line.Split(' ');
            for (var i = 0; i < 2; i++)
            {
                if (args[0] == _cmdsNames[i])
                {
                    return (_cmdsFunctions[i](handler, args));
               }
            }
            return null;
        }

        private TurnResponse HandleUnoCmd(ClientEventHandler handler, string[] args)
        {
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("NOT YOUR TURN");
                return null;
            }
            if (args.Length != 1)
            {
                Console.WriteLine("No arguments needed");
                return null;
            }
            if (handler.Event.Player.Hand.Cards.Count == 2)
            {
                TurnResponse response = new TurnResponse(null, true);
                return (response);
                // TODO : Je pense qu'il faut que le joueur fasse un UNO AVANT de jouer sa carte
            }
            else
            {
                Console.WriteLine("You must have 2 cards left to call a Uno");
                return null;
            }
        }

        private TurnResponse HandlePlayCmd(ClientEventHandler handler, string[] args)
        {
            Console.WriteLine("handle PLAY cmd");
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("NOT YOUR TURN");
                return null;
            }
            if (args.Length != 2)
            {
                Console.WriteLine("You need to pick one card");
                return null;
            }
            int cardIndex = int.Parse(args[1]);
            if (!(cardIndex >= 0 && cardIndex < handler.Event.Player.Hand.Cards.Count))
            {
                Console.WriteLine("Incorrect index");
                return null;
            }
            TurnResponse response = new TurnResponse(handler.Event.Player.Hand.Cards[cardIndex], false);
            return (response);
        }

        private TurnResponse HandlePassCmd(ClientEventHandler handler, string[] args)
        {
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("NOT YOUR TURN");
                return null;
            }
            if (args.Length != 1)
            {
                Console.WriteLine("No arguments needed");
                return null;
            }
            if (!handler.Event.HasDraw)
            {
                Console.WriteLine("You must play or draw");
                return null;
            }
            else
            {
                // TODO : Envoyer une requête PASS au serveur
                return null;
            }
        }

        private TurnResponse HandleHandCmd(ClientEventHandler handler, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("No arguments needed");
            }
            if (handler.Event.Table.Status != GameStatus.Running)
            {
                Console.WriteLine("There is no game running");
            }
            else
            {
                handler.Event.Player.Hand.DisplayHand(beautiful);
            }
            return null;
        }

        private TurnResponse HandleDrawCmd(ClientEventHandler handler, string[] args)
        {
            Console.WriteLine("handle DRAW cmd");
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("NOT YOUR TURN");
                return null;
            }
            if (args.Length != 1)
            {
                Console.WriteLine("No arguments needed");
                return null;
            }
            if (handler.Event.HasDraw)
            {
                Console.WriteLine("You can't draw another card");
                return null;
            }
            else
            {
                // TODO : Envoyer au serveur une requête de pioche
                return null;
            }
        }
        
    }
}