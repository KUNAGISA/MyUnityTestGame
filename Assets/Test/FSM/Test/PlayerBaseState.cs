using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Test
{
    public interface IPlayerState : IState<Player, EntityStateTransition>
    {

    }

    public class PlayerBaseState : BaseState<Player, EntityStateTransition>, IPlayerState
    {

    }

    public class PlayerMoveState : PlayerBaseState
    {
        protected override void OnTickState(Player entity, ITransition<EntityStateTransition> transition)
        {
            base.OnTickState(entity, transition);

            var face = Mathf.Sign(entity.face);
            var posx = entity.transform.position.x;

            posx = posx + entity.speed * Time.deltaTime * entity.face;
            entity.transform.position = new Vector3(posx, entity.transform.position.y, entity.transform.position.z);

            if ((face > 0 && posx >= 5.0f) || (face < 0 && posx <= -5.0f))
            {
                entity.face = -entity.face;
                transition.ChangeState(EntityStateTransition.MoveFinish);
            }
        }
    }
}