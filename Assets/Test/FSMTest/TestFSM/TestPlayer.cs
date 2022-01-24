using UnityEngine;

namespace Test
{
    public class TestPlayer : MonoBehaviour
    {
        private FSM.IStateMachine<TestPlayer> m_stateMachine;

        public float WaitTime = 0.0f;
        public float face = -1;

        public float speeds = 1.0f;

        private void Awake()
        {
            m_stateMachine = GetComponent<FSM.IStateMachine<TestPlayer>>();
            m_stateMachine?.InitStateMachine(this, new TestIdleState());
        }

        private void Update()
        {
            m_stateMachine?.FrameTick();
        }
    }
}
