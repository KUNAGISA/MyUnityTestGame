using FSM;
using System;

namespace Game.Test
{
    public class PlayerAttackFinishTransition : IPlayerTransition
    {
        public void Execute(IChangeState<IPlayerState> changeState)
        {
            changeState.ChangeState<PlayerNormalState>();
        }
    }
}
