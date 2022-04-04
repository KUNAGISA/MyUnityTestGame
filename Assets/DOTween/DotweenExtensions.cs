using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DotweenExtensions
{

    public static void DoTransformX(this Transform transform, float endVal, float useTime, TweenCallback callBack = null)
    {
        transform.DOMoveX(endVal, useTime).OnComplete(callBack);
    }
}
