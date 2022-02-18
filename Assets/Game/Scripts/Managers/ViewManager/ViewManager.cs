using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game.Manager.View
{
    public interface IViewManager
    {
        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="viewName">界面名字</param>
        public void Pop(ViewDefine.ViewName viewName);

        /// <summary>
        /// 关闭界面
        /// </summary>
        /// <param name="view">界面实例</param>
        public void Pop(IView view);

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="viewName">界面名字</param>
        public void Push(ViewDefine.ViewName viewName);

        /// <summary>
        /// 是否界面已经显示
        /// </summary>
        /// <param name="viewName">界面名字</param>
        /// <returns></returns>
        public bool IsShowView(ViewDefine.ViewName viewName);
    }

    public class ViewManager : BaseMonoController, IViewManager
    {
        [SerializeField]
        private BaseViewPanel m_ViewPanel;

        [SerializeField]
        private GameObject m_LoadAsyncMask;

        private readonly Queue<ViewDefine.ViewName> m_PushViewQueue = new Queue<ViewDefine.ViewName>();

        protected void Awake()
        {
            this.GetSystem<System.IViewSystem>().BindViewRoot(this);
            m_LoadAsyncMask.SetActive(false);
        }

        public void Pop(ViewDefine.ViewName viewName)
        {
            m_ViewPanel.Pop(viewName);
        }

        public void Pop(IView view)
        {
            m_ViewPanel.Pop(view);
        }

        public async void Push(ViewDefine.ViewName viewName)
        {
            var viewPath = ViewDefine.GetViewPath(viewName);
            if (viewPath == null || viewPath.Length <= 0)
            {
                return;
            }

            m_PushViewQueue.Enqueue(viewName);
            CheckLoadAsyncMask();

            var assetsSystem = this.GetSystem<System.IAssetsSystem>();
            await assetsSystem.LoadAssetsAsync<GameObject>(viewPath);

            CheckViewQueue();
        }

        public bool IsShowView(ViewDefine.ViewName viewName)
        {
            if (m_PushViewQueue.Contains(viewName))
            {
                return true;
            }
            return m_ViewPanel.IsShowView(viewName);
        }

        /// <summary>
        /// 检查界面队列是否能打开
        /// </summary>
        private void CheckViewQueue()
        {
            var queueCount = m_PushViewQueue.Count;
            if (queueCount <= 0)
            {
                return;
            }

            var assetsSystem = this.GetSystem<System.IAssetsSystem>();
            while(--queueCount >= 0)
            {
                var viewName = m_PushViewQueue.Peek();
                var viewPath = ViewDefine.GetViewPath(viewName);

                if (!assetsSystem.IsLoadedAssets(viewPath))
                {
                    break;
                }

                m_PushViewQueue.Dequeue();

                var prefab = assetsSystem.GetAssets<GameObject>(viewPath);
                var view = Instantiate(prefab).GetComponent<IView>();
                view.SetManager(this);
                view.SetViewName(viewName);
                m_ViewPanel.Push(view);
            }
            CheckLoadAsyncMask();
        }

        /// <summary>
        /// 检查是否要打开异步加载遮罩
        /// </summary>
        private void CheckLoadAsyncMask()
        {
            var queueCount = m_PushViewQueue.Count;
            m_LoadAsyncMask.SetActive(queueCount > 0);
        }
    }
}