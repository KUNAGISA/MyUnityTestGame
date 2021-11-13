using Framework;

namespace Game
{
    public sealed class Game2D : Architecture<Game2D>
    {
        protected override void Init()
        {
            RegisterSystem<System.ITimeSystem>(new System.TimeSystem());
            RegisterSystem<System.IAssetsSystem>(new System.AssetsSystem());

            /// 用Addressables来管理资源
            RegisterUtility<Utility.IAssetsUtility>(new Utility.AddressablesAssetsUtility());
        }
    }
}