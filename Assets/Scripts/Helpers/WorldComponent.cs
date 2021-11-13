using System;
using UnityEngine;

namespace Game
{
    public class WorldComponent : MonoBehaviour
    {
        public event Action onFrameTick;

        public event Action onFixedTick;

        private void FixedUpdate()
        {
            onFixedTick?.Invoke();
        }

        private void Update()
        {
            onFrameTick?.Invoke();
        }
    }
}
