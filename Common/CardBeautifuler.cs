using System;
using System.Collections.Generic;

namespace Common
{
    public class CardBeautifuler
    {
        public static string GetStringCard(Card card)
        {
            string colorString;
            string valueString;
            string jokerColorString;

            var colors = new List<KeyValuePair<CardColor, string>>
            {
                new KeyValuePair<CardColor, string>(CardColor.Undefined,
                    "Undefined"),
                new KeyValuePair<CardColor, string>(CardColor.Green, "Green"),
                new KeyValuePair<CardColor, string>(CardColor.Red, "Red"),
                new KeyValuePair<CardColor, string>(CardColor.Yellow, "Yellow"),
                new KeyValuePair<CardColor, string>(CardColor.Blue, "Blue")
            };
            
            var values = new List<KeyValuePair<CardValue, string>>
            {
                new KeyValuePair<CardValue, string>(CardValue.Zero, "Zero"),
                new KeyValuePair<CardValue, string>(CardValue.One, "One"),
                new KeyValuePair<CardValue, string>(CardValue.Two, "Two"),
                new KeyValuePair<CardValue, string>(CardValue.Three, "Three"),
                new KeyValuePair<CardValue, string>(CardValue.Four, "Four"),
                new KeyValuePair<CardValue, string>(CardValue.Five, "Five"),
                new KeyValuePair<CardValue, string>(CardValue.Six, "Six"),
                new KeyValuePair<CardValue, string>(CardValue.Seven, "Seven"),
                new KeyValuePair<CardValue, string>(CardValue.Eight, "Eight"),
                new KeyValuePair<CardValue, string>(CardValue.Nine, "Nine"),
                new KeyValuePair<CardValue, string>(CardValue.ChangeColor, "ChangeColor"),
                new KeyValuePair<CardValue, string>(CardValue.PassTurn, "PassTurn"),
                new KeyValuePair<CardValue, string>(CardValue.Plus2, "Plus 2"),
                new KeyValuePair<CardValue, string>(CardValue.Plus4, "Plus 4"),
                new KeyValuePair<CardValue, string>(CardValue.Revert, "Revert"),
            };

            try
            {
                colorString = colors.Find(x => x.Key == card.Color).Value;
                jokerColorString = colors.Find(x => x.Key == card.JokerColor).Value;
                valueString = values.Find(x => x.Key == card.Value).Value;
            }
            catch (Exception e)
            {
                throw new Exception("Impossible de traduire la carte");
            }            
            return $"{colorString} {valueString} {jokerColorString}";
        }
    }
}