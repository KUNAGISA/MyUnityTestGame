using UnityEngine;

namespace TestFSM2
{
    public class TestPlayer : MonoBehaviour
    {
        private ITestStateMachine m_TestStateMachine;

        public float WaitTime = 0.0f;
        public float face = -1;

        public float speeds = 1.0f;

        public FSM2.IState IdleState { get; private set; }
        public FSM2.IState MoveState { get; private set; }

        private void Awake()
        {
            m_TestStateMachine = GetComponent<ITestStateMachine>();

            IdleState = new TestIdleState(this, m_TestStateMachine);
            MoveState = new TestMoveState(this, m_TestStateMachine);

            m_TestStateMachine.ChangeState(IdleState);
        }

        private void Update()
        {
            m_TestStateMachine.FrameTick();
        }
    }
}
