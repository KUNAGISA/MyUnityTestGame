namespace Attack
{
    /// <summary>
    /// 攻击逻辑接口
    /// </summary>
    public interface IAttackLogic
    {
        /// <summary>
        /// 进入逻辑
        /// </summary>
        void EnterLogic();

        /// <summary>
        /// 逻辑帧更新
        /// </summary>
        void TickLogic();

        /// <summary>
        /// 离开逻辑
        /// </summary>
        void ExitLogic();
    }
}