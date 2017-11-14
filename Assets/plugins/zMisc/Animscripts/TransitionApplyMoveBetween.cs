using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionApplyMoveBetween :TransitionElementBase {

    public Vector3 startPos;
    public Vector3 endPos;
	[SerializeField]
   [HideInInspector]
    [Range(0, 1)]
    float _pos;

	protected override  void Reset()
	{
        base.Reset();
		startPos=transform.localPosition;
		endPos=transform.localPosition;
		
	}
    protected override  void OnTransitionValue(float f)
	{
		pos=f;
	}
    protected override void  OnValidate()
    {
        base.OnValidate();
        pos = _pos;
    }

    public float pos
    {
        get { return _pos; }
        set
        {
            _pos = value;
            transform.localPosition = Vector3.Lerp(startPos, endPos, value);
        }
    }

    [ExposeMethodInEditor]
    public void saveAsStart()
    {
        startPos = transform.localPosition;
    }

    [ExposeMethodInEditor]
    public void saveAsEnd()
    {
        endPos = transform.localPosition;

    }


	
	
	
}
