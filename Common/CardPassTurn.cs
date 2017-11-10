namespace Common
{
    public class CardPassTurn : Card
    {
        public CardPassTurn() : base(CardColor.Undefined, CardValue.PassTurn)
        {
            
        }

        public override void HandleUse(Player player)
        {
            base.HandleUse(player);
        }
    }
}