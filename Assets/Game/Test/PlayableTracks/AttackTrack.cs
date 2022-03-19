using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Game.Test
{
    [Serializable, TrackColor(1.0f, 1.0f, 0.0f), DisplayName("�������")]
    [TrackClipType(typeof(IAttackClipAsset))]
    public class AttackTracks : TrackAsset
    {
    }
}