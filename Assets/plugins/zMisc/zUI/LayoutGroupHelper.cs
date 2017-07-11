using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutGroupHelper : MonoRect {

//public LayoutElement[] childElements;
HorizontalLayoutGroup horizontalGroup;
VerticalLayoutGroup vericalGroup;
bool isHorizontal;
bool isVertical;

//LayoutHelper[] helpers;
//LayoutElement[] elements;
//public bool a;
[ExposeMethodInEditor]
public void add()
{
Debug.Log("public a");
}

[ExposeMethodInEditor]
void aa()
{
Debug.Log("not public a");
}
void OnValidate()
{
  horizontalGroup  =GetComponent<HorizontalLayoutGroup>() ;
 vericalGroup =GetComponent<VerticalLayoutGroup >();
 isHorizontal=(horizontalGroup!=null);
 isVertical=(vericalGroup!=null);
 if (!isVertical&&!isHorizontal) Debug.LogWarning("no layout group here" ,gameObject);

/*
    LayoutHelper[] helpers=GetComponentsInChildren<LayoutHelper>();
    for (int i=0;i<helpers.Length;i++)
    {
                Debug.Log("destroing "+helpers[i].name);
        Destroy(helpers[i].gameObject);

    }

	GameObject[] children=gameObject.getChildren();
    Debug.Log("ch c"+children.Length);

    elements=gameObject.getActiveElements();
      Debug.Log("elements"+elements.Length);
      helpers=new LayoutHelper[elements.Length-1];
    for (int i=0;i<elements.Length-1;i++)
    {
        Debug.Log("element" +i);
        helpers[i]=createHelper(elements[i].gameObject.transform);

    } */
}

LayoutHelper createHelper(Transform target)
{
  RectTransform child = rect.AddChild();
            
            child.anchorMin = new Vector2(0, 0);
            child.anchorMax = new Vector2(1, 1);
            child.offsetMin = new Vector2(0, 0);
            child.offsetMax = new Vector2(0, 0);
            Image im = child.gameObject.AddComponent<Image>();
            im.color = im.color.Random();
            //child.name = "Item " + (i + 1);
  //          LayoutElement le = child.gameObject.AddComponent<LayoutElement>();
//            le.flexibleHeight = (isVertical ? 1f / elements.Length : 1);
 //           le.flexibleWidth = (isVertical ? 1 : 1f / elements.Length);
             child.SetSiblingIndex(target.GetSiblingIndex()+1);
    return child.gameObject.AddComponent<LayoutHelper>();

}
/* 
void getElements()
{
    List<LayoutElement> elements = new List<LayoutElement>();
        if (layout == null) return elements.ToArray();
        for (int i = 0; i < layout.transform.childCount; i++)
        {
            GameObject thisChild = layout.transform.GetChild(i).gameObject;
            LayoutElement le = thisChild.GetComponent<LayoutElement>();
            if (le != null)
            {
                if (!le.ignoreLayout) elements.Add(le);
            }
        }
}*/
/*

  if (i < count - 1)
            {
                RectTransform controller = container.AddChild();

                Image cim = controller.gameObject.AddComponent<Image>();

                controller.name = "Controller " + (i + 1);

                cretedObjects.Add(child.gameObject);


                controller.anchorMin = new Vector2(0, 0);
                controller.anchorMax = new Vector2(1, 1);
                controller.offsetMin = new Vector2(0, 0);
                controller.offsetMax = new Vector2(0, 0);
                LayoutElement leC = controller.gameObject.AddComponent<LayoutElement>();
                leC.flexibleHeight = (vertical ? -1 : 1);
                leC.flexibleWidth = (vertical ? 1 : -1);
                if (vertical) leC.preferredHeight = spacing; else leC.preferredWidth = spacing;
            }
			
			 */
	
	
}
