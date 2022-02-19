using UnityEngine;

namespace FSM.Test
{
    public class PlayerIdleState : PlayerBaseState
    {
        protected override void OnEnterState()
        {
            base.OnEnterState();
            entity.SetAnimatorBool("idle", true);

            entity.WaitEndTime = 5.0f;
        }

        protected override void OnExitState()
        {
            entity.SetAnimatorBool("idle", false);
            base.OnExitState();
        }

        protected override ITransition<Player> OnTickState()
        {
            entity.WaitEndTime = entity.WaitEndTime - Time.deltaTime;

            if (entity.WaitEndTime <= 0.0f)
            {
                return new PlayerIdleFinish();
            }

            return base.OnTickState();
        }
    }
}