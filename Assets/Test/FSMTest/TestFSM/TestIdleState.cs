using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test
{
    public class TestIdleState : TestBaseState
    {
        protected override void OnEnterState(TestPlayer entity, IStateMachine<TestPlayer> stateMachine)
        {
            base.OnEnterState(entity, stateMachine);
            entity.WaitTime = 5.0f;
        }

        protected override void OnUpdateState(TestPlayer entity, IStateMachine<TestPlayer> stateMachine)
        {
            base.OnUpdateState(entity, stateMachine);
            entity.WaitTime = entity.WaitTime - Time.deltaTime;
            
            if (entity.WaitTime <= 0f)
            {
                stateMachine.ChangeState(new TestMoveState());
            }
        }
    }
}