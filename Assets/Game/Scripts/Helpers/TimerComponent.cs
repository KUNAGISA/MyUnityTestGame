using System;
using UnityEngine;

namespace Game
{
    public class TimerComponent : MonoBehaviour
    {
        public event Action OnTick;

        private void Update()
        {
            OnTick?.Invoke();
        }
    }
}