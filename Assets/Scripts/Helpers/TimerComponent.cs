using System;
using UnityEngine;

namespace Game
{
    public class TimerComponent : MonoBehaviour
    {
        public event Action OnUpdateCallback;

        private void Update()
        {
            OnUpdateCallback?.Invoke();
        }
    }
}