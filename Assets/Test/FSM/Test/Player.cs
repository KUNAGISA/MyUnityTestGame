using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Test
{

    public class Player : MonoBehaviour, IEntity
    {
        public int face = 1;

        public float speed = 1.0f;

        [SerializeField]
        private float m_WaitEndTime = 0.0f;
        public float WaitEndTime { get => m_WaitEndTime; set => m_WaitEndTime = value; }

        private IStateMachine<Player, EntityStateTransition> m_StateMachine = null;

        private void Awake()
        {
            m_StateMachine = new StateMachine<Player, EntityStateTransition>();

            m_StateMachine.RegisterState(new EntityIdleState(), EntityStateTransition.MoveFinish);
            m_StateMachine.RegisterState(new PlayerMoveState(), EntityStateTransition.WaitFinish);

            m_StateMachine.InitStateMachine(this, typeof(EntityIdleState));

            m_StateMachine.SendMessage(new StateMsg());
        }

        private void Update()
        {
            m_StateMachine.TickStateMachine();
        }

        public void SetAnimatorBool(string tag, bool isTag)
        {
            GetComponent<Animator>()?.SetBool(tag, isTag);
        }
    }
}