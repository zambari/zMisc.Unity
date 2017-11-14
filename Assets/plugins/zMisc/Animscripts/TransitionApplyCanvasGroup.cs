using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class TransitionApplyCanvasGroup : TransitionElementBase {

	CanvasGroup canvasGroup;
	public float minValue=0;

protected override  void OnTransitionValue(float f)
	{
		if (canvasGroup==null ) canvasGroup=GetComponent<CanvasGroup>();
		canvasGroup.alpha=f*(1-minValue)+minValue;
	}
	
	
}
