using System;
using UnityEngine;

namespace Game
{
    public class WorldComponent : MonoBehaviour
    {
        public event Action onFrameTick;

        public event Action onFixedTick;

        public event Action onLateFrameTick;

        private void FixedUpdate()
        {
            onFixedTick?.Invoke();
        }

        private void Update()
        {
            onFrameTick?.Invoke();
        }

        private void LateUpdate()
        {
            onLateFrameTick?.Invoke();
        }
    }
}
