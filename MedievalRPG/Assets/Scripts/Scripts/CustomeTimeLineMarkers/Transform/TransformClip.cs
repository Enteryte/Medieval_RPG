using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public class TransformClip : PlayableAsset
{
    public ExposedReference<Transform> startLocation;
    public ExposedReference<Transform> endLocation;

    public bool shouldTweenPosition = true;
    public bool shouldTweenRotation = true;

    public AnimationCurve curve;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        // create a new TweenBehaviour
        ScriptPlayable<TransformBehaviour> playable = ScriptPlayable<TransformBehaviour>.Create(graph);
        TransformBehaviour tween = playable.GetBehaviour();

        // set the behaviour's data
        tween.startLocation = startLocation.Resolve(graph.GetResolver());
        tween.endLocation = endLocation.Resolve(graph.GetResolver());
        tween.curve = curve;
        tween.shouldTweenPosition = shouldTweenPosition;
        tween.shouldTweenRotation = shouldTweenRotation;

        return playable;
    }
}
