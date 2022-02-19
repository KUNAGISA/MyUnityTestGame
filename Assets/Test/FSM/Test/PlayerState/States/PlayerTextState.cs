using UnityEngine;

namespace FSM.Test
{
    public class PlayerTextState : PlayerBaseState
    {
        protected override void OnEnterState()
        {
            base.OnEnterState();

            var animator = entity.GetComponent<Animator>();
            animator.Play("PlayerTextAnim");
        }

        protected override ITransition<Player> OnTickState()
        {
            var animator = entity.GetComponent<Animator>();
            var animStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (!animStateInfo.IsName("PlayerTextAnim"))
            {
                return new PlayerTextAnimFinish();
            }

            return base.OnTickState();
        }
    }
}
