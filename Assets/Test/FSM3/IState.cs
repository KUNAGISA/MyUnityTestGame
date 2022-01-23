using System;

namespace FSM3
{
    public interface IState<Entity, StateType> where StateType : Enum
    {
        void EnterState(Entity entity, IStateMachine<Entity, StateType> stateMachine);

        void ExitState(Entity entity, IStateMachine<Entity, StateType> stateMachine);

        void TickState(Entity entity, IStateMachine<Entity, StateType> stateMachine);
    }
}