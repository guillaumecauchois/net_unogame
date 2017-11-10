using System;
using System.Collections.Generic;
using Common;
using NUnit.Framework.Internal;

namespace Client
{
    public class ClientEventHandler
    {
        private delegate void ClientEventHandlerCmd();
        private ClientEventHandlerCmd[] _events;
        private CardBeautifuler _beautifuler;

        public ClientEventHandler()
        {
            Event = null;
            /*var _events = new Dictionary<int, ClientEventHandlerCmd>();
            {
                {(int)EventType.YourTurn, HandleEventYourTurn},
                {(int)EventType.PlayerTurn, HandleEventPlayerTurn},
                {(int)EventType.EndGame, HandleEventEndGame},
                {(int)EventType.PlayerHasPlayed, HandleEventPlayerHasPlayed},
                {(int)EventType.InvalidCommand, HandleEventInvalidCommand}
            };*/
        }
        
        public Event Event { get; set; }

        public void HandleEvent(Event eventReceived)
        {
            Event = eventReceived;
            if (Event.Type == EventType.YourTurn)
            {
                HandleEventYourTurn();
            }
            else if (Event.Type == EventType.PlayerTurn)
            {
                HandleEventPlayerTurn();
            }
            else if (Event.Type == EventType.PlayerHasPlayed)
            {
                HandleEventPlayerHasPlayed();
            }
            else if (Event.Type == EventType.EndGame)
            {
                HandleEventEndGame();
            }
            /*try
            {
                _events[(int)Event.Type].Invoke();
            }
            catch (Exception e)
            {
                Console.Error.Write("[ERR] Receive invalid Event Type : " + Event.Type);
            }*/
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