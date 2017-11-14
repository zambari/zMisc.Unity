using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionProxy : TransitionElementBase {

public GameObject targetTransition;
	// Use this for initialization
	ITransitionElement[] targets;
	[ReadOnly]
	public int foundElements;

protected override void OnValidate()
{
	base.OnValidate();
	targets=targetTransition.GetComponents<ITransitionElement>();
	if (targets.Length==0) targetTransition=null;
	foundElements=targets.Length;
}	
	// Update is called once per frame
	  protected override void OnTransitionValue(float f)
    {
		if (targetTransition==null) return;
		if (targets==null) 	targets=targetTransition.GetComponents<ITransitionElement>();

		for (int i=0;i<targets.Length;i++)
		targets[i].OnAnimationPhaseChange(f);

    }
}
