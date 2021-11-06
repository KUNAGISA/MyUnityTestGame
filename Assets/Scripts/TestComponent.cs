using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    public class TestComponent : AbstractsController, ICanGetUtility
    {
        public string path = "";
        private void Start()
        {
            var assets = this.GetUtility<Utility.IAssetsUtility>();
            assets.LoadAssetAsync(path, delegate(GameObject go)
            {
                var newgo = Instantiate(go);
                Destroy(newgo, 3.0f);
                assets.ReleaseAsset(go);
            });

            var timer = this.GetSystem<System.ITimerSystem>();
            timer.AddDelayTask(5.0f, (dt) => Debug.Log("Delay Call " + dt.ToString()))
                .KillWhenGameObjectDestroy(gameObject);

            var index = 10;
            timer.AddTask(1.0f, delegate (float dt)
            {
                index = index - 1;
                if (index <= 0)
                {
                    Destroy(gameObject);
                }
            }).KillWhenGameObjectDestroy(gameObject);

            timer.AddTask(0.0f, (dt) => Debug.Log("Timer " + dt.ToString()))
                .KillWhenGameObjectDestroy(gameObject);
        }
    }
}