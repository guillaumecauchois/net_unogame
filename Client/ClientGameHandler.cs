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
            if (args.Length != 1)
            {
                Console.WriteLine("431 ERR_NOARGUMENTSNEEDED");
                return null;
            }
            if (handler.Event.Table.Status != GameStatus.Running)
            {
                Console.WriteLine("432 ERR_NOTGAMINGTIME");
                return null;
            }
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("433 ERR_NOTYOURTURN");
                return null;
            }
            if (handler.Event.Player.Hand.Cards.Count == 2)
            {
                Console.WriteLine("200 UNO_OK");
                TurnResponse response = new TurnResponse(null, true);
                return (response);
                // TODO : Je pense qu'il faut que le joueur fasse un UNO AVANT de jouer sa carte
            }
            else
            {
                Console.WriteLine("434 ERR_INVALIDCARDSNB");
                return null;
            }
        }

        private TurnResponse HandlePlayCmd(ClientEventHandler handler, string[] args)
        {
            Console.WriteLine("handle PLAY cmd");
            if (handler.Event.Table.Status != GameStatus.Running)
            {
                Console.WriteLine("421 ERR_NOTGAMINGTIME");
                return null;
            }
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("422 ERR_NOTYOURTURN");
                return null;
            }
            if (args.Length != 2)
            {
                Console.WriteLine("423 ERR_INCOMPLETEARGUMENTS");
                return null;      
            }
            int cardIndex = int.Parse(args[1]);
            if (!(cardIndex >= 0 && cardIndex < handler.Event.Player.Hand.Cards.Count))
            {
                Console.WriteLine("424 ERR_BADINDEX");
                return null;
            }
            Console.WriteLine("200 PLAY_OK");
            TurnResponse response = new TurnResponse(handler.Event.Player.Hand.Cards[cardIndex], false);
            return (response);
        }

        private TurnResponse HandlePassCmd(ClientEventHandler handler, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("451 ERR_NOARGUMENTSNEEDED");
                return null;
            }
            if (handler.Event.Table.Status != GameStatus.Running)
            {
                Console.WriteLine("452 ERR_NOTGAMINGTIME");
                return null;
            }
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("453 ERR_NOTYOURTURN");
                return null;
            }
            if (!handler.Event.HasDraw)
            {
                Console.WriteLine("454 ERR_MUSTPLAYORDRAW");
                return null;
            }
            else
            {
                Console.WriteLine("200 PASS_OK");
                // TODO : Envoyer une requête PASS au serveur
                return null;
            }
        }

        private TurnResponse HandleHandCmd(ClientEventHandler handler, string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("411 ERR_NOARGUMENTSNEEDED");
            }
            if (handler.Event.Table.Status != GameStatus.Running)
            {
                Console.WriteLine("412 ERR_NOTGAMINGTIME");
            }
            if (handler.Event.Player.Hand.Cards.Count == 0)
            {
                Console.WriteLine("413 ERR_EMPTYHAND");
            }
            else
            {
                Console.WriteLine("200 HAND_OK:");
                handler.Event.Player.Hand.DisplayHand(beautiful);
            }
            return null;
        }

        private TurnResponse HandleDrawCmd(ClientEventHandler handler, string[] args)
        {
            Console.WriteLine("handle DRAW cmd");
            if (args.Length != 1)
            {
                Console.WriteLine("461 ERR_NOARGUMENTSNEEDED");
                return null;
            }
            if (handler.Event.Table.Status != GameStatus.Running)
            {
                Console.WriteLine("462 ERR_NOTGAMINGTIME");
                return null;
            }
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("463 ERR_NOTYOURTURN");
                return null;
            }
            if (handler.Event.HasDraw)
            {
                Console.WriteLine("464 ERR_ALREADYDRAW");
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