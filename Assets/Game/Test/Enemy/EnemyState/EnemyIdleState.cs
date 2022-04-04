using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Game.Test
{
    public class EnemyIdleState : BasePlayerState //, IPlayerReceiveMsg<PlayerAttackMsg>
    {
        private PlayableGraph m_playableGraph;
        private Animator m_animator;
        private AnimationClip m_IdleClip;


        public EnemyIdleState(EnemyPlayer player) : base(player)
        {
            m_animator = player.m_Animator;
            m_IdleClip = player.m_IdleClip;
            m_playableGraph = PlayableGraph.Create("EnemyIdleAnimation");
            m_playableGraph.SetTimeUpdateMode(DirectorUpdateMode.Manual);

        }
        protected override void OnEnterState()
        {
            base.OnEnterState();
            playAnima();
        }



        private void playAnima()
        {
            PlayableUtility.PlayAnima(m_playableGraph, "Idle", m_IdleClip, m_animator);
        }

        protected override ITransition<IPlayerState> OnTickState()
        {
            m_playableGraph.Evaluate(Time.deltaTime);
            return base.OnTickState();
        }

        protected override void OnExitState()
        {
            m_playableGraph.Stop();
            m_playableGraph.GetRootPlayable(0).SetTime(0);
            base.OnExitState();
        }

    }
}

