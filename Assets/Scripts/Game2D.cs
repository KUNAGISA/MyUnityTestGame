using Framework;

namespace Game
{
    public sealed class Game2D : Architecture<Game2D>
    {
        protected override void Init()
        {
            RegisterSystem<System.ITimerSystem>(new System.TimerSystem());

            /// 用Addressables来管理资源
            RegisterUtility<Utility.IAssetsUtility>(new Utility.AddressablesAssetsUtility());
        }
    }
}