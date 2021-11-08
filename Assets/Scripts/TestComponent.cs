using UnityEngine;
using Framework;

namespace Game
{
    public class TestComponent : AbstractsController, ICanGetUtility, ICanSendEvent
    {
        protected override void Start()
        {
            base.Start();
            this.SendEvent(new Event.PushViewEvent(ViewDefine.ViewName.TestView));
        }
    }
}