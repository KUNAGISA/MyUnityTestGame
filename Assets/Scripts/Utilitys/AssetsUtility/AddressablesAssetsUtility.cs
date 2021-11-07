using Framework;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Game.Utility
{
    /// <summary>
    /// Addressables资源管理工具
    /// </summary>
    public class AddressablesAssetsUtility : AbstractUtility, IAssetsUtility
    {
        private string GetResPath(in string path) => "Assets/" + path;

        public TObject LoadAsset<TObject>(string path) where TObject : UnityEngine.Object
        {
            var fullPath = GetResPath(path);
            var task = Addressables.LoadAssetAsync<TObject>(fullPath);
            task.WaitForCompletion();
            return task.Result;
        }

        public async Task<TObject> LoadAssetAsync<TObject>(string path) where TObject : UnityEngine.Object
        {
            var fullPath = GetResPath(path);
            return await Addressables.LoadAssetAsync<TObject>(fullPath).Task;
        }

        public void ReleaseAsset<TObject>(TObject obj) where TObject : UnityEngine.Object
        {
            Addressables.Release(obj);
        }
    }
}