using System;

namespace Common
{
    public class CardBeautifuler
    {
        private string[] CardColors;
        private string[] CardTypes;

        public CardBeautifuler()
        {
            CardColors = new string[] {"Undefined", "Green", "Red", "Yellow", "Blue"};
            CardTypes = new string[]
            {
                "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Plus_2", "Revert",
                "PassTurn", "ChangeColor", "Plus4"
            };
        }

        public string GetStringCard(Card card)
        {
            return string.Concat(CardColors[(int) card.Color], " ", CardTypes[(int) card.Value]);
        }
    }
}