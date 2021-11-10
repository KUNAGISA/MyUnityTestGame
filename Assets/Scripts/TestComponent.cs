using Framework;

namespace Game
{
    public class TestComponent : AbstractsController, ICanGetUtility, ICanSendEvent
    {
        protected void Start()
        {
            this.SendEvent(new Event.PushViewEvent(ViewDefine.ViewName.TestView));
        }
    }
}