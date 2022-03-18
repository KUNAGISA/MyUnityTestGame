using FSM;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Test
{
    public class PlayerAttackState : BasePlayerState
    {
        private PlayableGraph m_playable;

        public PlayerAttackState(TestPlayer player) : base(player)
        {
            m_playable = PlayableGraph.Create("Attack");
            m_playable.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            player.attackPlayable.CreatePlayable(m_playable, player.gameObject);
        }

        protected override void OnEnterState()
        {
            base.OnEnterState();
            m_playable.Play();
        }

        protected override void OnExitState()
        {
            m_playable.Stop();
            m_playable.GetRootPlayable(0).SetTime(0);
            base.OnExitState();
        }

        protected override ITransition<IPlayerState> OnTickState()
        {
            var duration = m_playable.GetRootPlayable(0).GetDuration();
            if (duration <= m_playable.GetRootPlayable(0).GetTime())
            {
                return new PlayerAttackFinishTransition();
            }
            else
            {
                m_playable.Evaluate(Time.deltaTime);
            }
            return base.OnTickState();
        }
    }
}
