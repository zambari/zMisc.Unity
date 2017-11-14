using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimExecuteMoveBetween : MonoBehaviour , IExecuteAnimation {

    public Vector3 startPos;
    public Vector3 endPos;
	[SerializeField]
    [Range(0, 1)]
    float _pos;

	void Reset()
	{
		startPos=transform.localPosition;
		endPos=transform.localPosition;
		
	}
	public void OnAnimationPhaseChange(float f)
	{
		pos=f;
	}
    void OnValidate()
    {
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
