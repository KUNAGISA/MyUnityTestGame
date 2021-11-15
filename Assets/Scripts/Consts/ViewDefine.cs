using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game
{
    public static class ViewDefine
    {
        public enum ViewName
        { 
            TestView,
            PauseView,
        }

        private static readonly Dictionary<ViewName, string> ViewDefineMap = new Dictionary<ViewName, string>
        {
            {ViewName.TestView, "Prefabs/Views/TestView.prefab" },
            {ViewName.PauseView, "Prefabs/Views/PauseView.prefab" },
        };

        public static string GetViewPath(ViewName viewName)
        {
            if (!ViewDefineMap.TryGetValue(viewName, out var viewPath))
            {
                Debug.LogWarning("未定义[" + viewName + "]界面的路径");
            }
            return viewPath;
        }
    }
}