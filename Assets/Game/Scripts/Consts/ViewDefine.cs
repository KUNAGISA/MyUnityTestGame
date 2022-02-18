using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
namespace Game {
    public static class ViewDefine {
        public enum ViewName {
            TestView,
            PauseView,

            UIAnalysis_TestView,
        }

        private static readonly Dictionary<ViewName, string> ViewDefineMap = new Dictionary<ViewName, string>
        {
            {ViewName.TestView, "Prefabs/Views/TestView.prefab" },
            {ViewName.PauseView, "Prefabs/Views/PauseView.prefab" },
            {ViewName.UIAnalysis_TestView, "Prefabs/Views/TestUI@Test.prefab" },

        };

        public static string GetViewPath(ViewName viewName) {
            if (!ViewDefineMap.TryGetValue(viewName, out var viewPath)) {
                Debug.LogWarning("未定义[" + viewName + "]界面的路径");
            }
            return viewPath;
        }

        public static string GetUICodePath(ViewName viewName) {
            var viewPrefabPath = GetViewPath(viewName);
            if (string.IsNullOrEmpty(viewPrefabPath)) {
                return null;
            }
            var realUIName = viewPrefabPath.Replace("Prefabs/Views/", "").Replace(".prefab", "");
            var arr = realUIName.Split('@');
            if (arr == null || arr.Length < 1) {
                Debug.LogWarning("[Nick] 预制体UI名 规范错误 需要添加 @ xxx");
                return null;
            }
            var uiCode = arr[0];
            var uiFile = arr[1];
            string path = Application.dataPath + string.Format("/UICode/Module/{0}/UIGen/{1}.cs", uiFile, uiCode);
            if (File.Exists(path)) {
                Debug.Log("[Nick] uiCode Path" + path);
                return path;
            }
            else {
                Debug.LogWarning("[Nick] 请使用UI 代码工具");
            }
            return null;
        }

    }
}