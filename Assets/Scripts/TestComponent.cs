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
        }
    }
}