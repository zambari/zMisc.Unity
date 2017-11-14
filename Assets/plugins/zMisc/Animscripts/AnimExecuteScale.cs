using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimExecuteScale : MonoBehaviour,IExecuteAnimation {

	public void OnAnimationPhaseChange(float f)
	{
	//	Debug.Log("executing scale "+f,gameObject);
			transform.localScale=new Vector3(f,f,f);
	}
	
	
}
