using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.8117647f, 0.2671887f, 0.02745096f)]
[TrackClipType(typeof(AnimTargetPlayHitOrDeadClip))]
public class AnimTargetPlayHitOrDeadTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<AnimTargetPlayHitOrDeadMixerBehaviour>.Create (graph, inputCount);
    }
}
