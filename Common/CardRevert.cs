
namespace Common
{
    public class CardRevert : Card
    {
        public CardRevert() : base(CardColor.Undefined, CardValue.Revert)
        {

        }

        public override void HandleUse(Player player)
        {
            base.HandleUse(player);
        }
    }
}