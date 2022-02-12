using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM.Test
{
    public interface IPlayerState : IState<Player, EntityStateTransition>
    {

    }

    public interface IPlayerStateReceiveMsg<TMessage> : IReceiveMsg<TMessage, Player, EntityStateTransition>
    {

    }

    public class PlayerBaseState : BaseState<Player, EntityStateTransition>, IPlayerState
    {

    }

    public struct StateMsg
    { 
    }


    public class PlayerMoveState : PlayerBaseState, IPlayerStateReceiveMsg<StateMsg>
    {
        protected override void OnEnterState(Player entity, ITransition<EntityStateTransition> transition)
        {
            base.OnEnterState(entity, transition);
            entity.SetAnimatorBool("move", true);
        }

        protected override void OnExitState(Player entity, ITransition<EntityStateTransition> transition)
        {
            entity.SetAnimatorBool("move", false);
            base.OnExitState(entity, transition);
        }

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
                transition.TransState(EntityStateTransition.MoveFinish);
            }
        }

        void IReceiveMsg<StateMsg, Player, EntityStateTransition>.ReceiveMsg(Player entity, ITransition<EntityStateTransition> transition, in StateMsg message)
        {

        }
    }
}