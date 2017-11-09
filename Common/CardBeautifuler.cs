using System;

namespace Common
{
    public class CardBeautifuler
    {
        private string[] _cardColors;
        private string[] _cardTypes;

        public CardBeautifuler()
        {
            _cardColors = new string[] {"Undefined", "Green", "Red", "Yellow", "Blue"};
            _cardTypes = new string[]
            {
                "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Plus_2", "Revert",
                "PassTurn", "ChangeColor", "Plus4"
            };
        }

        public string GetStringCard(Card card)
        {
            return string.Concat(_cardColors[(int) card.GetCardColor()], " ", _cardTypes[(int) card.GetCardValue()]);
        }
    }
}