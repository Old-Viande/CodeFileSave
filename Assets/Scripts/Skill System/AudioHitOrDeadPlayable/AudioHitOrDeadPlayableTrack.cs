using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(1f, 0.8795769f, 0f)]
[TrackClipType(typeof(AudioHitOrDeadPlayableClip))]
public class AudioHitOrDeadPlayableTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<AudioHitOrDeadPlayableMixerBehaviour>.Create (graph, inputCount);
    }
}
