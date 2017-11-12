using System;
using System.Collections.Generic;
using Common;
using NUnit.Framework.Internal;

namespace Client
{
    public class ClientEventHandler
    {
        public Event Event { get; set; }
        
        public ClientEventHandler(Event e)
        {
            Event = e;
        }
        
        public ClientEventHandler() {}

        public void HandleEvent(Event eventReceived)
        {
            Event = eventReceived;
            var events = new Dictionary<EventType, Func<Event, bool>>
            {
                {EventType.YourTurn, HandleEventYourTurn},
                {EventType.PlayerTurn, HandleEventPlayerTurn},
                {EventType.EndGame, HandleEventEndGame},
                {EventType.PlayerHasPlayed, HandleEventPlayerHasPlayed},
                {EventType.Error, HandleEventInvalidCommand},
                {EventType.StartGame, HandleEventStartGame}
            };
            try
            {
                events[eventReceived.Type].Invoke(eventReceived);
            }
            catch (Exception e)
            {
                Console.Error.Write("[ERR] Receive invalid Event Type : " + eventReceived.Type);
            }
        }

        private static bool HandleEventStartGame(Event eventReceived)
        {
            Console.WriteLine("[OK] New Game starting !");
            return (true);
        }

        private static bool HandleEventInvalidCommand(Event eventReceived)
        {
            Console.WriteLine($"[ERR] An error occured, server say \"{eventReceived.ErrorMsg}\"");
            return true;
        }

        private static bool HandleEventYourTurn(Event eventReceived)
        {
            Console.WriteLine("It's your turn to play !");
            Console.WriteLine("The card on table is : " +
                          CardBeautifuler.GetStringCard(eventReceived.Table.GetTopStackCard()));
            eventReceived.Player.Hand.DisplayHand();
            Console.WriteLine("");
            Console.Write("$> ");
            return true;
        }

        private static bool HandleEventEndGame(Event eventReceived)
        {
            if (eventReceived.Table.Winner != null)
                Console.WriteLine("End of the game ! Winner is player#" + eventReceived.Table.Winner);
            else
                Console.WriteLine("End of the game ! Lot of people win the game...");
            return true;
        }

        private static bool HandleEventPlayerTurn(Event eventReceived)
        {
            Console.WriteLine("It's turn of player #" + eventReceived.Player.Id);
            return true;
        }

        private static bool HandleEventPlayerHasPlayed(Event eventReceived)
        {
            Console.WriteLine("Player #" + eventReceived.Player.Id + " played " + CardBeautifuler.GetStringCard(eventReceived.Table.GetTopStackCard()));
            return true;
        }

        private static bool HandleEventError(Event eventReceived)
        {
            Console.WriteLine($"The server return an error : {eventReceived.ErrorMsg}");
            return true;
        }
    }
}