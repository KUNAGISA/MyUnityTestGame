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
            entity.WaitEndTime = Time.time + 5.0f;
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