﻿using System;
using Framework;
using UnityEngine;
using System.Text.RegularExpressions;

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

        public TObject LoadAsset<TObject>(string path) where TObject : UnityEngine.Object
        {
            var fullPath = GetResPath(path);
            return Resources.Load<TObject>(fullPath);
        }

        public void LoadAssetAsync<TObject>(string path, Action<TObject> onLoadFinishCallback) where TObject : UnityEngine.Object
        {
            var fullPath = GetResPath(path);
            var request = Resources.LoadAsync<TObject>(fullPath);
            if (request.isDone)
            {
                onLoadFinishCallback(request.asset as TObject);
            }
            else
            {
                request.completed += (result) => onLoadFinishCallback(request.asset as TObject);
            }
        }

        public void ReleaseAsset<TObject>(TObject obj) where TObject : UnityEngine.Object
        {
            /// Resources好像没有手动释放资源的接口
        }
    }
}
