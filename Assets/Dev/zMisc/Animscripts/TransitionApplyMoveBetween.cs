using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class TransitionApplyMoveBetween : TransitionElementBase
{

    public Vector3 startPos;
    public Vector3 endPos;
    
    public Vector3 current;

    [SerializeField]
    [HideInInspector]
    [Range(0, 1)]
    float _pos;

    protected override void Reset()
    {
        base.Reset();
        startPos = targetTransform.localPosition;
        endPos = targetTransform.localPosition;

    }
    protected override void OnTransitionValue(float f)
    {
        pos = f;
    }
    protected override void OnValidate()
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
            targetTransform.localPosition = Vector3.Lerp(startPos, endPos, value);
        }
    }

    [ExposeMethodInEditor]
    public void saveAaZero()
    {
        startPos = targetTransform.localPosition;
        pos=0;
    }

    [ExposeMethodInEditor]
    public void saveAsOne()
    {
        endPos = targetTransform.localPosition;
        pos=1;

    }
    void Update()
    {
        current = targetTransform.localPosition;
    }




}
