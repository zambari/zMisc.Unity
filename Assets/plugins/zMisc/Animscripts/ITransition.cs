using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ITransition
{

    Action<ITransition> fadeInComplete { set; }
    Action<ITransition> fadeOutComplete { set; }
    Action<ITransition> readyForNext { set; }

    string name { get; }
    GameObject gameObject { get; }
    void InstantIn();
    void AnimateIn();
    void AnimateOut();

}
public interface IExecuteAnimation
{
    void OnAnimationPhaseChange(float f);
}