using FSM3;
using UnityEngine;

namespace TestFSM3
{
    public class TestIdleState : TestBaseState
    {
        protected override void OnEnterState(TestPlayer entity, IStateMachine<TestPlayer, PlayerState> stateMachine)
        {
            base.OnEnterState(entity, stateMachine);
            entity.WaitTime = 5.0f;
        }

        protected override void OnTickState(TestPlayer entity, IStateMachine<TestPlayer, PlayerState> stateMachine)
        {
            base.OnTickState(entity, stateMachine);

            entity.WaitTime = entity.WaitTime - Time.deltaTime;

            if (entity.WaitTime <= 0f)
            {
                stateMachine.ChangeState(PlayerState.Move);
            }
        }
    }
}
