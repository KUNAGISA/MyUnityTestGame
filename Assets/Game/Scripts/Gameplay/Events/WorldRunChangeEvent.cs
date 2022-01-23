using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Event
{
    public struct WorldRunChangeEvent
    {
        public readonly bool isPause;

        public WorldRunChangeEvent(bool isPuaseWorld)
        {
            isPause = isPuaseWorld;
        }
    }
}