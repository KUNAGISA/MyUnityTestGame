using UnityEngine;

namespace FSM.Test
{
    public class PlayerIdleState : PlayerBaseState, IPlayerReceive<TestMsg>
    {
        public ITransition<Player> ReceiveMsg(in TestMsg message)
        {
            var animator = entity.GetComponent<Animator>();
            animator.Play("PlayerTextAnim");

            if (entity.IsInBattle)
                return new PlayerIdleFinish();
            return null;
        }

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

            if (entity.IsInBattle)
                return new PlayerIdleFinish();

            return base.OnTickState();
        }
    }
}