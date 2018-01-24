using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[ExecuteInEditMode]
public class RangeShow : MonoRect, IBeginDragHandler, IDragHandler,ITimelinePixel
{
    [Range(0, 1)]
    public float i;
    [Range(0, 1)]
    public float o = 1;
    [ReadOnly]
    public float dur = 1;

    public bool doNotScale;
    public bool doNotMove;
    ITimelinePixel[] children;
    public FloatEvent onDrag;

    public bool hasChildren;
    void OnEnable()
    {
        rectParent = transform.parent.GetComponent<RectTransform>();
       OnValidate();
    }
    RectTransform rectParent;

[SerializeField]
float _min;
[SerializeField]
float _max=1;
float max_min=1;
    public float minValue {
        get {return _min; }
           set { _min=value; 
           updatePixelSize();
           max_min=_max-_min; 
           }
    }

    public float maxValue {
        get {return _max; }
           set { _max=value;
           max_min=_max-_min; 
           updatePixelSize();
            }
    }

    protected void OnValidate()
    {   maxValue=_max;
        minValue=_min;
        if (o < i)
        {
            float k = i;
            i = o;
            o = k;

        }
        
        dur = o - i;

        updatePixelSize();

        children = GetComponentsInChildren<RangeShow>();
        if (children.Length > 1) hasChildren = true;
    }
    public void moveTo(float f)
    {
        i = f;
        o = i + dur;
        updatePixelSize();

    }

    public void showIn(float f)
    {
        i = f;
        dur = o - i;
        updatePixelSize();

    }
    public void showOut(float f)
    {
        o = f;
        dur = o - i;
        updatePixelSize();
    }

    float scaledValue(float i)
    {
        return i*(max_min)+_min;
    }
    public void updatePixelSize()
    {
        if (rectParent == null) rectParent = transform.parent.GetComponent<RectTransform>();

        rect.setRelativeLocalX(rectParent, scaledValue(i));

        if (!doNotScale) rect.setRelativeSizeX(rectParent, o - scaledValue(i));
        if (hasChildren)
            notifyChildren();
    }

    public void notifyChildren()
    {


        if (children == null) children = GetComponentsInChildren<ITimelinePixel>();
        for (int i = 1; i < children.Length; i++)
        {
            children[i].updatePixelSize();
        }
    }


    // @@@@@@@@@@@@ DRAG
    /*

    public enum DragDestination { position, durationFront, durationEnd, none }
    DragDestination dragDestination;*/
    Vector2 dragStartPosition;

    public void OnBeginDrag(PointerEventData e)
    {
        dragStartPosition = e.position;
    }
    public void OnDrag(PointerEventData e)
    {
        float dragX = (e.position.x - dragStartPosition.x) / (Screen.width*2);
        onDrag.Invoke(dragX);
     //  Debug.Log("drg" + dragX);
    }


}
