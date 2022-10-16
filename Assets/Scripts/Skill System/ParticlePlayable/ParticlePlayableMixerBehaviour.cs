using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ParticlePlayableMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Transform trackBinding = playerData as Transform;

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount ();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<ParticlePlayableBehaviour> inputPlayable = (ScriptPlayable<ParticlePlayableBehaviour>)playable.GetInput(i);
            ParticlePlayableBehaviour input = inputPlayable.GetBehaviour ();
            
            // Use the above variables to process each frame of this playable.
            
        }
    }
}
