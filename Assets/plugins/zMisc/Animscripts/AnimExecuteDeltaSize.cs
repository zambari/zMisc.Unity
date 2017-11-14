using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimExecuteDeltaSize : MonoBehaviour, IExecuteAnimation
{

    public Vector2 startSize;
    public Vector2 endSize;
    public Vector2 startPos;
    public Vector2 endPos;
	  public Vector3 startRot;
	  public Vector3 endRot;
    public bool useAnchoredPosition;
    public bool usePosition;

    public bool useRotation;
    [SerializeField]
    [Range(0, 1)]
    float _pos;
    [SerializeField]
    [HideInInspector]
    RectTransform rect;

    void Reset()
    {
        rect = GetComponent<RectTransform>();
        startSize = rect.sizeDelta;
        endSize = rect.sizeDelta;
        if (useAnchoredPosition)
        {
            startPos = rect.anchoredPosition;
            endPos = rect.anchoredPosition;
        }
        else
        {
            startPos = rect.localPosition;
            endPos = rect.localPosition;
        }
		startRot=rect.eulerAngles;
		endRot=rect.eulerAngles;
    }
    public void OnAnimationPhaseChange(float f)
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        pos = f;

    }
    void OnValidate()
    {	if (usePosition&&useAnchoredPosition) useAnchoredPosition=false;
        pos = _pos;
    }

    public float pos
    {
        get { return _pos; }
        set
        {
            _pos = value;
            rect.sizeDelta = Vector2.Lerp(startSize, endSize, value);
            if (useAnchoredPosition)
            {

                rect.anchoredPosition = Vector2.Lerp(startPos, endPos, value);
            } else
			if (usePosition)
			{

                rect.localPosition = Vector2.Lerp(startPos, endPos, value);
			}
			if (useRotation)
			{
				rect.eulerAngles=Vector3.Lerp(startRot,endRot,value);
			}
        }
    }

    [ExposeMethodInEditor]
    public void saveAsStart()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        startSize = rect.sizeDelta;
        startPos = rect.anchoredPosition;
        if (useAnchoredPosition)
        {
            startPos = rect.anchoredPosition;
        }
        else
        {
            startPos = rect.localPosition;
        }
				startRot=rect.eulerAngles;
    }


    [ExposeMethodInEditor]
    public void saveAsEnd()
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        endSize = rect.sizeDelta;
        endPos = rect.anchoredPosition;
        if (useAnchoredPosition)
        {
            endPos = rect.anchoredPosition;
        }
        else
        {
            endPos = rect.localPosition;
        }
	
		endRot=rect.eulerAngles;
    }
}


	
	