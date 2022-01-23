using FSM3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TestFSM3
{
    [CreateAssetMenu(fileName = "IdleState", menuName = "AI State/Idle State")]
    public class AIIdleState : AIBaseState
    {
        [SerializeField]
        private float m_WaitTime = 5.0f;

        private float EndTime = 5.0f;

        protected override void OnEnterState(AIEntity entity, IStateMachine<AIEntity, AIState> stateMachine)
        {
            base.OnEnterState(entity, stateMachine);
            EndTime = m_WaitTime + Time.time;
            Debug.Log(entity.name + ":" + EndTime);
        }

        protected override void OnTickState(AIEntity entity, IStateMachine<AIEntity, AIState> stateMachine)
        {
            base.OnTickState(entity, stateMachine);
            if (EndTime <= Time.time)
            {
                stateMachine.ChangeState(AIState.Move);
            }
        }
    }
}
