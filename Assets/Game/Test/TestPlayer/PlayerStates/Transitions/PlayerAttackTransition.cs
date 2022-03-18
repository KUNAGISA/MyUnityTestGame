using FSM;

namespace Game.Test
{
    public class PlayerAttackTransition : IPlayerTransition
    {
        void ITransition<IPlayerState>.Execute(IChangeState<IPlayerState> changeState)
        {
            changeState.ChangeState<PlayerAttackState>();
        }
    }
}
