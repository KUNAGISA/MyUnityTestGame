using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public abstract class AbstractsController : MonoBehaviour, IController
    {
        IArchitecture IBelongArchiecture.GetArchitecture()
        {
            return Game2D.Instance;
        }
    }
}