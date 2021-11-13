using Framework;
using UnityEngine;

namespace Game
{
    public class TestComponent : AbstractsController, ICanGetUtility, ICanSendEvent
    {
        protected void Start()
        {
            this.SendEvent(new Event.PushViewEvent(ViewDefine.ViewName.TestView));
            this.GetSystem<System.ITimeSystem>().AddTickTask((delta) => Debug.Log("定时器" + delta))
                .KillWhenGameObjectDestroy(gameObject);
        }
    }
}