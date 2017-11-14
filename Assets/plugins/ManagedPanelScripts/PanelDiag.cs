using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace zUI
{

[System.Serializable]
public class LayoutElementDiag
{

    public Orientation orientation;
    [ReadOnly]
    public int elements;
    [ReadOnly]
    public int nonElements;
    [ReadOnly]
    public int excludedFromLayout;
    [ReadOnly]
    public int usingLayout;
    [ReadOnly]
    public int usingFlexible;
    [ReadOnly]
    public float flexibleSum;

    void clearCoutners()
    {
        elements = 0;
    }

    // int nonElements=0;

    //public int excludedFromLayout=0;

    //public int usingLayout=0;

    //public int usingFlexible=0;

    //public float flexibleSum=0;



    //List<LayoutElement> elements;

    public void Remeasure(MonoBehaviour source)
    {
        //	if (elements==null)
        // elements=new List<LayoutElement>();
        GameObject[] children = source.gameObject.GetChildrenGameObjects();
        foreach (GameObject go in children)
        {
            //	 LayoutElement le=go.GetComponent<LayoutElement>();
            //if (le!=null)
            //elements.Add(le);
        }


    }
    public void OnValidate(MonoBehaviour source)
    {
        Remeasure(source);
    }
#pragma warning disable 414
    //[ReadOnly]
    //public int 

}
[System.Serializable]
public class PanelDiag
{
#pragma warning disable 414
    [ReadOnly]
    public int numberOfRectTransforms;
    public LayoutElementDiag horizontal;
    public LayoutElementDiag vertical;
    RectTransform homeTransform;
    void ReMeasure(RectTransform source)
    {
        //here we go, you can make stuff up
        numberOfRectTransforms = source.gameObject.GetComponentsInChildren<RectTransform>().Length;
//        GameObject[] goList = source.gameObject.GetChildrenGameObjects();


    }

    public void OnValidate(MonoBehaviour source)
    {
        if (homeTransform == null || true)
        {
            homeTransform = source.gameObject.GetComponent<RectTransform>();

        }
        horizontal.orientation = Orientation.Horizontal;
        vertical.orientation = Orientation.Vertical;
        horizontal.OnValidate(source);
        vertical.OnValidate(source);
        ReMeasure(homeTransform);
    }
    public void OnTransformChildrenChanged(MonoBehaviour source)
    {
        homeTransform = source.gameObject.GetComponent<RectTransform>();

    }

}
}