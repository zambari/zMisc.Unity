using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class TransitionApplyDeltaSize : TransitionElementBase
{
    [Header("Deltaisze")]
    public Vector2 startSize;
    public Vector2 endSize;
    [Header("position")]
    public Vector2 startPos;
    public Vector2 endPos;
    /* 
    [Header("anchors//notimplementd")]
    public Vector2 minStart;
    public Vector2 minEnd;
    */
    //[Header("rotation")]
    [SerializeField]
    Vector3 startRot;
    [SerializeField]
    Vector3 endRot;
    public bool useAnchoredPosition;
    public bool ignorePosition;
    public bool useRotation;
    float _pos = -1;
    [SerializeField]
    [HideInInspector]
    RectTransform rect;
    [Header("Realtime")]
    public Vector3 currentPosition;
    public Vector3 currentDelta;
    void Update()
    {
        if (transform.hasChanged)
        {
            currentPosition = rect.localPosition;
            if (useAnchoredPosition)
                currentPosition = rect.anchoredPosition;
            currentDelta = rect.sizeDelta;
        }
    }
    void Start()
    {
        enabled = false; //stop udpates
    }
    void Reset()
    {
        rect = GetComponent<RectTransform>();

        if (targetTransform != null && targetTransform.gameObject != null) rect = targetTransform.gameObject.GetComponent<RectTransform>();
        if (rect == null) rect = GetComponent<RectTransform>();
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
        startRot = rect.eulerAngles;
        endRot = rect.eulerAngles;
    }
    protected override void OnTransitionValue(float f)
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        pos = f;

    }
    protected override void OnValidate()
    {
        base.OnValidate();
        if ((!ignorePosition) && useAnchoredPosition) useAnchoredPosition = false;
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
            }
            else
            if (!ignorePosition)
            {

                rect.localPosition = Vector2.Lerp(startPos, endPos, value);
            }
            if (useRotation)
            {
                rect.eulerAngles = Vector3.Lerp(startRot, endRot, value);
            }
        }
    }

    [ExposeMethodInEditor]
    public void SaveAsZero()
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
        startRot = rect.eulerAngles;
        transitionPreview = 0;
    }


    [ExposeMethodInEditor]
    public void SaveAsOne()
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

        endRot = rect.eulerAngles;
        transitionPreview = 1;
    }
}



