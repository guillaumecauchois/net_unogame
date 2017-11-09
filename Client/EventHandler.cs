using System;
using Common;

namespace SecureChat.Client
{
    public class EventHandler
    {
        private Event _event;
        private delegate void EventHandlerCmd();
        private readonly EventHandlerCmd[] _events;
        private CardBeautifuler _beautifuler;

        public EventHandler()
        {
            _events = new EventHandlerCmd[5];
            _events[0] = new EventHandlerCmd(HandleEventYourTurn);
            _events[1] = new EventHandlerCmd(HandleEventPlayerTurn);
            _events[2] = new EventHandlerCmd(HandleEventEndGame);
            _events[3] = new EventHandlerCmd(HandleEventPlayerHasPlayed);
            _events[4] = new EventHandlerCmd(HandleEventInvalidCommand);
            _beautifuler = new CardBeautifuler();
        }

        public void HandleEvent(Event eventReceived)
        {
            _event = eventReceived;
            _events[(int)_event.Type]();
        }

        private void HandleEventInvalidCommand()
        {
            Console.WriteLine("handle event invalid command");
        }

        private void HandleEventYourTurn()
        {
            Console.WriteLine("It's your turn to play !");
            _event.Player.Hand.DisplayHand(_beautifuler);
        }

        private void HandleEventEndGame()
        {
            Console.WriteLine("handle event end game");
        }

        private void HandleEventPlayerTurn()
        {
            Console.WriteLine("It's turn of player#" + _event.Player.Id);
        }

        private void HandleEventPlayerHasPlayed()
        {
            // plus de précisions pour ce que le joueur a joué
            Console.WriteLine("handle event has played");
        }
    }
}