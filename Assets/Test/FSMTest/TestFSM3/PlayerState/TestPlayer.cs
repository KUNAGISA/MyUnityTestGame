using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFSM3
{
    public enum PlayerState
    {
        Idle,
        Move,
    }

    public class TestPlayer : MonoBehaviour
    {
        private FSM3.IStateMachine<TestPlayer, PlayerState> m_StateMachine;

        public float WaitTime = 0.0f;
        public float face = -1;

        public float speeds = 1.0f;

        private void Awake()
        {
            m_StateMachine = GetComponent<FSM3.IStateMachine<TestPlayer, PlayerState>>();

            m_StateMachine.InitStateMachine(this);
            m_StateMachine.RegisterState(PlayerState.Idle, new TestIdleState());
            m_StateMachine.RegisterState(PlayerState.Move, new TestMoveState());

            m_StateMachine.ChangeState(PlayerState.Idle);
        }

        private void Update()
        {
            m_StateMachine?.TickStateMachine();
        }
    }
}