using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{


    public abstract class AbstractsController : MonoBehaviour, IController
    {
        /// <summary>
        /// 初始化Controller
        /// </summary>
        protected virtual void Awake() { }

        /// <summary>
        /// 控制器正式执行，各个controller已经初始化完毕
        /// </summary>
        protected virtual void Start() { }

        IArchitecture IBelongArchiecture.GetArchitecture()
        {
            return Game2D.Instance;
        }
    }
}