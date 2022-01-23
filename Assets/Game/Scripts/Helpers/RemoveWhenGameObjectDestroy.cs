using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Game
{
    [DisallowMultipleComponent]
    public sealed class RemoveWhenGameObjectDestroy : MonoBehaviour
    {
        private List<IUnRegister> m_UnRegisterList = new List<IUnRegister>();
        public void AddUnRegister(IUnRegister unRegister) => m_UnRegisterList.Add(unRegister);

        private List<ITimer> m_TimerList = new List<ITimer>();
        public void AddTimer(ITimer timer) => m_TimerList.Add(timer);

        private void OnDestroy()
        {
            foreach(var unRegister in m_UnRegisterList)
            {
                unRegister.UnRegister();
            }
            m_UnRegisterList.Clear();

            foreach(var timer in m_TimerList)
            {
                timer.Kill();
            }
            m_TimerList.Clear();
        }
    }

    public static class RemoveWhenGameObjectExtension
    { 
        private static RemoveWhenGameObjectDestroy GetRemoveWhenGameObjectDestroy(GameObject gameObject)
        {
            var autoDestroy = gameObject.GetComponent<RemoveWhenGameObjectDestroy>();
            if (autoDestroy == null)
            {
                autoDestroy = gameObject.AddComponent<RemoveWhenGameObjectDestroy>();
            }
            return autoDestroy;
        }

        public static void UnRegisterWhenGameObjectDestroy(this IUnRegister self, GameObject gameObject)
        {
            GetRemoveWhenGameObjectDestroy(gameObject)
                .AddUnRegister(self);
        }

        public static void KillWhenGameObjectDestroy(this ITimer self, GameObject gameObject)
        {
            GetRemoveWhenGameObjectDestroy(gameObject)
                .AddTimer(self);
        }
    }
}