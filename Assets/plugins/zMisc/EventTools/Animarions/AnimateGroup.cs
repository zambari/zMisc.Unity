using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasGroup))]
public class AnimateGroup : MonoBehaviour, IAnimmateInOut
{
    CanvasGroup canvasGroup;
    public float disabledAlpha = 0.5f;
	public float animationTime=1;
	public bool hidden;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
		hidden=canvasGroup.alpha<1;
    }
    public void animateIn()
    {
		StartCoroutine(fadeIn());
    }
    public void animateOut()
    {
		StartCoroutine(fadeOut());
    }
	public bool isHidden()
	{
		return hidden;
	}
    IEnumerator fadeIn()
	{	hidden=false;
		float startTime=Time.time;
		float phase=0;
		while (phase<1)
		{
		  phase=(Time.time-startTime)/animationTime;
		  canvasGroup.alpha=disabledAlpha+(1-disabledAlpha)*phase;
		  
		  yield return null;
		}
	}
 	IEnumerator fadeOut()
	{
		float startTime=Time.time;
		float phase=0;
		while (phase<1)
		{
		  phase=(Time.time-startTime)/animationTime;
		  canvasGroup.alpha=disabledAlpha+(1-disabledAlpha)*(1-phase);
	      yield return null;
		}
		hidden=true;
	}

}
