using System;
using System.Collections.Generic;
using Common;

namespace Client
{
    public class ClientEventHandler
    {
        private delegate void ClientEventHandlerCmd();
        private readonly ClientEventHandlerCmd[] _events;
        private CardBeautifuler _beautifuler;
        public Event Event { get; set; }

        public ClientEventHandler()
        {
            Event = null;
            _beautifuler = new CardBeautifuler();
            var _events = new Dictionary<EventType, ClientEventHandlerCmd>
            {
                {EventType.YourTurn, HandleEventYourTurn},
                {EventType.PlayerTurn, HandleEventPlayerTurn},
                {EventType.EndGame, HandleEventEndGame},
                {EventType.PlayerHasPlayed, HandleEventPlayerHasPlayed},
                {EventType.Error, HandleEventInvalidCommand}
            };
        }
        

        public void HandleEvent(Event eventReceived)
        {
            Event = eventReceived;
            Console.WriteLine("TableType : {0} - HasDraw : {1} - pId : {2}", eventReceived.Table.Status, eventReceived.HasDraw, eventReceived.Player.Id);
            try
            {
                _events[(int) eventReceived.Type].Invoke();
            }
            catch (Exception e)
            {
                Console.Error.Write("[ERR] Receive invalid Event Type : " + eventReceived.Type);
            }
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