using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using UnityEngine.UI;

namespace Game
{
    public class TestComponent : AbstractsController, ICanGetUtility
    {
        public string path = "";

        private async void Start()
        {
            var assets = this.GetSystem<System.IAssetsSystem>();
            var obj = await assets.GetAssetsAsync<GameObject>(path);
            Instantiate(obj);
        }
    }
}