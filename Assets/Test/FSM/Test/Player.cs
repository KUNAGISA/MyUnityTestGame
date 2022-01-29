using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Test
{

    public class Player : MonoBehaviour, IEntity
    {
        public int face = 1;

        public float speed = 1.0f;

        public float WaitEndTime { get; set; }

        private IStateMachine<Player, EntityStateTransition> m_StateMachine = null;

        private void Awake()
        {
            m_StateMachine = new StateMachine<Player, EntityStateTransition>(this);

            m_StateMachine.RegisterState(new EntityIdleState())
                .Transition(EntityStateTransition.StateInit)
                .Transition(EntityStateTransition.MoveFinish);

            m_StateMachine.RegisterState(new PlayerMoveState())
                .Transition(EntityStateTransition.WaitFinish);

            m_StateMachine.ChangeState(EntityStateTransition.StateInit);
        }

        private void Update()
        {
            m_StateMachine.TickStateMachine();
        }
    }
}