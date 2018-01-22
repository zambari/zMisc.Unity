using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;

public class zEditorBase : EditorWindow
{
    //line 512 gameObject.transform.SetAsFirstSibling();    

    protected virtual void DisplayTab(string toolName)
    {
        // main thing to override,do your shit here
    }

    protected virtual bool ShouldTabBeVisible(string s) // override to enable context sensitive tab switching
    {
        // conditionally enable tabs
        return true;
    }

    /// <summary>
    /// This method will be called by the base class on init to enable easy menu construcion
    /// </summary>
    protected virtual void AddTabs()
    {
        //AddTab("myTab");
    }

    /// <summary>
    /// This method will be called by the base class on init to enable easy preferences access
    /// </summary>

    protected virtual void AddOptions()
    {
        //AddOption(string s, bool defaultValue = true);
    }

    /// <summary>
    /// Called on selection change
    /// </summary>

    protected virtual void OnSelectionChange()
    {
        //     objectCacheValid = false;
        GetAvailableTools();
        Repaint();
    }


    /// <summary>
    ///  Called on tab change
    /// </summary>
    protected virtual void OnToolChange()
    {

    }
    protected virtual void OnEnable()
    {
        selectedTab = PlayerPrefs.GetInt(baseName + "_lastTool", 0);
        //  SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }
    protected virtual void OnDisable()
    {
        //   SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }

    protected static void BaseInit(Type t)
    {
        var window = EditorWindow.GetWindow(t);
        zEditorBase editorBase = (zEditorBase)window;
        editorBase.allTabs = new List<string>();
        editorBase.options = new Dictionary<string, bool>();
        editorBase.toolEnabledFlag = new List<bool>();
        editorBase.AddTabs();
        editorBase.AddOptions();
        editorBase.GetAvailableTools();
    }
 protected void BH() {     GUILayout.BeginHorizontal(); }
 protected void EH() {     GUILayout.EndHorizontal(); }
 protected void BV() {     GUILayout.BeginVertical(); }
 protected void EV() {     GUILayout.EndVertical(); }
    protected virtual void OnGUI()
    {
        GUILayout.Space(6);
        if (availableTools == null) GetAvailableTools();
        if (availableTools.Length == 0) return;
        else
        if (selectedTab >= availableTools.Length) selectedTab = availableTools.Length - 1;

        selectedTab = GUILayout.Toolbar(selectedTab, availableTools);
        if (availableTools[selectedTab] == "CFG") DisplayConfig();
        else
            DisplayTab(availableTools[selectedTab]);
        DisplayFooter();
    }
    // bool IfHasComponentAndClicked<T>(string buttonLabel, Action<T> actionToPerform, bool includeChildren = false) where T : Component
    //  IfClicked<T>(string buttonLabel, Action<T> actionToPerform, bool includeChildren = false) where T : Component
    protected virtual GUILayoutOption smallButton { get { return GUILayout.Width(100); } }

    protected bool IfButton(string buttonLabel, Action actionToPerform)
    {
        if (GUILayout.Button(buttonLabel, buttonStyle))
        {
            actionToPerform();
            return true;
        }
        return false;

    }
    protected bool IfButtonDoOnComponents<T>(string buttonLabel, Action<T> actionToPerform, bool includeChildren = false) where T : Component
    {
        if (GUILayout.Button(buttonLabel, buttonStyle))
        {
            List<T> componentList = GetComponentsOfType<T>();
            for (int i = 0; i < componentList.Count; i++)
                if (componentList[i] != null)
                {
                    Undo.RecordObject(componentList[i], "utility");
                    actionToPerform((T)componentList[i]);
                }
            if (componentList.Count > 0) return true;
        }
        return false;
    }

    /// <summary>
    /// returns false if multiple selected
    /// </summary>
    /// <returns></returns>
    protected bool SelectionHasComponent<T>() where T : Component
    {
        if (Selection.gameObjects.Length!=1) return false;
        return (Selection.activeGameObject.GetComponent<T>()!=null);
    }

    protected bool SelectionHasComponentsSome<T>(bool inChildren = false) where T : Component
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            if (!inChildren)
            {
                if (Selection.gameObjects[i].GetComponent<T>() != null) return true; ;
            }
            else
            {
                if (Selection.gameObjects[i].GetComponentInChildren<T>() != null) return true;

            }

        }
        return false;
    }

