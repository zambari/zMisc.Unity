
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LayoutEditorUtilities
{


    static void createCanvasIfNotPresent()
    {
        if (Selection.activeGameObject == null || Selection.activeGameObject.GetComponentInParent<Canvas>() == null)
        {
            Canvas can = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
            if (can == null)
            {
                GameObject c = new GameObject("Canvas");
                c.AddComponent<Canvas>();
                c.AddComponent<GraphicRaycaster>();
                c.AddComponent<CanvasScaler>();
                Selection.activeGameObject = c;
                return;
            }
            Selection.activeGameObject = can.gameObject;
        }
    }
    static void CreateLayout(RectTransform container, bool vertical)
    {
        int count = 3;
//        int spacing = 5;

        if (vertical)

            container.gameObject.AddComponent<VerticalLayoutGroup>().setChildControl();
        else

            container.gameObject.AddComponent<HorizontalLayoutGroup>().setChildControl();

        container.gameObject.AddOrGetComponent<LayoutGroupHelper>();
        List<GameObject> cretedObjects = new List<GameObject>();
        for (int i = 0; i < count; i++)
        {
            RectTransform child = container.AddChild();
            cretedObjects.Add(child.gameObject);
            child.anchorMin = new Vector2(0, 0);
            child.anchorMax = new Vector2(1, 1);
            child.offsetMin = new Vector2(0, 0);
            child.offsetMax = new Vector2(0, 0);
            Image im = child.gameObject.AddComponent<Image>();
            im.color = im.color.Random();
            child.name = "Item " + (i + 1);
            LayoutElement le = child.gameObject.AddComponent<LayoutElement>();
            le.flexibleHeight = (vertical ? 1f / count : 1);
            le.flexibleWidth = (vertical ? 1 : 1f / count);
          
        }
        container.name = (vertical ? "VerticalLayout" : "HorizontalLayout");
    }


    [MenuItem("GameObject/UI/Create horizontal  layout")]
    static void CreateHorizontalLayout()

    {
        createCanvasIfNotPresent();

        RectTransform container = Selection.activeGameObject.rect();//.AddChild();
        Image a = container.GetComponent<Image>();
        if (a != null) a.enabled = false;
        Undo.RecordObject(Selection.activeGameObject, "Adding layout");
        CreateLayout(container, false);
    }
    [MenuItem("GameObject/UI/Create verticallayout")]
    static void CreateVerticalLayout()
    {

        createCanvasIfNotPresent();

        RectTransform container = Selection.activeGameObject.rect();//.AddChild();
        Image a = container.GetComponent<Image>();
        if (a != null) a.enabled = false;

        Undo.RecordObject(Selection.activeGameObject, "Adding layout");
        CreateLayout(container, true);
    }

}
#endif