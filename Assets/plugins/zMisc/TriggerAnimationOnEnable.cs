using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimationOnEnable : MonoBehaviour {

void OnDisable()
{
	StopAllCoroutines();
}
void OnEnable()
{
	StartCoroutine(Sweep());
}
public float animationLengh=3;
public float startDelay=1;
public FloatEvent sweepEvent;
public AnimationCurve animationCurve;
public bool useCurve;
float nonlinearize(float f)
{ 
	if (useCurve) return animationCurve.Evaluate(f); else return f;
}
IEnumerator Sweep()
{   sweepEvent.Invoke(nonlinearize(0));
	yield return new WaitForSeconds(startDelay);

	float startTime=Time.time;
	float r=0;
	while (r<=1)
	{
		r=(Time.time-startTime)/animationLengh;
		sweepEvent.Invoke(nonlinearize(r));
		yield return null;
	}
	sweepEvent.Invoke(nonlinearize(1));
}
}
