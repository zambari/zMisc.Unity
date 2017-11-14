using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class TransitionBase : MonoBehaviour, ITransition
{
    [Range(0.1f, 3f)]
    public float animationSpeed = 1;

    public enum AnimStates { inactive, pendingShow, transitionIn, active, transitionOut }

    [Range(0, 1)]
    public float phase;
    public TransitionGroup group;
    public AnimationCurve curve;
    public bool useSmoothStep = true;

    void Reset()
    {
        curve = new AnimationCurve();
        curve.AddKey(0, 0);
        curve.AddKey(1, 1);
        group = GetComponentInParent<TransitionGroup>();
        if (group == null) transform.parent.gameObject.AddComponent<TransitionGroup>();
    }
    IExecuteAnimation[] executors;
    public AnimStates _animState;
    public AnimStates animState { get { return _animState; } set { _animState = value; } }

    bool nextEvnetTriggered;
    [Range(0, 1)]
    public float whenToTriggerOnOut = 0.5f;
    public bool coroutineRunning;

    void OnValidate()
    {
        setPhase(phase);
        //animState=_animState;
        if (executors == null) executors = GetComponents<IExecuteAnimation>();
    }
    public void setPhase(float f)
    {
        phase = f;
        if (useSmoothStep)
            f = Mathf.SmoothStep(0, 1, f);
        else
            f = curve.Evaluate(f);
        if (executors == null) executors = GetComponents<IExecuteAnimation>();
        for (int i = 0; i < executors.Length; i++)
            executors[i].OnAnimationPhaseChange(f);
    }
    public Action<ITransition> fadeInComplete { set { _fadeInComplete = value; } }

    public Action<ITransition> fadeOutComplete { set { _fadeOutComplete = value; } }

    public Action<ITransition> readyForNext { set { _readyForNext = value; } }

    Action<ITransition> _fadeInComplete;

    Action<ITransition> _fadeOutComplete;
    Action<ITransition> _readyForNext;
    Coroutine animRoutine;
    public void InstantIn()
    {
        gameObject.SetActive(true);
        setPhase(1);
        animState = AnimStates.active;
    }

    public void AnimateIn()
    {
        gameObject.SetActive(true);

        if (phase < 1) animState = AnimStates.transitionIn;
        if (animRoutine == null) animRoutine = StartCoroutine(AnimatorCoroutine());

        Debug.Log(name + " animating in");
    }
    public void AnimateOut()
    {
        if (!isActiveAndEnabled) sendFadeOutComplete();
        nextEvnetTriggered = false;

        if (phase > 0) animState = AnimStates.transitionOut;

        if (animRoutine == null) animRoutine = StartCoroutine(AnimatorCoroutine());

        Debug.Log(name + " animating out");
    }
    void OnEnable()
    {
        if (group == null) group = GetComponentInParent<TransitionGroup>();
        if (!Application.isPlaying) { group.RequestActivation(this); return; }
        setPhase(0);

        if (!group.readyForTransition) gameObject.SetActive(false);
        else
        {
            animState = AnimStates.pendingShow;
            group.RequestActivation(this);
        }

    }

    void OnDisable()
    {
        if (!Application.isPlaying) return;
        if (animState != AnimStates.inactive)
        {
            gameObject.SetActive(true);
            AnimateOut();
        }

    }
    void sendFadeInComplete()
    {
        phase = 1;
        animState = AnimStates.active;
        if (_fadeInComplete != null)
        {
            _fadeInComplete.Invoke(this);
            Debug.Log("sent fade complete");
        }
        _fadeInComplete = null;
    }
    void sendFadeOutComplete()
    {
        phase = 0;
        animState = AnimStates.inactive;
        if (_fadeOutComplete != null) _fadeOutComplete.Invoke(this);
        _fadeOutComplete = null;
        gameObject.SetActive(false);
    }
    void sendFadeReadyForNext()
    {
        if (_readyForNext != null) _readyForNext.Invoke(this);
        _readyForNext = null;
    }

    IEnumerator AnimatorCoroutine()
    {
        coroutineRunning = true;

        while (animState == AnimStates.pendingShow) yield return null;
        while ((animState == AnimStates.transitionIn) || (animState == AnimStates.transitionOut))
        {

            if (animState == AnimStates.transitionIn)
            {
                phase += animationSpeed * Time.deltaTime;
            }
            if (animState == AnimStates.transitionOut)
            {
                phase -= animationSpeed * Time.deltaTime;
                if (phase <= whenToTriggerOnOut && !nextEvnetTriggered)
                {
                    sendFadeReadyForNext();
                    nextEvnetTriggered = true;
                }
            }
            if (phase >= 1 && animState == AnimStates.transitionIn)
            {
                sendFadeInComplete();
                animRoutine = null;
                yield break;
            }
            else
            if (phase <= 0 && animState == AnimStates.transitionOut)
            {
                sendFadeOutComplete();
                animRoutine = null;
                yield break;
            }
            setPhase(phase);
            yield return null;
        }
        coroutineRunning = false;
        animRoutine = null;
        Debug.Log("coroutine finsihed");
    }

}
