using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Timeline;

namespace Test
{
    [AddComponentMenu("AAA/TTT")]
    public class TTT : MonoBehaviour, ITimeControl
    {
        void ITimeControl.OnControlTimeStart()
        {
        }

        void ITimeControl.OnControlTimeStop()
        {
        }

        void ITimeControl.SetTime(double time)
        {
            Debug.Log(time);
        }
    }
}
