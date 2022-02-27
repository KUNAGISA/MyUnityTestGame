namespace Attack
{
    public abstract class BaseAttackLogic<TEntity, TAttackData> : IAttackLogic
    {
        public TEntity entity { get; private set; }

        protected readonly TAttackData data;

        public BaseAttackLogic(TEntity entity, TAttackData data)
        {
            this.entity = entity;
            this.data = data;
        }

        void IAttackLogic.EnterLogic() => OnEnterLogic();

        void IAttackLogic.TickLogic() => OnTickLogic();

        void IAttackLogic.ExitLogic() => OnExitLogic();

        protected virtual void OnEnterLogic() { }

        protected virtual void OnExitLogic() { }

        protected abstract void OnTickLogic();

    }

    public abstract class BaseAttackLogic<TEntity> : IAttackLogic
    {
        public TEntity entity { get; private set; }

        public BaseAttackLogic(TEntity entity)
        {
            this.entity = entity;
        }

        void IAttackLogic.EnterLogic() => OnEnterLogic();

        void IAttackLogic.TickLogic() => OnTickLogic();

        void IAttackLogic.ExitLogic() => OnExitLogic();

        protected virtual void OnEnterLogic() { }

        protected virtual void OnExitLogic() { }

        protected abstract void OnTickLogic();
    }
}
