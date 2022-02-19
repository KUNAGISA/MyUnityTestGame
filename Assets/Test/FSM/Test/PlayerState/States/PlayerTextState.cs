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
            if (!entity.IsInBattle)
            {
                return new PlayerTextAnimFinish();
            }

            return base.OnTickState();
        }
    }
}
