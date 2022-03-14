using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.Animations;

namespace Test
{
    [TrackClipType(typeof(BoxAsset)), TrackColor(1.0f, 1.0f, 0.0f)]
    public class CheckBoxTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return base.CreateTrackMixer(graph, go, inputCount);
        }
    }
}