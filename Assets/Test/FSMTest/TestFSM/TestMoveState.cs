using FSM;
using UnityEngine;

namespace Test
{
    public class TestMoveState : TestBaseState
    {
        protected override void OnEnterState(TestPlayer entity, IStateMachine<TestPlayer> stateMachine)
        {
            base.OnEnterState(entity, stateMachine);
        }

        protected override void OnUpdateState(TestPlayer entity, IStateMachine<TestPlayer> stateMachine)
        {
            base.OnUpdateState(entity, stateMachine);

            var movex = entity.face * entity.speeds * Time.deltaTime + entity.transform.position.x;
            entity.transform.position = new Vector3(movex, entity.transform.position.y, entity.transform.position.z);

            if (Mathf.Abs(entity.transform.position.x) >= 10)
            {
                entity.face = -entity.face;
                stateMachine.ChangeState(new TestIdleState());
            }
        }
    }
}
