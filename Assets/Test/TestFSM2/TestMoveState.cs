using UnityEngine;

namespace TestFSM2
{
    public class TestMoveState : TestBaseState
    {
        public TestMoveState(TestPlayer entity, ITestStateMachine stateMachine)
            : base(entity, stateMachine)
        {

        }

        protected override void OnTickState()
        {
            base.OnTickState();

            var movex = CurrEntity.face * CurrEntity.speeds * Time.deltaTime + CurrEntity.transform.position.x;
            CurrEntity.transform.position = new Vector3(movex, CurrEntity.transform.position.y, CurrEntity.transform.position.z);

            if (Mathf.Abs(CurrEntity.transform.position.x) >= 10)
            {
                CurrEntity.face = -CurrEntity.face;
                CurrStateMachine.ChangeState(CurrEntity.IdleState);
            }
        }
    }
}
