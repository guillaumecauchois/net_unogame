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
            _cmdsFunctions = new ClientCmdHandler[5];
            _cmdsFunctions[0] = HandlePlayCmd;
            _cmdsFunctions[1] = HandleDrawCmd;
            _cmdsFunctions[2] = HandleUnoCmd;
            _cmdsFunctions[3] = HandlePassCmd;
            _cmdsFunctions[4] = HandleHandCmd;
            _cmdsNames = new[] {"Play", "Draw", "Uno", "Pass", "Hand"};
            beautiful = new CardBeautifuler();
        }

        public TurnResponse HandleClientCmd(ClientEventHandler handler, string line)
        {
            var args = line.Split(' ');
            for (var i = 0; i < 2; i++)
            {
                if (args[0] == _cmdsNames[i])
                {
                    return (_cmdsFunctions[i](handler, args));
                }
            }
            return null;
        }

        private static TurnResponse HandleUnoCmd(ClientEventHandler handler, string[] args)
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
                return new TurnResponse(null, true, TurnResponse.TurnType.Uno, CardColor.Undefined);
                // TODO : Je pense qu'il faut que le joueur fasse un UNO AVANT de jouer sa carte
            }
            else
            {
                Console.WriteLine("434 ERR_INVALIDCARDSNB");
                return null;
            }
        }

        private CardColor GetInputCardColor(string color)
        {
            switch (color)
            {
                case "Green":
                    return CardColor.Green;
                case "Red":
                    return CardColor.Red;
                case "Yellow":
                    return CardColor.Yellow;
                case "Blue":
                    return CardColor.Blue;
                default:
                    return CardColor.Undefined;
            }
        }

        private TurnResponse PlayJokerCard(ClientEventHandler handler, string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("423 ERR_INCOMPLETEARGUMENTS");
                return null;      
            }
            CardColor color = GetInputCardColor(args[2]);
            if (color == CardColor.Undefined)
            {
                Console.WriteLine("425 ERR_UNDEFINEDCOLOR");
                return null;
            }
            else
            {
                Console.WriteLine("200 PLAY_OK");
                return new TurnResponse(handler.Event.Player.Hand.Cards[int.Parse(args[1])], false, TurnResponse.TurnType.Play, color);
            }
        }
        
        private TurnResponse PlayBasicCard(ClientEventHandler handler, string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("423 ERR_INCOMPLETEARGUMENTS");
                return null;      
            }
            Console.WriteLine("200 PLAY_OK");
            return new TurnResponse(handler.Event.Player.Hand.Cards[int.Parse(args[1])], false, TurnResponse.TurnType.Play, CardColor.Undefined);
            
        }

        private TurnResponse HandlePlayCmd(ClientEventHandler handler, string[] args)
        {
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
            if (args.Length < 2)
            {
                Console.WriteLine("423 ERR_INCOMPLETEARGUMENTS");
                return null;      
            }
            var cardIndex = int.Parse(args[1]);
            if (!(cardIndex >= 0 && cardIndex < handler.Event.Player.Hand.Cards.Count))
            {
                Console.WriteLine("424 ERR_BADINDEX");
                return null;
            }
            CardValue value = handler.Event.Player.Hand.Cards[cardIndex].Value;
            if (value == CardValue.ChangeColor || value == CardValue.Plus4)
            {
                return PlayJokerCard(handler, args);
            }
            else
            {
                return PlayBasicCard(handler, args);
            }
        }

        private static TurnResponse HandlePassCmd(ClientEventHandler handler, string[] args)
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
                return new TurnResponse(null, false, TurnResponse.TurnType.Pass, CardColor.Undefined);
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

        private static TurnResponse HandleDrawCmd(ClientEventHandler handler, string[] args)
        {
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
            return new TurnResponse(null, false, TurnResponse.TurnType.Draw, CardColor.Undefined);
        }
        
    }
}