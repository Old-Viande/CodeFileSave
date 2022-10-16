using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0f, 0f, 0f)]
[TrackClipType(typeof(DamageCheckClip))]
public class DamageCheckTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<DamageCheckMixerBehaviour>.Create (graph, inputCount);
    }
}
