using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TransitionBaseOptions
{
    MonoBehaviour mono;
    public TransitionBaseOptions(MonoBehaviour m)
    {
        mono = m;
    }


    public Transform _targetTransform;
    [Range(0, 4f)]
    public float _delay;
    public bool hasDelay;
    public bool useCurve;
        public AnimationCurve animationCurve;
   // public bool dumpCurve;
}
public class TransitionElementBase : MonoBehaviour, ITransitionElement
{
    public Transform targetTransform { get { if (transitionOptions != null) return transitionOptions._targetTransform; else return transform; } }
    DelayedValue delayedValue;
    public TransitionBaseOptions transitionOptions;

    [Range(0, 1)]
    public float transitionPreview = 1;
    protected virtual void OnTransitionValue(float f)
    {
        // your code goes here
    }
    public void OnAnimationPhaseChange(float f)
    {
        transitionPreview = f;
        if (delayedValue == null) initDelay();
        if (!Application.isPlaying) OnTransitionValue(f);
        else
        if (transitionOptions._delay != 0) delayedValue.QueueValue(f);
        else OnTransitionValue(f);
    }

    protected virtual void Reset()
    {
        initDelay();
        transitionOptions._targetTransform = transform;
    }
    protected virtual void OnValidate()
    {
        if (transitionOptions == null) transitionOptions = new TransitionBaseOptions(this);
        if (delayedValue == null) initDelay();
        delayedValue.delay = transitionOptions._delay;
        transitionOptions.hasDelay = transitionOptions._delay > 0;
        if (transitionOptions.useCurve && (transitionOptions.animationCurve == null || transitionOptions.animationCurve.length < 2))
        {
            transitionOptions.animationCurve = new AnimationCurve();
      // Begin AnimationCurve dump (copy from here)
                transitionOptions.animationCurve .AddKey(new Keyframe(0f,0f,0f,0f));
                transitionOptions.animationCurve .AddKey(new Keyframe(0.2102612f,0.09875421f,1.996346f,1.996346f));
                transitionOptions.animationCurve .AddKey(new Keyframe(0.4665389f,0.7166011f,1.454055f,1.454055f));
                transitionOptions.animationCurve .AddKey(new Keyframe(1f,1f,0f,0f));
// end keyframe dump (created using zExtensions)

        }
        OnAnimationPhaseChange(transitionPreview);
       /* if (transitionOptions.dumpCurve)
        {
            transitionOptions.dumpCurve = false;
            transitionOptions.animationCurve.dumpKeys();

        }*/

    }

    public float delay
    {
        get { return transitionOptions._delay; }
        set { transitionOptions._delay = value; }
    }

    void initDelay()
    {
        if (transitionOptions == null) transitionOptions = new TransitionBaseOptions(this);
        delayedValue = new DelayedValue(this);
        delayedValue.delay = transitionOptions._delay;
        delayedValue.delayedOutputAction += OnTransitionValue;
    }


}
