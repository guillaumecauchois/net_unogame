namespace Common
{
    public class CardBeautifuler
    {
        public static string GetStringCard(Card card)
        {
            var CardColors = new [] {"Undefined", "Green", "Red", "Yellow", "Blue"};
            var CardTypes = new[]
            {
                "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven",
                "Eight", "Nine", "Plus_2", "Revert",
                "PassTurn", "ChangeColor", "Plus4"
            };
            
            return string.Concat(CardColors[(int) card.Color], " ", CardTypes[(int) card.Value]);
        }
    }
}