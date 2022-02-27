using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Test
{
    public struct PlayerIdleFinish : ITransition<Player>
    {
        public void Execute(Player entity, IChangeState<Player> transition)
        {
            transition.ChangeState<PlayerTextState>();

        }
    }
}