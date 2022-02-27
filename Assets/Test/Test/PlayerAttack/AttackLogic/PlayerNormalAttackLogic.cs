using Attack;

namespace Test
{
    public class PlayerNormalAttackLogic : BaseAttackLogic<Player, PlayerNormalAttackData.PlayerNormalAttack>
    {
        public PlayerNormalAttackLogic(Player player, PlayerNormalAttackData.PlayerNormalAttack data): base(player, data)
        {

        }

        protected override void OnTickLogic()
        {
        }
    }
}
