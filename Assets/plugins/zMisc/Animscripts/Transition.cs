using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Transitions;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Transitions
{

    [ExecuteInEditMode]
    public class Transition : MonoBehaviour, ITransitionable
    {
        [Range(0.1f, 3f)]
        public float animationSpeed = 1;//
                                        //  public TransitionElements transitionElements;
        [ExposeMethodInEditor]
        void FillTransitionList()
        {
            adders = new List<ComponentAdder>();
            adders.Add(new ComponentAdder(typeof(TransitionApplyCanvasGroup)));
            adders.Add(new ComponentAdder(typeof(TransitionApplyMoveBetween)));
            adders.Add(new ComponentAdder(typeof(TransitionApplyDeltaSize)));
            adders.Add(new ComponentAdder(typeof(TransitionApplyScale)));

        }

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
            FillTransitionList();
            if (group == null) transform.parent.gameObject.AddComponent<TransitionGroup>();
        }
        ITransitionElement[] executors;
        public AnimStates _animState;
        public AnimStates animState { get { return _animState; } set { _animState = value; } }

        bool nextEvnetTriggered;
        [Range(0, 1)]
        public float whenToTriggerOnOut = 0.5f;
        public bool coroutineRunning;
        public List<ComponentAdder> adders;
        [ReadOnly]
        public float maxDelay;
        void OnValidate()
        {
            setPhase(phase);
            //animState=_animState;
            // if (executors == null) executors = GetComponents<ITransitionElement>();
            // transitionElements.OnValidate(this);
            if (adders == null) FillTransitionList();
            for (int i = 0; i < adders.Count; i++)
            {
                adders[i].OnValidate(gameObject);

            }
        }
        public void setPhase(float f)
        {
            phase = f;
            if (useSmoothStep)
                f = Mathf.SmoothStep(0, 1, f);
            else
                f = curve.Evaluate(f);
            if (executors == null) GetExecutors();
            for (int i = 0; i < executors.Length; i++)
                executors[i].OnAnimationPhaseChange(f);
        }
        void GetExecutors()
        {
            executors = GetComponents<ITransitionElement>();
            maxDelay = 0;
            for (int i = 0; i < executors.Length; i++)
            {
                if (executors[i].delay > maxDelay) maxDelay = executors[i].delay;

            }
        }
        public Action<ITransitionable> fadeInComplete { set { _fadeInComplete = value; } }

        public Action<ITransitionable> fadeOutComplete { set { _fadeOutComplete = value; } }

        public Action<ITransitionable> readyForNext { set { _readyForNext = value; } }

        Action<ITransitionable> _fadeInComplete;

        Action<ITransitionable> _fadeOutComplete;
        Action<ITransitionable> _readyForNext;
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
            if (!Application.isPlaying) { if (group != null) group.RequestActivation(this); return; }
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
                //              gameObject.SetActive(true);
                //                AnimateOut();
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
                    yield return new WaitForSeconds(maxDelay);
                    sendFadeInComplete();
                    animRoutine = null;
                    yield break;
                }
                else
                if (phase <= 0 && animState == AnimStates.transitionOut)
                {
                    yield return new WaitForSeconds(maxDelay);
                    sendFadeOutComplete();
                    animRoutine = null;
                    yield break;
                }
                setPhase(phase);
                yield return null;
            }
            coroutineRunning = false;
            animRoutine = null;
        }
    }
}

namespace Transitions
{
    [System.Serializable]
    public class ComponentAdder
    {
        public string name;
        Type componentType;
        public ComponentAdder(Type t)
        {
            name = t.ToString();
            componentType = t;
        }
        public bool _isPresent;
        bool __isPresent;
        public bool isPresent
        {
            get { return _isPresent; }
            set
            {
                if (!__isPresent && value)
                {
                    Debug.Log("adding");
                    _isPresent = value;
                    __isPresent = value;
                    Component c = gameObject.AddComponent(componentType);
                    //  if (c!=null) GameObject.DestroyImmediate(c);
                }
                else
                              if (__isPresent && !value)
                {
                    Debug.Log("removing");
#if UNITY_EDITOR
                    Component c = gameObject.GetComponent(componentType);
#endif
                    if (c != null) EditorApplication.delayCall += () => GameObject.DestroyImmediate(c);

                    __isPresent = value;
                    _isPresent = value;
                }

            }

        }

        GameObject gameObject;
        public void OnValidate(GameObject gameObject)
        {
            this.gameObject = gameObject;
            isPresent = _isPresent;
        }

    }
}
