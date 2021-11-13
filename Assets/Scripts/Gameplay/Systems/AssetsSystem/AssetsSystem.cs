using Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.System
{
    /// <summary>
    /// 资源缓存系统
    /// </summary>
    public interface IAssetsSystem : ISystem
    {
        public bool IsLoadedAssets(string path);

        public Task LoadAssetsAsync<TObject>(string path) where TObject : UnityEngine.Object;

        public TObject GetAssets<TObject>(string path) where TObject : UnityEngine.Object;

        public Task<TObject> GetAssetsAsync<TObject>(string path) where TObject : UnityEngine.Object;

        public void Cleanup();
    }

    public class AssetsSystem : AbstractSystem, IAssetsSystem
    {
        private readonly Dictionary<string, UnityEngine.Object> m_AssetCacheMap = new Dictionary<string, UnityEngine.Object>();

        protected override void OnInitSystem()
        {
            Cleanup();
        }

        public void Cleanup()
        {
            var assetsUtility = this.GetUtility<Utility.IAssetsUtility>();
            foreach(var pair in m_AssetCacheMap)
            {
                assetsUtility.ReleaseAsset(pair.Value);
            }
            m_AssetCacheMap.Clear();
            Resources.UnloadUnusedAssets();
        }

        public async Task LoadAssetsAsync<TObject>(string path) where TObject : UnityEngine.Object
        {
            if (!m_AssetCacheMap.ContainsKey(path))
            {
                var assetsUtility = this.GetUtility<Utility.IAssetsUtility>();
                var obj = await assetsUtility.LoadAssetAsync<TObject>(path);
                m_AssetCacheMap[path] = obj;
            }
        }

        public TObject GetAssets<TObject>(string path) where TObject : UnityEngine.Object
        {
            if (!m_AssetCacheMap.TryGetValue(path, out var obj))
            {
                var assetsUtility = this.GetUtility<Utility.IAssetsUtility>();
                obj = assetsUtility.LoadAsset<TObject>(path);
                m_AssetCacheMap[path] = obj;
            }
            return obj as TObject;
        }

        public async Task<TObject> GetAssetsAsync<TObject>(string path) where TObject : UnityEngine.Object
        {   
            if (!m_AssetCacheMap.TryGetValue(path, out var obj))
            {
                /// AssetsUtility具体实现都带缓存，所以这里就暂时不做缓存task了
                var assetsUtility = this.GetUtility<Utility.IAssetsUtility>();
                obj = await assetsUtility.LoadAssetAsync<TObject>(path);
                m_AssetCacheMap[path] = obj;
            }
            return obj as TObject;
        }

        public bool IsLoadedAssets(string path)
        {
            return m_AssetCacheMap.ContainsKey(path);
        }
    }
}