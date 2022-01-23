using UnityEngine;

namespace TestFSM2
{
    public class TestIdleState : TestBaseState
    {
        public TestIdleState(TestPlayer entity, ITestStateMachine stateMachine)
            : base(entity, stateMachine)
        {

        }

        protected override void OnEnterState()
        {
            base.OnEnterState();
            CurrEntity.WaitTime = 5.0f;
        }

        protected override void OnTickState()
        {
            base.OnTickState();

            CurrEntity.WaitTime = CurrEntity.WaitTime - Time.deltaTime;

            if (CurrEntity.WaitTime <= 0f)
            {
                CurrStateMachine.ChangeState(CurrEntity.MoveState);
            }
        }
    }
}
