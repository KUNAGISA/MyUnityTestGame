using UnityEngine;
using FSM4;

namespace TestFSM4
{
    public interface ITestMoveState : ITestBaseState
    {

    }

    public class TestMoveState : TestBaseState, ITestMoveState
    {
        protected override void OnTickState(TestPlayer entity, ICanChangeState<TestPlayer> stateMachine)
        {
            base.OnTickState(entity, stateMachine);

            var movex = entity.face * entity.speeds * Time.deltaTime + entity.transform.position.x;
            entity.transform.position = new Vector3(movex, entity.transform.position.y, entity.transform.position.z);

            if (Mathf.Abs(entity.transform.position.x) >= 5.0f)
            {
                entity.face = -entity.face;
                stateMachine.ChangeState<ITestIdleState>();
            }
        }
    }
}
