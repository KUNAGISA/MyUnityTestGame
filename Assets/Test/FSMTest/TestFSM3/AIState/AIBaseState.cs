using FSM3;
using UnityEngine;

namespace TestFSM3
{
    public class AIBaseState : ScriptableObject, FSM3.IState<AIEntity, AIState>
    {
        void IState<AIEntity, AIState>.EnterState(AIEntity entity, IStateMachine<AIEntity, AIState> stateMachine)
        {
            OnEnterState(entity, stateMachine);
        }

        void IState<AIEntity, AIState>.ExitState(AIEntity entity, IStateMachine<AIEntity, AIState> stateMachine)
        {
            OnExitState(entity, stateMachine);
        }

        void IState<AIEntity, AIState>.TickState(AIEntity entity, IStateMachine<AIEntity, AIState> stateMachine)
        {
            OnTickState(entity, stateMachine);
        }

        protected virtual void OnEnterState(AIEntity entity, IStateMachine<AIEntity, AIState> stateMachine) { }

        protected virtual void OnExitState(AIEntity entity, IStateMachine<AIEntity, AIState> stateMachine) { }

        protected virtual void OnTickState(AIEntity entity, IStateMachine<AIEntity, AIState> stateMachine) { }
    }
}
