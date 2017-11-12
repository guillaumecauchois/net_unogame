using System;
using System.Collections.Generic;
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
            beautiful = new CardBeautifuler();
        }

        public TurnResponse HandleClientCmd(ClientEventHandler handler, string line)
        {
            try
            {
                var args = line.Split(' ');
                var handlers =
                    new Dictionary<string, Func<ClientEventHandler, string[],
                        TurnResponse>>
                    {
                        {"Pass", HandlePassCmd},
                        {"Draw", HandleDrawCmd},
                        {"Play", HandlePlayCmd},
                        {"Uno", HandleUnoCmd}
                    };
                return handlers[args[0]](handler, args);
            }
            catch
            {
                Console.WriteLine("[IGN] Unknowned command");
            }
            return null;
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
            var color = GetInputCardColor(args[2]);
            if (color == CardColor.Undefined)
            {
                Console.WriteLine("425 ERR_UNDEFINEDCOLOR");
                return null;
            }
            else
            {
                Console.WriteLine("200 PLAY_OK");
                handler.Event.Player.Hand.Cards[int.Parse(args[1])].JokerColor =
                    color;
                return new TurnResponse(handler.Event.Player.Hand.Cards[int.Parse(args[1])], TurnResponse.TurnType.Play);
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
            return new TurnResponse(handler.Event.Player.Hand.Cards[int.Parse(args[1])], TurnResponse.TurnType.Play);
            
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
            var value = handler.Event.Player.Hand.Cards[cardIndex].Value;
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
            else
            {
                Console.WriteLine("200 PASS_OK");
                return new TurnResponse(null, TurnResponse.TurnType.Pass);
            }
        }
        
        private static TurnResponse HandleUnoCmd(ClientEventHandler handler, string[] args)
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
            Console.WriteLine("200 UNO_OK");
            return new TurnResponse(null, TurnResponse.TurnType.Uno);
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
            Console.WriteLine("200 DRAW_OK:");
            return new TurnResponse(null, TurnResponse.TurnType.Draw);
        }
    }
}