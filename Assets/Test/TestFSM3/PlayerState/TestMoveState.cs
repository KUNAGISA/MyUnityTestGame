using FSM3;
using UnityEngine;

namespace TestFSM3
{
    class TestMoveState : TestBaseState
    {
        protected override void OnTickState(TestPlayer entity, IStateMachine<TestPlayer, PlayerState> stateMachine)
        {
            base.OnTickState(entity, stateMachine);

            var movex = entity.face * entity.speeds * Time.deltaTime + entity.transform.position.x;
            entity.transform.position = new Vector3(movex, entity.transform.position.y, entity.transform.position.z);

            if (Mathf.Abs(entity.transform.position.x) >= 5.0f)
            {
                entity.face = -entity.face;
                stateMachine.ChangeState(PlayerState.Idle);
            }
        }
    }
}
