using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ITransitionable
{

    Action<ITransitionable> fadeInComplete { set; }
    Action<ITransitionable> fadeOutComplete { set; }
    Action<ITransitionable> readyForNext { set; }

    string name { get; }
    GameObject gameObject { get; }
    void InstantIn();
    void AnimateIn();
    void AnimateOut();

}
public interface ITransitionElement
{
    void OnAnimationPhaseChange(float f);
    float delay {get;}
}