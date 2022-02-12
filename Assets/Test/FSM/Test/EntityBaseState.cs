using UnityEngine;

namespace FSM.Test
{
    public interface IEntityBaseState<ETransition> : IState<IEntity, ETransition>
    {

    }

    public class EntityBaseState<ETransition> : BaseState<IEntity, ETransition>, IEntityBaseState<ETransition>
    {
        
    }

    public class EntityIdleState : EntityBaseState<EntityStateTransition>
    {
        protected override void OnEnterState(IEntity entity, ITransition<EntityStateTransition> transition)
        {
            base.OnEnterState(entity, transition);

            entity.SetAnimatorBool("idle", true);
            entity.WaitEndTime = Time.time + 5.0f;
        }

        protected override void OnExitState(IEntity entity, ITransition<EntityStateTransition> transition)
        {
            entity.SetAnimatorBool("idle", false);
            base.OnExitState(entity, transition);
        }

        protected override void OnTickState(IEntity entity, ITransition<EntityStateTransition> transition)
        {
            base.OnTickState(entity, transition);
            if (entity.WaitEndTime <= Time.time)
            {
                transition.TransState(EntityStateTransition.WaitFinish);
            }
        }
    }
}