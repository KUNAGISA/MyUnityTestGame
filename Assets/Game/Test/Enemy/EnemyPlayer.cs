using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Test
{
    public class EnemyPlayer : BasePlayer
    {
        public EnemyPlayer()
        {
            kStayIdleTime = 3;
        }

        //[SerializeField]
        //public PlayableAsset m_IdelPlayable;
        [SerializeField]
        public Animator m_Animator;
        [SerializeField]
        public AnimationClip m_IdleClip;
        [SerializeField]
        public AnimationClip m_RunClip;
        [SerializeField]
        public AnimationClip m_WalkClip;
        private void Awake()
        {
            m_stateMachine = new StateMachine<IPlayerState>();
            m_stateMachine.RegisterState(new EnemyIdleState(this));
            m_stateMachine.RegisterState(new EnemyGoRoundState(this));

            (m_stateMachine as IChangeState<IPlayerState>).ChangeState<EnemyGoRoundState>();
        }

        private void FixedUpdate()
        {
            m_stateMachine.TickStateMachine();
        }

        #region 常量参数
        /// <summary>
        /// 停留待机状态时长
        /// </summary>
        private readonly int kStayIdleTime;
        #endregion


        private void Update()
        {


        }

    }




}
