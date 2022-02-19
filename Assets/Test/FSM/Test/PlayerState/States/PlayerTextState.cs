using UnityEngine;

namespace FSM.Test
{
    public class PlayerTextState : PlayerBaseState
    {
        protected override void OnEnterState()
        {
            base.OnEnterState();
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
