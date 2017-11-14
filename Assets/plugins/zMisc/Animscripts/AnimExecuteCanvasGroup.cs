using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class AnimExecuteCanvasGroup : MonoBehaviour, IExecuteAnimation {

	CanvasGroup canvasGroup;
	public float minValue=0;

	public void OnAnimationPhaseChange(float f)
	{
		if (canvasGroup==null ) canvasGroup=GetComponent<CanvasGroup>();
		canvasGroup.alpha=f*(1-minValue)+minValue;
	}
	
	
}
