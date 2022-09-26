using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ScreenshakeMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        CameraTempController trackBinding = playerData as CameraTempController;

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount ();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<ScreenshakeBehaviour> inputPlayable = (ScriptPlayable<ScreenshakeBehaviour>)playable.GetInput(i);
            ScreenshakeBehaviour input = inputPlayable.GetBehaviour ();
            
            // Use the above variables to process each frame of this playable.
            
        }
    }
}
