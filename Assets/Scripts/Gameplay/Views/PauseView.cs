using Framework;

namespace Game.View
{
    public class PauseView : BaseView
    {
        private void Start()
        {
            this.GetSystem<System.IWorldSystem>().Pause("PauseView");
        }

        private void OnDestroy()
        {
            this.GetSystem<System.IWorldSystem>().Resume("PauseView");
        }
    }
}