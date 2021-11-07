using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Manager.View
{
    public interface IViewManager
    {
        public void Push(ViewDefine.ViewName viewName);

        public void Pop(ViewDefine.ViewName viewName);

        public void Pop(IView view);
    }

    public class ViewManager : AbstractsController, IViewManager
    {
        [SerializeField]
        private BaseViewPanel m_ViewPanel;

        public void Pop(ViewDefine.ViewName viewName)
        {
            //Addressables.LoadAssetAsync<GameObject>("aaaaa");
        }

        public void Pop(IView view)
        {
            
            throw new global::System.NotImplementedException();
        }

        public void Push(ViewDefine.ViewName viewName)
        {
            throw new global::System.NotImplementedException();
        }
    }
}