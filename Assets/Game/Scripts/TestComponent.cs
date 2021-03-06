using Framework;
using UnityEngine;

namespace Game
{
    public class TestComponent : BaseMonoController, ICanGetUtility, ICanSendEvent
    {
        protected void Start()
        {
            this.GetSystem<System.IViewSystem>().Push(ViewDefine.ViewName.TestView);
        }
    }
}