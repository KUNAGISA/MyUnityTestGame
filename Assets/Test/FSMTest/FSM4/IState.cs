using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM4
{
    public interface IState<Entity>
    {
        void EnterState(Entity entity, ICanChangeState<Entity> stateMachine);

        void ExitState(Entity entity, ICanChangeState<Entity> stateMachine);

        void TickState(Entity entity, ICanChangeState<Entity> stateMachine);
    }
}