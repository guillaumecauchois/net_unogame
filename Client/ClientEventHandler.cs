using System;
using Common;

namespace Client
{
    public class ClientEventHandler
    {
        private delegate void ClientEventHandlerCmd();
        private readonly ClientEventHandlerCmd[] _events;
        private CardBeautifuler _beautifuler;

        public ClientEventHandler()
        {
            Event = null;
            _events = new ClientEventHandlerCmd[5];
            _events[0] = new ClientEventHandlerCmd(HandleEventYourTurn);
            _events[1] = new ClientEventHandlerCmd(HandleEventPlayerTurn);
            _events[2] = new ClientEventHandlerCmd(HandleEventEndGame);
            _events[3] = new ClientEventHandlerCmd(HandleEventPlayerHasPlayed);
            _events[4] = new ClientEventHandlerCmd(HandleEventInvalidCommand);
            _beautifuler = new CardBeautifuler();
        }
        
        public Event Event { get; set; }

        public void HandleEvent(Event eventReceived)
        {
            Event = eventReceived;
            _events[(int)Event.Type]();
        }

        private void HandleEventInvalidCommand()
        {
            Console.WriteLine("handle event invalid command");
        }

        private void HandleEventYourTurn()
        {
            Console.WriteLine("It's your turn to play !");
            Event.Player.Hand.DisplayHand(_beautifuler);
        }

        private void HandleEventEndGame()
        {
            Console.WriteLine("handle event end game");
        }

        private void HandleEventPlayerTurn()
        {
            Console.WriteLine("It's turn of player#" + Event.Player.Id);
        }

        private void HandleEventPlayerHasPlayed()
        {
            // plus de précisions pour ce que le joueur a joué
            Console.WriteLine("handle event has played");
        }
    }
}