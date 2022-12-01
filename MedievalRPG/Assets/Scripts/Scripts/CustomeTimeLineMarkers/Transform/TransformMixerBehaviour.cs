using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TransformMixerBehaviour : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        //int inputCount = playable.GetInputCount();
        //for (int i = 0; i < inputCount; i++)
        //{
        //    // get the input connected to the mixer
        //    Playable input = playable.GetInput(i);

        //    // get the weight of the connection
        //    float inputWeight = playable.GetInputWeight(i);

        //    // get the clip's behaviour
        //    TransformBehaviour tweenInput = GetTweenBehaviour(input);

        //    trackBinding.position = accumPosition + m_InitialPosition * (1.0f - totalPositionWeight);
        //    trackBinding.rotation = accumRotation.Blend(m_InitialRotation, 1.0f - totalRotationWeight);
        //}
    }
}
