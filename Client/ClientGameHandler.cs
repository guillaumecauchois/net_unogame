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

        public void HandleUnoCmd(ClientEventHandler handler, string[] args)
        {
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("NOT YOUR TURN");
                return;
            }
            if (args.Length != 1)
            {
                Console.WriteLine("No arguments needed");
                return;
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
            if (args.Length != 2)
            {
                Console.WriteLine("You need to pick one card");
                return;
            }
            int cardIndex = int.Parse(args[1]);
            if (!(cardIndex >= 0 && cardIndex < handler.Event.Player.Hand.Cards.Count))
            {
                Console.WriteLine("Incorrect index");
                return;
            }
            var card = handler.Event.Player.Hand.Cards[cardIndex];
            var LastCard = handler.Event.Table.GetTopStackCard();
            if (card.Color == LastCard.Color || card.Value == LastCard.Value)
            {
                // SEND TURN RESPONSE
            }
            else
            {
                Console.WriteLine("You can't play this card");
            }
        }
        
        public void HandleDrawCmd(ClientEventHandler handler, string[] args)
        {
            Console.WriteLine("handle DRAW cmd");
            if (handler.Event.Type != EventType.YourTurn)
            {
                Console.WriteLine("NOT YOUR TURN");
                return;
            }
            if (args.Length != 1)
            {
                Console.WriteLine("No arguments needed");
                return;
            }
            if (handler.Event.HasDraw)
            {
                Console.WriteLine("You can't draw another card");
                return;
            }
            else
            {
                // SEND TURN RESPONSE
            }
        }
        
    }
}