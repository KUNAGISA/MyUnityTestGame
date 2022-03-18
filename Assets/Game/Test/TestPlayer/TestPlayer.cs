using FSM;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game.Test
{
    public class TestPlayer : MonoBehaviour
    {
        private StateMachine<IPlayerState> m_stateMachine;

        [SerializeField]
        public PlayableAsset normalPlayable;

        [SerializeField]
        public TimelineAsset attackPlayable;

        private void Awake()
        {
            m_stateMachine = new StateMachine<IPlayerState>();
            m_stateMachine.RegisterState(new PlayerNormalState(this));
            m_stateMachine.RegisterState(new PlayerAttackState(this));
            (m_stateMachine as IChangeState<IPlayerState>).ChangeState<PlayerNormalState>();
        }

        private void FixedUpdate()
        {
            m_stateMachine.TickStateMachine();
        }

        public void OnPlayerAttack()
        {
            m_stateMachine.SendMessage(new PlayerAttackMsg());
        }
    }
}