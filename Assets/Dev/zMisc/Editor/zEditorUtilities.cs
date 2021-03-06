﻿
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;

// changelist
// v.030 image->rawimage added
public static class zEditorUtilities
{
    const string uiTag = "UI";
    [MenuItem("GameObject/UI/UI Subobjects/Add 'UI' tag to subobjects")]

    public static void AddUITags()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject g = Selection.gameObjects[i];
            AddUITags<Button>(g);
            AddUITags<Toggle>(g);
            AddUITags<Slider>(g);
            AddUITags<Scrollbar>(g);
            AddUITags<InputField>(g);
            AddUITags<Dropdown>(g);

        }

    }

    static void AddUITags<T>(GameObject g)
    {
        T[] ui = g.GetComponentsInChildren<T>();
        {
            for (int i = 0; i < ui.Length; i++)
            {
                GameObject[] childrenOfThisUI = (ui[i] as MonoBehaviour).gameObject.getChildren(true);
                for (int k = 0; k < childrenOfThisUI.Length; k++)
                {
                    childrenOfThisUI[k].tag = uiTag;
                }
            }
        }
    }


    [MenuItem("GameObject/UI/UI Subobjects/Select/All Children  Handles")]
    public static void SelHandles()
    {

        Selection.objects = Selection.gameObjects.getAllChildrenCalled("Handle");

    }

    [MenuItem("GameObject/UI/UI Subobjects/Select/All Children  Backgounrd")]
    public static void SelBackgrounds()
    {

        Selection.objects = Selection.gameObjects.getAllChildrenCalled("Background");

    }


    [MenuItem("GameObject/UI/UI Subobjects/Select/All Children  Fills")]
    public static void SelFills()
    {

        Selection.objects = Selection.gameObjects.getAllChildrenCalled("Fill");

    }
    [MenuItem("Tools/selection/select objects called the same one level up")]

    public static void selectObjectsNamedTheSame1()
    {
        selectObjectsNamedTheSame(1);
    }


    [MenuItem("Tools/selection/select objects called the same two levels up")]

    public static void selectObjectsNamedTheSame2()
    {
        selectObjectsNamedTheSame(2);
    }

    [MenuItem("Tools/selection/select objects called the same three levels up")]

    public static void selectObjectsNamedTheSame3()
    {
        selectObjectsNamedTheSame(3);
    }

    [MenuItem("Tools/selection/select objects called the same four levels up")]

    public static void selectObjectsNamedTheSame4()
    {
        selectObjectsNamedTheSame(4);
    }


    [MenuItem("Tools/selection/select objects called the same five levels up")]

    public static void selectObjectsNamedTheSame5()
    {
        selectObjectsNamedTheSame(5);
    }
    public static void selectObjectsNamedTheSame(int level)
    {   level++;
        string selName=Selection.activeGameObject.name;
        
        Transform transform=Selection.activeGameObject.transform;
        for (int i=0;i<level;i++)
         transform=transform.parent;
       // Debug.Log("parent object is "+transform.gameObject.name);
        Transform[] all=transform.gameObject.GetComponentsInChildren<Transform>();
        List<GameObject> g=new List<GameObject>();
        int activeObjects=0;
        int inactiveObjects=0;
        for (int i=0;i<all.Length;i++)
        {
            if (all[i].name.Equals(selName))
            {  g.Add(all[i].gameObject);
               if (all[i].gameObject.activeInHierarchy) activeObjects++; else inactiveObjects++;
                }
        }
        Selection.objects=g.ToArray();
        Debug.Log("selected "+g.Count+" objects "+activeObjects+" active "+ inactiveObjects+" inactive");


    }

    [MenuItem("GameObject/UI/UI Subobjects/Hide Objects with UI tags")]

    public static void HideUITags()
    {
        GameObject[] childrenOfThisUI = GameObject.FindGameObjectsWithTag(uiTag);
        for (int i = 0; i < childrenOfThisUI.Length; i++)
        {
            childrenOfThisUI[i].hideFlags = HideFlags.HideInHierarchy;

        }

        EditorApplication.RepaintHierarchyWindow();
    }

    [MenuItem("GameObject/Create Empty Parent")] // #&e
    static void createEmptyParent()
    {
        GameObject go = new GameObject("GameObject");
        if (Selection.activeTransform != null)
        {

            go.transform.parent = Selection.activeTransform.parent;

            go.transform.Translate(Selection.activeTransform.position);

            Selection.activeTransform.parent = go.transform;

        }

    }


    [MenuItem("GameObject/Create Empty Duplicate")] // #&d
    static void createEmptyDuplicate()
    {

        GameObject go = new GameObject("GameObject");

        if (Selection.activeTransform != null)
        {
            go.transform.parent = Selection.activeTransform.parent;
            go.transform.Translate(Selection.activeTransform.position);
        }

    }

    [MenuItem("GameObject/Create Empty Child")] // #&c
    static void createEmptyChild()
    {

        GameObject go = new GameObject("GameObject");

        if (Selection.activeTransform != null)
        {
            go.transform.parent = Selection.activeTransform;
            go.transform.Translate(Selection.activeTransform.position);
        }

    }




    [MenuItem("Edit/PasteAsChild %&v")]
    public static void PasteAsChild()
    {

        GameObject x = Selection.activeGameObject;

        EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent("Paste"));

        Selection.activeGameObject.transform.SetParent(x.transform);

    }
    [MenuItem("GameObject/UI/UI Subobjects/Show Objects with UI tags")]

    public static void ShowUITags()
    {
        GameObject[] childrenOfThisUI = GameObject.FindGameObjectsWithTag(uiTag);
        for (int i = 0; i < childrenOfThisUI.Length; i++)
        {
            childrenOfThisUI[i].hideFlags = HideFlags.None;

        }

        EditorApplication.RepaintHierarchyWindow();
    }
    [MenuItem("GameObject/UI/Copy Without Children")]
    public static void AddCopy()
    {
        if (Selection.activeGameObject == null)
        { Debug.LogWarning("select  gameObject"); return; }
        if (Selection.activeGameObject.transform == null) Debug.LogWarning("sss parent gameObject");
        GameObject go = MonoBehaviour.Instantiate(Selection.activeGameObject, Selection.activeGameObject.transform.parent);
        go.transform.SetSiblingIndex(Selection.activeGameObject.transform.GetSiblingIndex() + 1);
        for (int i = go.transform.childCount; i < 0; i++)
        {
            GameObject tgo = go.transform.GetChild(i).gameObject;
            Debug.Log("detrod" + tgo.name);
            MonoBehaviour.DestroyImmediate(tgo);

        }
        Selection.activeGameObject = go;
    }


    /*
      public  static RectTransform AddImageChildChildLayout(this RectTransform parentRect)
        {      RectTransform rect = parentRect.AddImageChildChild();




            return rect;
        }*/
    static void getCanvasTarget()
    {
        Undo.RecordObject(Selection.activeGameObject, "gggge");
        Canvas c = Selection.activeGameObject.GetComponentInParent<Canvas>();
        if (c == null)
        {
            c = GameObject.FindObjectOfType(typeof(Canvas)) as Canvas;
            if (c == null) Debug.Log("no canvas object");
            Selection.activeGameObject = c.gameObject;
        }


    }
    /*
    
     var components = GetComponents<Component>();
Then loop through those components and do:

 UnityEditorInternal.ComponentUtility.CopyComponent(components[i]);
 UnityEditorInternal.ComponentUtility.PasteComponentAsNew(targetGameObject);
     */
    static GameObject[] storedSelection;
    [MenuItem("Edit/Selection/markForSelection %#c")]
    static void storeSelection()
    {

        storedSelection = new GameObject[Selection.gameObjects.Length];
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            storedSelection[i] = Selection.gameObjects[i];
        }
        Debug.Log("marked " + Selection.activeGameObject.name + " for copy");

    }
    [MenuItem("Edit/Selection/paste in places %#v")]
    static void pasteSelection()
    {

        if (storedSelection == null)
        { EditorApplication.ExecuteMenuItem("Edit/Paste%v");
            Debug.Log("nothing marked for selction");
        }
        else
        {
            GameObject[] newSelection = new GameObject[storedSelection.Length];
            for (int i = 0; i < storedSelection.Length; i++)
            {
                GameObject go = MonoBehaviour.Instantiate(storedSelection[i], Selection.activeGameObject.transform);
                go.transform.localPosition = storedSelection[i].transform.localPosition;
                go.name = storedSelection[i].name;
                newSelection[i] = go;
                Undo.RegisterCreatedObjectUndo(go, "Created go");
            }

            Selection.objects = newSelection;
        }
    }
    /*
static GameObject[] sels;
 [MenuItem("GameObject/Selection/restore/restoreSel1")]
 static void restorestore1()
    {

        restore(1);
    }
    [MenuItem("GameObject/Selection/store/storeSel1")]
 static void store1()
    {

        store(1);
    }
    static void store(int id)
    {
        if (sels==null) 
        sels=new GameObject[10];
        sels[id]= Selection.activeGameObject;
    }

  static void restore(int id)
    {
        if (sels==null) Debug.Log("no store");
        if (sels[id]!=null)
        Selection.activeGameObject=sels[id]; else
        Debug.Log("no selection saved under "+id);

    }*/
    [MenuItem("GameObject/Selection/Select text children")]

    static void SelectAllTextChildren()
    {
        if (Selection.activeGameObject == null)
        { Debug.LogWarning("select  gameObject"); return; }

        Text[] texts = Selection.activeGameObject.GetComponentsInChildren<Text>();
        GameObject[] gos = new GameObject[texts.Length];
        for (int i = 0; i < texts.Length; i++)
        {
            gos[i] = texts[i].gameObject;
        }
        Selection.objects = gos;
    }



    public static GameObject duplicateInPlace(GameObject source)
    {
        if (source.transform == null) Debug.LogWarning("sss parent gameObject");
        GameObject go = MonoBehaviour.Instantiate(source, source.transform.parent);
        go.transform.SetSiblingIndex(source.transform.GetSiblingIndex() + 1);

        Undo.RegisterCreatedObjectUndo(go, "Created Clone");
        Undo.RecordObject(go, "Created Clone");
        go.name = source.name + "_";
        return go;
    }


    [MenuItem("Edit/DuplicateinPlace #%d")]
    static void DupInPlace()
    {
        GameObject[] newSelection = new GameObject[Selection.gameObjects.Length];
        for (int i = 0; i < newSelection.Length; i++)
            newSelection[i] = duplicateInPlace(Selection.gameObjects[i]);

        Selection.objects = newSelection;
    }
    [MenuItem("GameObject/UI/Create slider bank")]

    static void AddSliderBank()
    {
        string prefabName = "SimpleSlider";

        GameObject go = Resources.Load(prefabName) as GameObject;
        if (go == null)
        {
            Debug.Log("a prefab clled DefaultSlider was not found in any of the Resources folder. Please save a default slider first");
            Debug.Log("make sure it has a layoutelement component");
            return;
        }
        AddImageChildAaChildToSelection();
        GameObject container = Selection.activeGameObject;


        container.name = "Sliderbank";
        Transform t = Selection.activeGameObject.transform;
        for (int i = 0; i < 5; i++)
        {
            GameObject g = MonoBehaviour.Instantiate(go, t);
            Slider s = g.GetComponent<Slider>();
            g.name = "Slider " + (i + 1);
            SafeSlider ss = container.AddComponent<SafeSlider>();
            ss.slider = s;
            ss.createTexts();
        }
        VerticalLayoutGroup vl = container.AddComponent<VerticalLayoutGroup>();
        ContentSizeFitter cf = container.AddComponent<ContentSizeFitter>();
        cf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        cf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        vl.LayoutParameters();
    }
    [MenuItem("GameObject/UI/Image Child")]
    public static void AddImageChildAaChildToSelection()
    {
        if (Selection.activeGameObject == null)
        { Debug.Log("select parent gameObject"); return; }
        getCanvasTarget();
        Undo.RecordObject(Selection.activeGameObject, "Adding image");
        RectTransform parentRect = Selection.activeGameObject.GetComponent<RectTransform>();

        RectTransform r = parentRect.AddChild();
        Image i = r.AddImageChild();//;
        i.gameObject.AddLayoutElementFlexible();

        Selection.activeGameObject = i.gameObject;
    }
    [MenuItem("GameObject/UI/Image->RawImage")]
    static void ImageToRawImage()
    {
                Undo.RecordObject(Selection.activeGameObject, "Converting image");
                int k=0;
                for (int i=0;i<Selection.gameObjects.Length;i++)
                {
                    GameObject g=Selection.gameObjects[i];
                    Image image=g.GetComponent<Image>();
                    if (image!=null)
                    {
                        Material m=image.material;
                        Texture t=image.sprite.texture;
                         MonoBehaviour.DestroyImmediate(image);
                        RawImage raw=g.AddComponent<RawImage>();
                        raw.texture=t;
                        raw.material=m;
                            k++;

                    }
                }
                Debug.Log("Converted "+k+" images to rawimage");

    }

}
#endif