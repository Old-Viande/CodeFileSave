using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class ScreenshakeBehaviour : PlayableBehaviour
{
    public float shakeAdd;

    public override void OnPlayableCreate (Playable playable)
    {
        
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        PlayableDirector pd = (PlayableDirector)playable.GetGraph().GetResolver();
        foreach (var track in pd.playableAsset.outputs)
        {
            if (track.streamName== "Screenshake Track")
            {
                 ((CameraTempController)pd.GetGenericBinding(track.sourceObject)).AddShake(1f);
            }
        }
    }
}