    protected bool SelectionHasComponentsAll<T>(bool inChildren = false) where T : Component
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            if (!inChildren)
            {
                if (Selection.gameObjects[i].GetComponent<T>() == null) return false;
            }
            else
            {
                if (Selection.gameObjects[i].GetComponentInChildren<T>() == null) return false;
            }

        }
        return true;
    }
    protected bool IfHasComponentAndClicked<T>(string buttonLabel, Action<T> actionToPerform, bool includeChildren = false) where T : Component
    {
        List<T> componentList = GetComponentsOfType<T>();
        if (componentList.Count > 0 && GUILayout.Button(buttonLabel, buttonStyle))
        {
            for (int i = 0; i < componentList.Count; i++)
                if (componentList[i] != null)
                {
                    Undo.RecordObject(componentList[i], "utility");
                    actionToPerform((T)componentList[i]);
                }
            return true;
        }
        else return false;
    }
    protected bool GetOption(string s)
    {
        bool result = true;
        CheckLists();
        options.TryGetValue(s, out result);
        return result;

    }
    protected void StoreOption(string opName, bool newVal)
    {
        CheckLists();
        PlayerPrefs.SetInt(optionName(opName), (newVal ? 1 : 0));
        bool val = newVal;
        if (options.TryGetValue(opName, out val))
            options[opName] = newVal;
        else
            options.Add(opName, newVal);
    }

    protected virtual void DisplayCustomConfig()
    {
        if (options.Count == 0) return;
        Label("Custom Options:");
        List<string> keys = options.Keys.ToList();
        foreach (string thisKey in keys)
        {
            bool current = GetOption(thisKey);
            bool newVal = GUILayout.Toggle(current, thisKey);
            if (current != newVal) StoreOption(thisKey, newVal);
        }
    }

    protected virtual void DisplayConfig()
    {
        CheckLists();
        GUILayout.Space(10);
        DisplayColumns(DisplayTabConfig, DisplayCustomConfig);

    }
    protected bool ButtonSmall(string label)
    {
        return GUILayout.Button(label,EditorStyles.miniButton);
    }
    protected bool Button(string label)
    {
        return GUILayout.Button(label);
    }
      protected bool Button(string label,Action a)
    {
        bool result=GUILayout.Button(label);
        if (result) a();
        return result;
    }
    protected virtual void DisplayFooter()
    {
        GUILayout.FlexibleSpace();
    }
    protected virtual void DisplayTabConfig()
    {
        Label("Tabs");
        for (int i = 0; i < allTabs.Count - 1; i++)
        {
            bool newVal = GUILayout.Toggle(toolEnabledFlag[i], allTabs[i]);
            if (newVal != toolEnabledFlag[i])
            {
                toolEnabledFlag[i] = newVal;
                PlayerPrefs.SetInt(baseName + "_toolShown_" + allTabs[i], (newVal ? 1 : 0));
                GetAvailableTools();
            }
        }
    }
    protected virtual GUIStyle buttonStyle { get { return EditorStyles.miniButton; } }
    // Boilerplate code and helper functoins below
    protected virtual void Label(string s)
    {
        GUILayout.Label(s);
    }
    protected virtual void Space()
    {
        GUILayout.Space(10);
    }

    #region nonVirtual
    protected void PerformOnSelection(Action<GameObject> actionToPerform)
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            Undo.RecordObject(Selection.gameObjects[i], "PerformOnSelection");
            actionToPerform(Selection.gameObjects[i]);
        }
    }

    protected void PerformOnComponents<T>(Action<T> actionToPerform, bool includeChildren = false) where T : Component
    {
        List<T> componentList = GetComponentsOfType<T>(includeChildren);
        for (int i = 0; i < componentList.Count; i++)
            if (componentList[i] != null)
            {
                Undo.RecordObject(componentList[i], "utility");
                actionToPerform((T)componentList[i]);
            }
    }
    protected void DisplayColumns(Action col1, Action col2)
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        col1();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        col2();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    protected void DisplayColumns(Action col1, Action col2, Action col3)
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        col1();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        col2();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        col3();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }


    protected void DisplayColumns(Action col1, Action col2, Action col3, Action col4)
    {
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        col1();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        col2();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        col3();
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        col4();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }


    protected virtual string baseName { get { return _baseName; } set { _baseName = value; } }
    protected virtual bool showConfig { get { return _showConfig; } set { _showConfig = value; } }
    protected List<bool> toolEnabledFlag;
    List<string> allTabs;
    protected string[] availableTools;
    Dictionary<string, bool> options;
    string _baseName = "editorBase";
    bool _showConfig = true;
    protected bool multipleSelected { get { return Selection.gameObjects.Length>1; } }
    protected bool objectIsSelected { get { return Selection.activeGameObject != null; } }
    protected bool nothingSelected { get { if (selObj == null) { GUILayout.Label("nothing selected"); return true; } return false; } }
    protected GameObject selObj
    {
        get { return Selection.activeGameObject; }
        set { Selection.activeGameObject = value; ; }
    }
