using FSM;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.Test
{
    public class PlayerNormalState : BasePlayerState, IPlayerReceiveMsg<PlayerAttackMsg>
    {
        private PlayableGraph m_playable;

        public PlayerNormalState(TestPlayer player) : base(player)
        {
            m_playable = PlayableGraph.Create("Normal");
            m_playable.SetTimeUpdateMode(DirectorUpdateMode.Manual);
            player.normalPlayable.CreatePlayable(m_playable, player.gameObject);
        }

        protected override void OnEnterState()
        {
            base.OnEnterState();
            m_playable.Play();
        }

        protected override ITransition<IPlayerState> OnTickState()
        {
            m_playable.Evaluate(Time.deltaTime);
            return base.OnTickState();
        }

        protected override void OnExitState()
        {
            m_playable.Stop();
            m_playable.GetRootPlayable(0).SetTime(0);
            base.OnExitState();
        }

        ITransition<IPlayerState> IReceiveMsg<IPlayerState, PlayerAttackMsg>.ReceiveMsg(in PlayerAttackMsg message)
        {
            return new PlayerAttackTransition();
        }
    }
}