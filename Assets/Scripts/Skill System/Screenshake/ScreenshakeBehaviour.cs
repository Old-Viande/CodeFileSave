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
        if (SkillManagerMono.Instance.tempTargets.Count<=0)
        {
            return;
        }
        Camera.main.GetComponent<CameraTempController>().AddShake(shakeAdd);

    }
}
