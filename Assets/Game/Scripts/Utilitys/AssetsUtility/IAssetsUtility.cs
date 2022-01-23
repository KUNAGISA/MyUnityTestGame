using Framework;
using System.Threading.Tasks;

namespace Game.Utility
{
    /// <summary>
    /// 资源管理工具
    /// </summary>
    public interface IAssetsUtility : IUtility
    {
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <returns>资源</returns>
        TObject LoadAsset<TObject>(string path) where TObject : UnityEngine.Object;

        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="path">资源路径</param>
        /// <returns>加载任务</returns>
        Task<TObject> LoadAssetAsync<TObject>(string path) where TObject : UnityEngine.Object;

        /// <summary>
        /// 释放资源
        /// </summary>
        /// <typeparam name="TObject">资源类型</typeparam>
        /// <param name="obj">资源类型</param>
        void ReleaseAsset<TObject>(TObject obj) where TObject : UnityEngine.Object;
    }
}
