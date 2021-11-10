using System.Collections;
using UnityEngine;

namespace Game.System.Timer
{
    public interface ITimer
    {
        public bool IsInvalid { get; }

        public void Kill();

        public void Pause();

        public void Resume();
    }
}