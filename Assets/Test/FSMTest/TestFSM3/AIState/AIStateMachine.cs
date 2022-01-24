using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestFSM3
{
    public enum AIState
    {
        Idle,
        Move,
    }

    public class AIStateMachine : FSM3.StateMachine<AIEntity, AIState>
    {
        [SerializeField]
        private AIIdleState m_AIIdleState;

        [SerializeField]
        private AIMoveState m_AIMoveState;

        protected override void OnInitStateMachine()
        {
            /// 考虑复用问题，可能要做一个黑板参数类来给State用
            RegisterState(AIState.Idle, Instantiate(m_AIIdleState));
            RegisterState(AIState.Move, Instantiate(m_AIMoveState));

            ChangeState(AIState.Idle);
        }
    }
}
