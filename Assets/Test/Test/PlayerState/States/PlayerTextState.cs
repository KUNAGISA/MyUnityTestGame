using UnityEngine;

namespace FSM.Test
{
    public class PlayerTextState : PlayerBaseState
    {
        protected override void OnEnterState()
        {
            base.OnEnterState();
            entity.GetComponent<Animator>().Play("PlayerTextAnim");
            entity.GetComponent<Animator>().Update(0);
        }

        protected override void OnExitState()
        {
            base.OnExitState();
        }

        protected override ITransition<Player> OnTickState()
        {
            var animator = entity.GetComponent<Animator>();
            var state = animator.GetCurrentAnimatorStateInfo(0);
            if (!state.IsName("PlayerTextAnim") || state.normalizedTime >= 0.95)
            {
                return new PlayerTextAnimFinish();
            }

            return base.OnTickState();
        }
    }
}
