

/// depreciated for timeline.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Z;
namespace Z {

[System.Serializable]
public enum LoopPriority { constantLength, freePoints }
[System.Serializable]
public class zLoopLogic
{
    public Action onRangeChange;
    float minDistance = 0.01f;

    public LoopPriority loopPriority;
    LoopPriority lastLoopPriority;
    float _inPoint;
    [Range(0, 1)]
    public float inPointSet;
    public float inPoint
    {
        get { return _inPoint; }
        set
        {
            if (value < 0) value = 0;
            if (value > _outPoint) value = outPoint - minDistance;
            {

                inPointSet = value;
                if (_inPoint != value)
                { changed = true;

                
                _inPoint = value;
                if (onInpointChange!=null) onInpointChange.Invoke(value);
                }


            }
        }
    }
    float _outPoint = 1;
    [Range(0, 1)]
    public float outPointSet = 1;
    public bool changed;
public float mappedValue(float f)
{
    return _inPoint+f*_deltaLoop;
}
    public float outPoint
    {
        get { return _outPoint; }
        set
        {
            if (value < _inPoint)
            {
                value = inPoint + minDistance;
            }
            if (_outPoint != value) 
            { changed = true;
              _outPoint = value;
              if (onOutpointChange!=null)
              onOutpointChange.Invoke(value);
            }
            outPointSet = value;
           
        }
    }

 

    [Range(0, 1)]
    public float deltaLoopSet = 1;
    float _deltaLoop = 1;
    public float deltaLoop
    {
        get { return _deltaLoop; }
        set
        {
            //	if (inPoint+value>1)
            //	{
            //		inPoint=1-value;
            //
            //	}
            if (value < 0) { Debug.LogWarning("trying to set negative delta "+value); value = 0; }
            if (value > 1) { Debug.LogWarning("trying to set wrong "+value); value = 1; }
            if (deltaLoop != value) 
            {
                changed = true;
            _deltaLoop = value;
            deltaLoopSet = value;
            if (onDurationChange!=null) onDurationChange.Invoke(value);
            
            }
            ///	float f=inPoint+deltaLoop;
            //	if (f>1)
            //	outPoint=f; else outPoint=1;
        }
    }

    //bool validateFlag
    [ReadOnly]
    public int validateCount;
    bool dontValidate;
    public void OnValidate()
    {  changed=false;
        validateCount++;
        if (loopPriority == LoopPriority.constantLength)
        {

            if (lastLoopPriority == LoopPriority.freePoints)
            {
                deltaLoopSet = outPoint - inPoint;
                lastLoopPriority = loopPriority;
            }
            deltaLoop = deltaLoopSet;
            inPoint = inPointSet;
            outPoint = inPoint + deltaLoop;

        }
        else
        {
            if (lastLoopPriority == LoopPriority.constantLength)
            {
                lastLoopPriority = loopPriority;
                outPointSet = outPoint + deltaLoop;
            }
            inPoint = inPointSet;
            outPoint = outPointSet;
            deltaLoop = outPoint - inPoint;
        }
        if (changed)
        {
            if (onRangeChange != null) onRangeChange.Invoke();
            changed = false;
        }
        //dontValidate=false;
        changed=false;
    }
    public FloatEvent onInpointChange;
   public FloatEvent onOutpointChange;
   public FloatEvent onDurationChange;

}
}