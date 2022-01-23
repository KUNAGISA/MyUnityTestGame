using Framework;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Game.Utility
{
    /// <summary>
    /// Resources资源管理工具
    /// </summary>
    public class ResourcesAssetsUtility : AbstractUtility, IAssetsUtility
    {
        /// <summary>
        /// 用来干掉资源后缀
        /// </summary>
        private static readonly Regex suffix = new Regex(@"\.\w+$");

        private string GetResPath(in string path) => suffix.Replace(path, "");

        public TObject LoadAsset<TObject>(string path) where TObject : Object
        {
            var fullPath = GetResPath(path);
            return Resources.Load<TObject>(fullPath);
        }

        public async Task<TObject> LoadAssetAsync<TObject>(string path) where TObject : Object
        {
            var tcs = new TaskCompletionSource<TObject>();
            var request = Resources.LoadAsync<TObject>(GetResPath(path));
            request.completed += (operation) => tcs.TrySetResult(request.asset as TObject);
            return await tcs.Task;
        }

        public void ReleaseAsset<TObject>(TObject obj) where TObject : UnityEngine.Object
        {
            /// Resources好像没有手动释放资源的接口
        }
    }
}
