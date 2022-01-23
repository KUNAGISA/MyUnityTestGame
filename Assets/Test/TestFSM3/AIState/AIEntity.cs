using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestFSM3
{
    public class AIEntity : MonoBehaviour
    {
        public float face = 1.0f;
        public float speeds = 1.0f;

        private FSM3.IStateMachine<AIEntity, AIState> m_StateMachine;

        private void Awake()
        {
            m_StateMachine = GetComponent<FSM3.IStateMachine<AIEntity, AIState>>();
            m_StateMachine.InitStateMachine(this);
        }

        private void Update()
        {
            m_StateMachine?.TickStateMachine();
        }
    }
}