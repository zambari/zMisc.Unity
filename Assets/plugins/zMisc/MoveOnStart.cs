using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnStart : MonoBehaviour {

	// Use this for initialization
	public float animTime=4;
	MoveBetween move;
	void Start () {
		move=GetComponent<MoveBetween>();
		StartCoroutine(anim());
	}


IEnumerator anim()
{
	float t=Time.time;
	float r=0;
	while (r<1)
	{
		move.pos=(Time.time-t)/animTime;
		yield return null;
	}
}
	
	
}
