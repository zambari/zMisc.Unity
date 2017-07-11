using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[ExecuteInEditMode]
public class LayoutHelper : MonoRect//, ISyncColors, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{



    
/* 
    public LayoutElement layoutElement;
   LayoutElement _myLayoutElement;
     LayoutElement myLayoutElement {
get {
    if (_myLayoutElement==null)
    _myLayoutElement=GetComponent<LayoutElement>();
    if (_myLayoutElement==null)
    _myLayoutElement=gameObject.AddComponent<LayoutElement>();
    return _myLayoutElement;
}
     }
    public VerticalLayoutGroup verticalGroup;
    public HorizontalLayoutGroup horizontalGroup;
    public bool horizontal;
    public bool vertical;
    //public bool setToChildControls;
    public Slider slider;

    public float flexSum;
    public LayoutElement[] siblings;
    public HoverableColors colors;

    Vector3 dragStartPosition;

    public long randomId;

    public float[] otherFlex;
    LayoutElement nextElement;
    LayoutElement previousElement;
    void Reset()
    {
        getSibling();
    }
    void updateColors()
    {
        image.color = colors.normalColor;
    }
    public void setColors(HoverableColors c)
    {
        if (c != null)
        {
            colors = c;

            colors.OnChange.AddListener(updateColors);
        }
       // else Debug.Log(" null colors ", gameObject);
    }
    protected  void OnValidate()
    {   if (this.disabled()) return;
        if (randomId == 0)
        {
            randomId = (long)((long.MaxValue / 1000) * Random.value);
        }
if (image!=null&&colors!=null)

        image.color = colors.normalColor;
       
        slider = GetComponentInChildren<Slider>();
        layoutElement = transform.parent.GetComponent<LayoutElement>();
        if (transform.parent == null) Debug.Log("no parent");
        getSibling();
        setSize(size);
  
        // horizontal = (horizontalGroup != null);
        //		slider.wholeNumbers=true;//
        //slider.maxValue=5;



    }
    void checkDirection()
    {   //if (!vertical && !horizontal)
        {
            verticalGroup = transform.parent.parent.GetComponent<VerticalLayoutGroup>();
            horizontalGroup = transform.parent.GetComponent<HorizontalLayoutGroup>();
            vertical = (verticalGroup != null);
            horizontal = (horizontalGroup != null);
        }
    }

public void setVisibleInHierarchy(bool b)
{ if (b)
    gameObject.hideFlags=HideFlags.None;
else
    gameObject.hideFlags=HideFlags.HideInHierarchy;
}
    void getSibling()
    {
        checkDirection();
        layoutElement = transform.parent.GetComponent<LayoutElement>();
        if (randomId == 0) OnValidate();
        if (vertical)
            siblings = verticalGroup.getActiveElements();
        else

            siblings = horizontalGroup.getActiveElements();
        if (otherFlex == null || otherFlex.Length < siblings.Length) otherFlex = new float[siblings.Length];
        flexSum = 0;
        nextElement = null;
        previousElement = null;
        for (int i = 0; i < siblings.Length; i++)
        {
            if (siblings[i] == layoutElement)
            {
                if (i < siblings.Length - 2) nextElement = siblings[i + 1];
                if (i > 0) previousElement = siblings[i - 1];
            }
            flexSum += getFlex(siblings[i], vertical);

            otherFlex[i] = siblings[i].flexibleHeight;
        }


    }



    void setThisAndNeighbour(float f)
    {
        if (previousElement != null) offsetFlex(previousElement, -f);
        else offsetFlex(nextElement, -f);

        offsetFlex(layoutElement, f);
    }

    public static void normalizeFlex(LayoutElement[] list, bool vertical = true)
    {
        float flexSum = 0;
        for (int i = 0; i < list.Length; i++)
        {
            if (vertical)
                flexSum += list[i].flexibleHeight;
            else
                flexSum += list[i].flexibleWidth;
        }
        for (int i = 0; i < list.Length; i++)
        {
            if (vertical)

                list[i].flexibleHeight = list[i].flexibleHeight / flexSum;
            else
                list[i].flexibleWidth = list[i].flexibleWidth / flexSum;
        }
    }
    public static void reset(LayoutElement[] list, bool vertical = true)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (vertical)

                list[i].flexibleHeight = 1f / list.Length;
            else
                list[i].flexibleWidth = 1f / list.Length;
        }
    }
float size=10;
    public void setSize(float f)
    { size=f;
        if (vertical) rect.sizeDelta = new Vector2(0, f);
        else rect.sizeDelta = new Vector2(f, 0);
       if (vertical)
        {
            myLayoutElement.flexibleWidth=1;
            myLayoutElement.flexibleHeight=-1;
            myLayoutElement.minHeight=size;
        } else
        {
            myLayoutElement.flexibleWidth=-1;
            myLayoutElement.flexibleHeight=1;
            myLayoutElement.minWidth=size;
        }
    }
    public string getID()
    {
        return "LayoutHelper_" + randomId.ToString();
    }

    // Use this for initialization
    public float getFlex(LayoutElement l, bool vertical)
    {
        if (l == null) l = layoutElement;
        if (vertical)
            return l.flexibleHeight;
        else
            return l.flexibleWidth;
    }

    void offsetFlex(LayoutElement l, float newFlex)
    {
        if (l != null)
            if (vertical)
            {
                if (l.flexibleHeight + newFlex > 0) l.flexibleHeight += newFlex;
            }
            else
            {
                if (l.flexibleWidth + newFlex > 0) l.flexibleWidth += newFlex;
            }

    }

    public void OnBeginDrag(PointerEventData e)
    {
        dragStartPosition = e.position;
            image.color = colors.activeColor;
    }
    public void OnEndDrag(PointerEventData e)
    {
        image.color = colors.baseColor;
    }

    public void OnDrag(PointerEventData e)
    {
        float drag = 0;
        if (vertical) drag = (e.position.y - dragStartPosition.y) / Screen.height * 1.5f;
        else drag = (dragStartPosition.x + e.position.x) / Screen.width * 1.0f;
        setThisAndNeighbour(drag);
        dragStartPosition = e.position;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        image.color = colors.hoveredColor;
    }
    public void OnPointerExit(PointerEventData e)
    {
        image.color = colors.baseColor;
    }

    void Start()
    {
        
        transform.SetAsLastSibling();
    }

}
/*
public class LayoutHelper : MonoRect, ISyncColors, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public LayoutElement layoutElement;
   LayoutElement _myLayoutElement;
     LayoutElement myLayoutElement {
get {
    if (_myLayoutElement==null)
    _myLayoutElement=GetComponent<LayoutElement>();
    if (_myLayoutElement==null)
    _myLayoutElement=gameObject.AddComponent<LayoutElement>();
    return _myLayoutElement;
}
     }
    public VerticalLayoutGroup verticalGroup;
    public HorizontalLayoutGroup horizontalGroup;
    public bool horizontal;
    public bool vertical;
    //public bool setToChildControls;
    public Slider slider;

    public float flexSum;
    public LayoutElement[] siblings;
    public HoverableColors colors;

    Vector3 dragStartPosition;

    public long randomId;

    public float[] otherFlex;
    LayoutElement nextElement;
    LayoutElement previousElement;
    void Reset()
    {
        getSibling();
    }
    void updateColors()
    {
        image.color = colors.normalColor;
    }
    public void setColors(HoverableColors c)
    {
        if (c != null)
        {
            colors = c;

            colors.OnChange.AddListener(updateColors);
        }
        else Debug.Log(" null colors ", gameObject);
    }
    protected  void OnValidate()
    {
        if (randomId == 0)
        {
            randomId = (long)((long.MaxValue / 1000) * Random.value);
        }

        image.color = colors.normalColor;
       
        slider = GetComponentInChildren<Slider>();
        layoutElement = transform.parent.GetComponent<LayoutElement>();
        if (transform.parent == null) Debug.Log("no parent");
        getSibling();
        setSize(size);
  
        // horizontal = (horizontalGroup != null);
        //		slider.wholeNumbers=true;//
        //slider.maxValue=5;



    }
    void checkDirection()
    {   //if (!vertical && !horizontal)
        {
            verticalGroup = transform.parent.parent.GetComponent<VerticalLayoutGroup>();
            horizontalGroup = transform.parent.GetComponent<HorizontalLayoutGroup>();
            vertical = (verticalGroup != null);
            horizontal = (horizontalGroup != null);
        }
    }

public void setVisibleInHierarchy(bool b)
{ if (b)
    gameObject.hideFlags=HideFlags.None;
else
    gameObject.hideFlags=HideFlags.HideInHierarchy;
}
    void getSibling()
    {
        checkDirection();
        layoutElement = transform.parent.GetComponent<LayoutElement>();
        if (randomId == 0) OnValidate();
        if (vertical)
            siblings = verticalGroup.getActiveElements();
        else

            siblings = horizontalGroup.getActiveElements();
        if (otherFlex == null || otherFlex.Length < siblings.Length) otherFlex = new float[siblings.Length];
        flexSum = 0;
        nextElement = null;
        previousElement = null;
        for (int i = 0; i < siblings.Length; i++)
        {
            if (siblings[i] == layoutElement)
            {
                if (i < siblings.Length - 2) nextElement = siblings[i + 1];
                if (i > 0) previousElement = siblings[i - 1];
            }
            flexSum += getFlex(siblings[i], vertical);

            otherFlex[i] = siblings[i].flexibleHeight;
        }


    }



    void setThisAndNeighbour(float f)
    {
        if (previousElement != null) offsetFlex(previousElement, -f);
        else offsetFlex(nextElement, -f);

        offsetFlex(layoutElement, f);
    }

    public static void normalizeFlex(LayoutElement[] list, bool vertical = true)
    {
        float flexSum = 0;
        for (int i = 0; i < list.Length; i++)
        {
            if (vertical)
                flexSum += list[i].flexibleHeight;
            else
                flexSum += list[i].flexibleWidth;
        }
        for (int i = 0; i < list.Length; i++)
        {
            if (vertical)

                list[i].flexibleHeight = list[i].flexibleHeight / flexSum;
            else
                list[i].flexibleWidth = list[i].flexibleWidth / flexSum;
        }
    }
    public static void reset(LayoutElement[] list, bool vertical = true)
    {
        for (int i = 0; i < list.Length; i++)
        {
            if (vertical)

                list[i].flexibleHeight = 1f / list.Length;
            else
                list[i].flexibleWidth = 1f / list.Length;
        }
    }
float size=10;
    public void setSize(float f)
    { size=f;
        if (vertical) rect.sizeDelta = new Vector2(0, f);
        else rect.sizeDelta = new Vector2(f, 0);
       if (vertical)
        {
            myLayoutElement.flexibleWidth=1;
            myLayoutElement.flexibleHeight=-1;
            myLayoutElement.minHeight=size;
        } else
        {
            myLayoutElement.flexibleWidth=-1;
            myLayoutElement.flexibleHeight=1;
            myLayoutElement.minWidth=size;
        }
    }
    public string getID()
    {
        return "LayoutHelper_" + randomId.ToString();
    }

    // Use this for initialization
    public float getFlex(LayoutElement l, bool vertical)
    {
        if (l == null) l = layoutElement;
        if (vertical)
            return l.flexibleHeight;
        else
            return l.flexibleWidth;
    }

    void offsetFlex(LayoutElement l, float newFlex)
    {
        if (l != null)
            if (vertical)
            {
                if (l.flexibleHeight + newFlex > 0) l.flexibleHeight += newFlex;
            }
            else
            {
                if (l.flexibleWidth + newFlex > 0) l.flexibleWidth += newFlex;
            }

    }

    public void OnBeginDrag(PointerEventData e)
    {
        dragStartPosition = e.position;
            image.color = colors.activeColor;
    }
    public void OnEndDrag(PointerEventData e)
    {
        image.color = colors.baseColor;
    }

    public void OnDrag(PointerEventData e)
    {
        float drag = 0;
        if (vertical) drag = (e.position.y - dragStartPosition.y) / Screen.height * 1.5f;
        else drag = (dragStartPosition.x + e.position.x) / Screen.width * 1.0f;
        setThisAndNeighbour(drag);
        dragStartPosition = e.position;
    }

    public void OnPointerEnter(PointerEventData e)
    {
        image.color = colors.hoveredColor;
    }
    public void OnPointerExit(PointerEventData e)
    {
        image.color = colors.baseColor;
    }

    void Start()
    {
        
        transform.SetAsLastSibling();
    }*/

}