protected void SetTool(string t)
{
    for (int i=0;i<availableTools.Length;i++)
    {
        if (availableTools[i]==t) selectedTab=i;
    }

}
    protected Color defaultTextColor = Color.white;
    protected string[] layouts = { "None", "Horizontal", "Vertical" };
    protected virtual Color GetNewPanelColor()
    {
        return new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value) * 0.8f;
    }

    void CheckLists()
    {
        if (allTabs == null) { allTabs = new List<string>(); AddTabs(); }
        if (options == null) { options = new Dictionary<string, bool>(); AddOptions(); }
        if (toolEnabledFlag == null) toolEnabledFlag = new List<bool>();
    }

    string optionName(string s)
    {
        return baseName + "_customOptions_" + s;
    }

    protected void AddOption(string s, bool defaultValue = true)
    {
        CheckLists();
        bool val = defaultValue;
        int fromPrefs = PlayerPrefs.GetInt(optionName(s), -1);
        if (fromPrefs == 0) val = false;
        if (fromPrefs == 1) val = true;
        StoreOption(s, val);

    }
protected void FlexibleSpace()
{
    GUILayout.FlexibleSpace();
}
    protected void AddTab(string s)
    {
        CheckLists();
        allTabs.Add(s);
        toolEnabledFlag.Add(PlayerPrefs.GetInt(baseName + "_toolShown_" + s, 1) == 1);
    }
    protected string[] GetAvailableTools()
    {
        int lastCount = 0;
        if (availableTools != null) lastCount = availableTools.Length - 1;
         CheckLists();
      /*  if (allTabs == null)
        {
            AddTab("no tools added");
        }*/
        List<string> activeTabs = new List<string>();
        for (int i = 0; i < allTabs.Count; i++)
        {
            if (toolEnabledFlag[i] && ShouldTabBeVisible(allTabs[i])) activeTabs.Add(allTabs[i]);
        }
        if (showConfig) activeTabs.Add("CFG");
        availableTools = activeTabs.ToArray();
        int thisCount = availableTools.Length - 1;
        if (thisCount > lastCount) selectedTab++;
        else
        if (thisCount < lastCount) selectedTab--;
        return availableTools;
    }

    protected int selectedTab
    {
        get { return _selectedTab; }
        set
        {
            if (_selectedTab != value)
            {
                PlayerPrefs.SetInt(baseName + "_lastTool", value);
                _selectedTab = value;
                OnToolChange();
            }
        }
    }
    protected int _selectedTab;


    protected void CollapseComponents(bool collapse)
    {
        Component[] components = selObj.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(components[i], !collapse);
        }

        GameObject g = selObj;
        selObj = null;
        EditorApplication.delayCall += () => selObj = g;
    }
    protected string StripUnityComponentName(string s)
    {
        if (s.Contains("UnityEngine"))
        {
            return s.Substring(12);
        }
        return s;
    }
    protected T CreateObjectWithComponent<T>(string name) where T : Component
    {
        return CreateChild(name).gameObject.AddComponent<T>();
    }

    protected Transform GetCanvasTransform()
    {
        Canvas c = GameObject.FindObjectOfType<Canvas>();
        if (c == null)
        {
            c = CreateObjectWithComponent<Canvas>("Canvas");
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler cs = c.gameObject.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1920, 1080);
            c.gameObject.AddComponent<GraphicRaycaster>();
            EventSystem e = CreateObjectWithComponent<EventSystem>("EventSystem");
            e.gameObject.AddComponent<StandaloneInputModule>();
            c.transform.SetParent(null);
        }
        return c.transform;
    }

    /// <summary>
    /// Returns all the components of given type within selected gameObject (and their children with optional flag)
    /// </summary>

    protected List<T> GetComponentsOfType<T>(bool includeChildren = false) where T : Component
    {
        List<T> listOfComponents = new List<T>();
        listOfComponents = new List<T>();
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            if (!includeChildren)
            {
                listOfComponents.Add(Selection.gameObjects[i].GetComponent<T>());
            }
            else
            {
                T[] myComponents = Selection.gameObjects[i].GetComponentsInChildren<T>();
                for (int j = 0; j < myComponents.Length; j++)
                    if (!listOfComponents.Contains(myComponents[j]))
                        listOfComponents.Add(myComponents[j]);
            }
        }
        return listOfComponents;
    }


    #region objectCreators
    protected GameObject CreatePanel(bool filling = false)
    {
        GameObject gameObject = CreateChild("Panel", GetUIParent()).gameObject;
        gameObject.transform.SetAsFirstSibling();
        
        Image i = gameObject.AddComponent<Image>();
        i.color = GetNewPanelColor();
        if (filling)
        {
            RectTransform r = gameObject.GetComponent<RectTransform>();
            r.localScale = Vector2.one;
            r.anchorMin = Vector2.zero;
            r.anchorMax = Vector2.one;
            r.offsetMax = Vector2.zero;
            r.offsetMin = Vector2.zero;
        }
        return gameObject;
    }

    protected GameObject HandleParentSelectionContext(GameObject newGo)
    {
        Undo.RegisterCreatedObjectUndo(newGo, "created via helper");
        if (selObj != null)
        {
            newGo.transform.SetParent(selObj.transform);
            newGo.transform.position = Vector3.zero;
            newGo.transform.rotation = Quaternion.identity;
        }
        selObj = newGo;
        return newGo;
    }


    protected void TextObjectNameChange(Text t)
    {
        string currentText = t.text;
        string newText = GUILayout.TextArea(currentText);
        
        if (currentText != newText)
        {  EditorGUIUtility.PingObject(t);

        t.text = newText; 

        }
        if (GetOption("SetTextObjectName"))

        {
            string[] afterSplit=t.text.Split('\n');
            if (afterSplit.Length>1) t.name = "<Text: multitline>";
            else 
            t.name = "<Text: \""+afterSplit[0]+(afterSplit.Length>1?"\n":"")+"\">";
        t.raycastTarget=false;
        }
    
   
      
    }

    protected GameObject createText()
    {
        RectTransform rect = CreateChild("Text", GetUIParent());
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        Text text = rect.gameObject.AddComponent<Text>();
        text.text = "New Text";
        text.color = defaultTextColor;
        return rect.gameObject;
    }

    protected void RequestComponentOnSelection<T>(string butlabel) where T : Component
    {
        if (selObj == null) return;
        if (GUILayout.Button(butlabel))
        {
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                T myComponent = Selection.gameObjects[i].GetComponent<T>();
                if (myComponent == null)
                {
                    myComponent = Selection.gameObjects[i].AddComponent<T>();
                    Undo.RegisterCreatedObjectUndo(myComponent, "component added");
                }
            }
            GetAvailableTools();
        }
    }
    protected void CreateRawImage()
    {
        Canvas canvas = GameObject.FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject go = new GameObject("Canvas");
            canvas = go.AddComponent<Canvas>();
        }
        GameObject im = new GameObject("RawImage");
        im.transform.SetParent(canvas.transform);
        im.AddComponent<RawImage>();
    }

    protected RectTransform CreateChild(string name, Transform parent = null)
    {
        GameObject gameObject = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(gameObject, "Created " + name);
        RectTransform rect = gameObject.AddComponent<RectTransform>();
        if (parent != null)
            gameObject.transform.SetParent(parent);
        else
        if (objectIsSelected)
            gameObject.transform.SetParent(selObj.transform);
        gameObject.transform.localPosition = Vector3.zero;
        return rect;
    }

    #endregion objectCreators
    protected Transform GetUIParent()
    {
        if (objectIsSelected)
            return selObj.transform;
        else
            return GetCanvasTransform().transform;
    }
    #region layoutFunctionality

    protected void CreateParentPanel()
    {
        RectTransform rect = CreatePanel(true).GetComponent<RectTransform>();
        Vector2 size = rect.sizeDelta;
        Debug.Log(size);
        size = new Vector2(rect.rect.width, rect.rect.height);
        rect.anchorMin = Vector2.one / 2;
        rect.anchorMax = rect.anchorMin;
        rect.sizeDelta = size;
        rect.localScale = Vector3.one;
        rect.position = selObj.transform.position;
        rect.SetParent(selObj.transform.parent);
        rect.SetSiblingIndex(selObj.transform.GetSiblingIndex());
        LayoutElement le = rect.gameObject.AddComponent<LayoutElement>();
        le.preferredHeight = rect.rect.height;
        le.preferredWidth = rect.rect.width;
        rect.gameObject.GetComponent<Image>().enabled = false;
        rect.name = "Parented " + selObj.name;
        Undo.RecordObject(selObj, "object moved");
        selObj.transform.SetParent(rect);
        selObj = rect.gameObject;
    }
    protected void SwitchAutoLayoutType(GameObject gameObject)
    {
        Undo.RecordObject(gameObject, "Layout");
        HorizontalLayoutGroup horiz = gameObject.GetComponent<HorizontalLayoutGroup>();
        VerticalLayoutGroup vert = gameObject.GetComponent<VerticalLayoutGroup>();
        float spacing = 5;
        int hasLayout = 0;
        RectOffset rectOffset = new RectOffset();
        if (horiz != null)
        {
            hasLayout = 1;
            rectOffset = horiz.padding;
            spacing = horiz.spacing;
        }
        else if (vert != null)
        {
            hasLayout = 2;
            rectOffset = vert.padding;
            spacing = vert.spacing;
        }

        int newLayout = GUILayout.Toolbar(hasLayout, layouts);
        if (hasLayout != newLayout)
        {
            if (horiz != null) DestroyImmediate(horiz);
            if (vert != null) DestroyImmediate(vert);
            if (newLayout == 0) return;
            if (newLayout == 1)
            {
                HorizontalLayoutGroup hg = gameObject.AddComponent<HorizontalLayoutGroup>();
                hg.padding = rectOffset;
                hg.spacing = spacing;
                hg.childForceExpandHeight = false;
                hg.childForceExpandWidth = false;
                hg.childControlHeight = true;
                hg.childControlWidth = true;
                addLayoutElements(gameObject);
            }
            if (newLayout == 2)
            {
                VerticalLayoutGroup vg = gameObject.AddComponent<VerticalLayoutGroup>();
                vg.padding = rectOffset;
                vg.spacing = spacing;
                vg.childForceExpandHeight = false;
                vg.childForceExpandWidth = false;
                vg.childControlHeight = true;
                vg.childControlWidth = true;
                addLayoutElements(gameObject);
            }
        }
    }

    protected void addLayoutElements(GameObject gameObject)
    {
        Transform active = gameObject.transform;
        for (int i = 0; i < active.childCount; i++)
        {
            GameObject thisChild = active.GetChild(i).gameObject;
            LayoutElement layoutElement = thisChild.GetComponent<LayoutElement>();
            if (layoutElement == null)
            {
                layoutElement = thisChild.AddComponent<LayoutElement>();
                RectTransform rect = thisChild.GetComponent<RectTransform>();
                layoutElement.preferredWidth = rect.rect.width;
                layoutElement.preferredHeight = rect.rect.height;
            }
        }
    }

    #endregion layoutFunctionality



    #endregion nonVirtual
}
