using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FSM.Test
{
    public struct TestMsg
    {

    }

    public class Player : MonoBehaviour
    {
        public int face = 1;

        public float speed = 1.0f;

        public bool IsInBattle = false;

        [SerializeField]
        private float m_WaitEndTime = 0.0f;
        public float WaitEndTime { get => m_WaitEndTime; set => m_WaitEndTime = value; }

        private StateMachine<Player> m_StateMachine = null;

        private void Awake()
        {
            m_StateMachine = new StateMachine<Player>(this);
            m_StateMachine.RegisterState(new PlayerIdleState());
            m_StateMachine.RegisterState(new PlayerTextState());
            m_StateMachine.ChangeState<PlayerIdleState>();
        }

        private void Update()
        {
            m_StateMachine.TickStateMachine();
        }

        public void SetAnimatorBool(string tag, bool isTag)
        {
            GetComponent<Animator>()?.SetBool(tag, isTag);
        }

        public void OnInput()
        {
            m_StateMachine.SendMessage(new TestMsg());
        }
    }
}