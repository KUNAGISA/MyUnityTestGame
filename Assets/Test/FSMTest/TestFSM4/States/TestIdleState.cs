using UnityEngine;
using FSM4;

namespace TestFSM4
{
    public interface ITestIdleState : ITestBaseState
    {

    }

    public class TestIdleState : TestBaseState, ITestIdleState
    {

        protected override void OnEnterState(TestPlayer entity, ICanChangeState<TestPlayer> stateMachine)
        {
            base.OnEnterState(entity, stateMachine);
            entity.WaitTime = 5.0f;
        }

        protected override void OnTickState(TestPlayer entity, ICanChangeState<TestPlayer> stateMachine)
        {
            base.OnTickState(entity, stateMachine);

            entity.WaitTime = entity.WaitTime - Time.deltaTime;

            if (entity.WaitTime <= 0f)
            {
                stateMachine.ChangeState<ITestMoveState>();
            }
        }
    }
}
