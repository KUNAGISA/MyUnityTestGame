using Framework;

namespace Game
{
    public sealed class Game2D : Architecture<Game2D>
    {
        protected override void Init()
        {
            RegisterSystem<System.ITimeSystem>(new System.TimeSystem());
            RegisterSystem<System.IAssetsSystem>(new System.AssetsSystem());
            RegisterSystem<System.IWorldSystem>(new System.WorldSystem());
            RegisterSystem<System.IInputSystem>(new System.InputSystem());
            RegisterSystem<System.IViewSystem>(new System.ViewSystem());

            /// 用Addressables来管理资源
            RegisterUtility<Utility.IAssetsUtility>(new Utility.AddressablesAssetsUtility());
        }
    }
}