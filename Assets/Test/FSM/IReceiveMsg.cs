namespace FSM
{
    public interface IReceiveMsg<TMessage, in Entity, ETransition>
    {
        public void ReceiveMsg(Entity entity, ITransition<ETransition> transition, in TMessage message);
    }
}
