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
        }

        private static readonly Dictionary<ViewName, string> ViewDefineMap = new Dictionary<ViewName, string>
        {
            {ViewName.TestView, "Prefabs/Views/TestView.prefab" },
            {ViewName.PauseView, "Prefabs/Views/PauseView.prefab" },
        };

        public static string GetViewPath(ViewName viewName) {
            if (!ViewDefineMap.TryGetValue(viewName, out var viewPath)) {
                Debug.LogWarning("未定义[" + viewName + "]界面的路径");
            }
            return viewPath;
        }

        public static string GetUIPath(ViewName viewName) {
            var realViewName = GetViewPath(viewName);
            var realUIName = realViewName.Replace("Prefabs/Views/", "").Replace(".prefab", "");
            var arr = realUIName.Split('@');
            if (arr == null || arr.Length < 1) {
                Debug.LogWarning("[Nick] 预制体UI名 规范错误 需要添加 @ xxx");
                return null;
            }
            var uiFile = arr[1];
            string path = Application.dataPath + "/UICode/" + uiFile + "/" + realViewName;
            if (Directory.Exists(path)) {

            }
            else {
                Debug.LogWarning("[Nick] 请使用UI 代码工具");
            }
            return null;
        }

    }
}