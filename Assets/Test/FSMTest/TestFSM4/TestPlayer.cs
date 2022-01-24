using UnityEngine;

namespace TestFSM4
{
    public class TestPlayer : MonoBehaviour
    {
        public float WaitTime = 0.0f;
        public float face = -1;

        public float speeds = 1.0f;

        private ITestStateMachine m_TestStateMachine;

        private void Awake()
        {
            m_TestStateMachine = GetComponent<ITestStateMachine>();
            m_TestStateMachine.InitStateMachine(this);

            m_TestStateMachine.RegisterState<ITestIdleState>(new TestIdleState());
            m_TestStateMachine.RegisterState<ITestMoveState>(new TestMoveState());

            m_TestStateMachine.ChangeState<ITestIdleState>();
        }

        private void Update()
        {
            m_TestStateMachine?.TickStateMachin();
        }
    }
}
