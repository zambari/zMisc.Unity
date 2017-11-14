using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TransitionBaseOptions
{
    public Transform _targetTransform;
    [Range(0, 4f)]
    public float _delay;
    public bool hasDelay;
}
public class TransitionElementBase : MonoBehaviour, ITransitionElement
{
    [SerializeField]
    //[HideInInspector]
    public Transform targetTransform { get { return transitionOptions._targetTransform; } }

[Range(0,1)]
public float transitionPreview;
    protected virtual void OnTransitionValue(float f)
    {

    }
    public void OnAnimationPhaseChange(float f)
    {
        transitionPreview=f;
        if (delayedValue == null) initDelay();
        if (!Application.isPlaying) OnTransitionValue(f);
        else
        if (transitionOptions._delay != 0 ) delayedValue.QueueValue(f);
        else  OnTransitionValue(f);
    }

    DelayedValue delayedValue;

    public TransitionBaseOptions transitionOptions;

    protected virtual void Reset()
    {
     
        initDelay();
        transitionOptions._targetTransform = transform;
    }
    protected virtual void OnValidate()
    {   if (delayedValue==null) initDelay();
        delayedValue.delay = transitionOptions._delay;
        transitionOptions.hasDelay = transitionOptions._delay > 0;
        OnAnimationPhaseChange(transitionPreview);
    }
    public float delay
    {
        get { return transitionOptions._delay; }
        set { transitionOptions._delay = value; }
    }

    void initDelay()
    {   if (transitionOptions==null) transitionOptions=new TransitionBaseOptions();
        delayedValue = new DelayedValue(this);
        delayedValue.delay = transitionOptions._delay;
        delayedValue.delayedOutputAction += OnTransitionValue;
    }


}
