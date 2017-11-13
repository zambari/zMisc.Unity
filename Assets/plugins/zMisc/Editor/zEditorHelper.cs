using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;
using UnityEngine.EventSystems;

/// Zambari 2017
/// v.1.05
/// 

public class zEditorHelper : EditorWindow
{
    [MenuItem("Tools/Open zEditorHelper")]
    static void Init()
    {
        zEditorHelper window =
            (zEditorHelper)EditorWindow.GetWindow(typeof(zEditorHelper));
        rebuldTools();

    }
    #region defines
    static string[] tools = { "Components", "Layers", "Transform", "Text", "Misc", "Layout", "Config" };
    static string[] bgColors = { "SkyBox", "Black", "Bluish", "Random", "White" };
    static string[] activeTools;
    string activeTool;
    static bool[] toolsHidden;
    Type[] listOfTypesToAdd = new Type[] { typeof(LayoutElement), typeof(Rigidbody), typeof(BrownianMotionZ), typeof(RawImage), typeof(Image), typeof(MeshCollider), typeof(SphereCollider), typeof(BoxCollider) };
    float scaleSliderVal;
    Color defaultTextColor = Color.white;
    string[] layouts = { "None", "Horizontal", "Vertical" };
    GUIStyle thisStyle;
    bool showParentLayout;
    float lastCloneSpread;
    static int _screenNr;

    bool applyToChildren;
    string lastStatus;

    bool objectCacheValid;
    List<Component> listOfComponents;
    public static string[] tags = { "None", "Layer0", "Layer1", "Layer2", "Layer3" };
    static int screenNr
    {
        get { return _screenNr; }
        set
        {
            if (_screenNr != value) PlayerPrefs.SetInt("zEditorTool_lastTool", value);
            _screenNr = value;
        }
    }

    bool objectIsSelected { get { return Selection.activeGameObject != null; } }
    bool nothingSelected { get { if (selObj == null) { GUILayout.Label("nothing selected"); return true; } return false; } }
    GameObject selObj
    {
        get { return Selection.activeGameObject; }
        set { Selection.activeGameObject = value; ; }
    }
    #endregion defines
  
    #region unity
    void OnEnable()
    {
        screenNr = PlayerPrefs.GetInt("zEditorTool_lastTool", 0);
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }
    void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }


    void OnSelectionChange()
    {
        objectCacheValid = false;
        if (selObj != null)
            scaleSliderVal = selObj.transform.localScale.x;
        Repaint();
    }
    void OnSceneGUI(SceneView sceneView)
    {
        if (activeTool != "Layout") return;
        if (selObj == null) return;
        if (SceneView.lastActiveSceneView != null)
        {
            LayoutElement layoutElement = selObj.GetComponent<LayoutElement>();
            if (layoutElement == null) return;
            float valueDivide = 200;
            if (layoutElement.GetComponentInParent<HorizontalLayoutGroup>() != null && layoutElement.preferredWidth > 0)
            {
                float newVal = Handles.ScaleSlider(layoutElement.preferredWidth, layoutElement.transform.position/*& (corners[2] + corners[3]) / 2*/, Vector3.right, Quaternion.identity, 50 + layoutElement.preferredWidth / 3, 0);
                float delta = newVal - layoutElement.preferredWidth;
                if (delta != 0) delta /= valueDivide;
                if (delta > 0 || layoutElement.preferredWidth > 10)
                    layoutElement.preferredWidth += delta;
            }
            if (layoutElement.GetComponentInParent<VerticalLayoutGroup>() != null && layoutElement.preferredHeight > 0)
            {
                float newVal = Handles.ScaleSlider(layoutElement.preferredHeight, layoutElement.transform.position, -Vector3.up, Quaternion.identity, 50 + layoutElement.preferredHeight / 3, 0);
                //           float newVal = Handles.ScaleSlider(layoutElement.preferredHeight, (corners[3] + corners[0]) / 2, -Vector3.up, Quaternion.identity, handleSize, 0);
                float delta = newVal - layoutElement.preferredHeight;
                if (delta != 0)
                {
                    delta /= valueDivide;
                    if (delta > 0 || layoutElement.preferredHeight > 10)
                        layoutElement.preferredHeight += delta;
                }

            }

        }
    }
    void OnGUI()
    {
        if (toolsHidden == null) rebuldTools();
        GUILayout.Space(6);
        thisStyle = EditorStyles.miniButton;
        int newScreen = GUILayout.Toolbar(screenNr, activeTools);
        if (newScreen != screenNr)
        {
            objectCacheValid = false;
            screenNr = newScreen;
        }

        GUILayout.Space(6);
        if (screenNr >= activeTools.Length) screenNr = activeTools.Length - 1;
        activeTool = activeTools[screenNr];
        if (activeTool == "Layers") drawTagger();
        if (activeTool == "Transform") drawTransform();
        if (activeTool == "Misc") drawMisc();
        if (activeTool == "Text") drawTexts();
        if (activeTool == "Components") drawComponents();
        if (activeTool == "Config") drawConfig();
        if (activeTool == "Layout") drawLayout();
    }
    #endregion unity
    #region tabsAndTools
    void drawMisc()
    {
        GUILayout.BeginHorizontal();
        if (selObj != null)
            if (GUILayout.Button("Parent " + selObj.name))
            {
                Transform sel = selObj.transform;
                GameObject newGO = new GameObject("Parent " + selObj.name);
                if (sel.parent != null)
                    newGO.transform.SetParent(sel.parent);

                newGO.transform.position = sel.position;
                newGO.transform.rotation = sel.rotation;
                newGO.transform.SetSiblingIndex(sel.GetSiblingIndex());
                sel.SetParent(newGO.transform);
            }
        if (GUILayout.Button("Create Child"))
            handleParentSelectionContext(new GameObject("gameobject"));
        if (selObj != null && GUILayout.Button("as sibling"))
        {
            GameObject newGo = new GameObject("gameobject");
            Undo.RegisterCreatedObjectUndo(newGo, "created via helper");
            newGo.transform.SetParent(selObj.transform.parent);
            newGo.transform.position = selObj.transform.position;
            newGo.transform.rotation = selObj.transform.rotation;
            newGo.transform.SetSiblingIndex(selObj.transform.GetSiblingIndex() + 1);
            selObj = newGo;
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Plane"))
            handleParentSelectionContext(GameObject.CreatePrimitive(PrimitiveType.Plane));
        if (GUILayout.Button("Sphere"))
            handleParentSelectionContext(GameObject.CreatePrimitive(PrimitiveType.Sphere));
        if (GUILayout.Button("Cube"))
            handleParentSelectionContext(GameObject.CreatePrimitive(PrimitiveType.Cube));
        if (GUILayout.Button("Capsule"))
            handleParentSelectionContext(GameObject.CreatePrimitive(PrimitiveType.Capsule));
        if (GUILayout.Button("RawImage"))
            createRawImage();
        GUILayout.EndHorizontal();

        drawCloner();
        /*   GUILayout.BeginHorizontal();

           if (selObj!=null && GUILayout.Button("DumpStructureToConsole"))
               dumpStructure();

           GUILayout.EndHorizontal();*/

        GUILayout.FlexibleSpace();
        if (GUILayout.Button("edit helper"))
            EditHelperCode();

    }

    void drawComponentEditRemove()
    {
        Component[] components = selObj.GetComponents<Component>();
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("collapse all", EditorStyles.miniButton))
                collapseComponents(true);
            if (GUILayout.Button("expand all", EditorStyles.miniButton))
                collapseComponents(false);
            EditorGUILayout.EndHorizontal();
            GUILayout.Label("Edit/Remove");
            for (int i = 1; i < components.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                Component thisComponent = components[i];
                if (thisComponent == null) break;
                string fullName = thisComponent.GetType().ToString();
                string labelString = stripUnity(fullName);
                if (GUILayout.Button("X", thisStyle, GUILayout.Width(20)))
                {
                    Type thisComponentType = thisComponent.GetType();
                    for (int j = 0; j < Selection.gameObjects.Length; j++)
                    {
                        GameObject thisObj = Selection.gameObjects[j];
                        Component thisIterateComponent = thisObj.GetComponent(thisComponentType);
                        if (thisIterateComponent != null)
                            Undo.DestroyObjectImmediate(thisIterateComponent);
                    }
                }
                if (fullName == labelString)
                    if (GUILayout.Button("Edit", thisStyle, GUILayout.Width(40)))
                        EditComponentSource(thisComponent, 0);
                GUILayout.Label(labelString);
                EditorGUILayout.EndHorizontal();
            }
        }
        GUILayout.FlexibleSpace();

        EditorGUILayout.EndVertical();
    }
    void drawComponents()
    {
        if (nothingSelected) return;
        EditorGUILayout.BeginHorizontal();
        drawComponentEditRemove();
        drawComponentAdd();
        EditorGUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
    }
    void drawCloner()
    {

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("create 10 copies"))
        {
            Transform group = (new GameObject("Group")).transform;
            for (int i = 0; i < 10; i++)
            {
                GameObject newGo = Instantiate(selObj);
                newGo.transform.position += (new Vector3(0.5f - UnityEngine.Random.value, 1 * i, 0.5f - UnityEngine.Random.value)) * lastCloneSpread;
                newGo.transform.SetParent(group);
            }
        }
        if (GUILayout.Button("create 10 sphere"))
        {
            Transform group = (new GameObject("Group")).transform;
            for (int i = 0; i < 10; i++)
            {
                GameObject newGo = Instantiate(selObj);
                newGo.transform.position += selObj.transform.position + UnityEngine.Random.onUnitSphere * lastCloneSpread;
                newGo.transform.SetParent(group);
            }
        }
        lastCloneSpread = GUILayout.HorizontalSlider(lastCloneSpread, 0.4f, 3);
        GUILayout.EndHorizontal();

    }
    static void rebuldTools()
    {
        if (toolsHidden == null)
        {
            toolsHidden = new bool[tools.Length];
            for (int i = 0; i < toolsHidden.Length; i++)
                toolsHidden[i] = PlayerPrefs.GetInt("zEditorTool_" + tools[i], 0) == 1;
        }
        List<string> newToolList = new List<string>();
        for (int i = 0; i < tools.Length - 1; i++)
            if (!toolsHidden[i]) newToolList.Add(tools[i]);
        newToolList.Add(tools[tools.Length - 1]);
        activeTools = newToolList.ToArray();
        for (int i = 0; i < toolsHidden.Length; i++)
            PlayerPrefs.SetInt("zEditorTool_" + tools[i], (toolsHidden[i] ? 1 : 0));
    }

    void drawTagger()
    {
        applyToChildren = GUILayout.Toggle(applyToChildren, "Apply to children");
        if (nothingSelected) return;
        string currentTag = selObj.tag;
        for (int i = 0; i < tags.Length; i++)
        {
            if (tags[i] == currentTag)
            {
                if (GUILayout.Button(tags[i], EditorStyles.toolbarButton))
                    setTags(tags[i]);
            }
            else
                if (GUILayout.Button(tags[i])) setTags(tags[i]);
        }
    }
    void drawConfig()
    {
        GUILayout.Space(10);
        for (int i = 0; i < tools.Length - 1; i++)
        {
            bool newVal = !GUILayout.Toggle(!toolsHidden[i], tools[i]);
            if (newVal != toolsHidden[i])
            {
                toolsHidden[i] = newVal;
                rebuldTools();
            }
        }
        GUILayout.Space(10);
        showParentLayout = GUILayout.Toggle(showParentLayout, "Show Parent Auto Layout");
    }

    void drawLayout()
    {
        drawCameraBg();
        if (selObj != null)
        { drawImageColorHelper(); }
        if (Selection.objects.Length > 1)
        {
            GUILayout.Label("please select only one obejct");
            return;
        }

        if (showParentLayout)
            if (selObj != null && selObj.transform.parent != null)
            {
                GUILayout.Label("ParentLayout");
                switchLayout(selObj.transform.parent.gameObject);
            }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("create panel")) selObj = createPanel(false);
        if (GUILayout.Button("filling panel")) selObj = createPanel(true);
        if (selObj != null) if (GUILayout.Button("Parent panel")) createParentPanel();
        GUILayout.EndHorizontal();

        if (GUILayout.Button("create Text")) selObj = createText();

        if (selObj != null)
        {
            GUILayout.Label("Sub Layout");
            switchLayout(selObj);
        }

    }

    void drawTransform()
    {
        if (nothingSelected) return;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset Local Position"))
            selObj.transform.localPosition = Vector3.zero;
        if (GUILayout.Button("Reset Global Position"))
            selObj.transform.position = Vector3.zero;

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset Local Rotation"))
            selObj.transform.localRotation = Quaternion.identity;
        if (GUILayout.Button("Reset Global Rotation"))
            selObj.transform.rotation = Quaternion.identity;

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset Scale"))
            selObj.transform.localScale = Vector3.one;
        EditorGUILayout.EndHorizontal();
        Vector3 currentScale = selObj.transform.localScale;
        if (currentScale.x == currentScale.y && currentScale.y == currentScale.z)
            scaleSliderVal = GUILayout.HorizontalSlider(currentScale.x, 0.001f, 2);
        if (scaleSliderVal != currentScale.x) selObj.transform.localScale = new Vector3(scaleSliderVal, scaleSliderVal, scaleSliderVal);
        drawflipAxis();
    }

    void drawflipAxis()
    {
        EditorGUILayout.BeginHorizontal();
        Vector3 cur = selObj.transform.eulerAngles;
        if (GUILayout.Button("x +"))
            selObj.transform.localEulerAngles = new Vector3(Mathf.Round(cur.x + 90), cur.y, cur.z);
        if (GUILayout.Button("x -"))
            selObj.transform.localEulerAngles = new Vector3(Mathf.Round(cur.x - 90), cur.y, cur.z);
        if (GUILayout.Button("y +"))
            selObj.transform.localEulerAngles = new Vector3(cur.x, Mathf.Round(cur.y + 90), cur.z);
        if (GUILayout.Button("y +"))
            selObj.transform.localEulerAngles = new Vector3(cur.x, Mathf.Round(cur.y - 90), cur.z);
        if (GUILayout.Button("z +"))
            selObj.transform.localEulerAngles = new Vector3(cur.x, cur.y, Mathf.Round(cur.z + 90));
        if (GUILayout.Button("z -"))
            selObj.transform.localEulerAngles = new Vector3(cur.x, cur.y, Mathf.Round(cur.z - 90));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("A"))
        {
            selObj.transform.position = new Vector3(0, 0, 0);
            selObj.transform.eulerAngles = new Vector3(-45, 0, 0);
        }

        if (GUILayout.Button("B"))
        {
            selObj.transform.position = new Vector3(0, 0, 0);
            selObj.transform.eulerAngles = new Vector3(-45, -90, 0);
        }
        if (GUILayout.Button("A front"))
        {
            selObj.transform.position = new Vector3(0, -0.8f, -2);
            selObj.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (GUILayout.Button("B Front"))
        {
            selObj.transform.position = new Vector3(2, -0.8f, 0);
            selObj.transform.eulerAngles = new Vector3(0, -90, 0);
        }
        if (GUILayout.Button("Reset"))
        {
            selObj.transform.position = new Vector3(0, 0, 0);
            selObj.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        EditorGUILayout.EndHorizontal();
    }
    void drawComponentAdd()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Add");
        for (int i = 0; i < listOfTypesToAdd.Length; i++)
            if (GUILayout.Button(stripUnity(listOfTypesToAdd[i].ToString()), thisStyle))
                for (int k = 0; k < Selection.gameObjects.Length; k++)
                {
                    GameObject thisObject = Selection.gameObjects[k];
                    if (thisObject.GetComponent(listOfTypesToAdd[i]) == null)
                    {
                        Component thisComponent = Undo.AddComponent(thisObject, listOfTypesToAdd[i]);
                        // beghin handle specoa cases
                        if (listOfTypesToAdd[i] == typeof(Rigidbody))
                            ((Rigidbody)thisComponent).isKinematic = true;
                        if (listOfTypesToAdd[i] == typeof(BrownianMotionZ))
                        {
                            ((BrownianMotionZ)thisComponent).positionAmplitude = 0;
                            ((BrownianMotionZ)thisComponent).rotationAmplitude = 0;
                        }
                        // end handle special cases
                    }
                }
        EditorGUILayout.EndVertical();
    }
    void drawCameraBg()
    {
        if (Camera.main == null) return;
        Color current = Camera.main.backgroundColor;
        GUILayout.BeginHorizontal();
        bool isSOlid = Camera.main.clearFlags == CameraClearFlags.SolidColor;
        if (GUILayout.Toggle(isSOlid, "Camera background solid"))
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = EditorGUILayout.ColorField(Camera.main.backgroundColor);
        }
        else
            Camera.main.clearFlags = CameraClearFlags.Skybox;

        GUILayout.EndHorizontal();
    }


    void drawTexts()
    {
        if (nothingSelected) return;
        performOnComponents<Text>(textChange, true);
        GUILayout.BeginHorizontal();
        if (selObj.GetComponentInChildren<Text>() != null)
        {
            if (GUILayout.Button("makeWhite"))
                performOnComponents<Text>((Text t) => t.color = Color.white, true);
            if (GUILayout.Button("makeBlack"))
                performOnComponents<Text>((Text t) => t.color = Color.black, true);
        }
        GUILayout.EndHorizontal();
    }

    void drawImageColorHelper()
    {
        if (selObj == null) return;
        Image image = selObj.GetComponent<Image>();
        if (image == null) return;
        GUILayout.BeginHorizontal();
        Color newColor = EditorGUILayout.ColorField(image.color);

        float a = newColor.a;
        if (GUILayout.Button("black")) newColor = Color.black * a;
        if (GUILayout.Button("white")) newColor = Color.white * a;
        if (GUILayout.Button("red")) newColor = Color.red * a;
        if (GUILayout.Button("green")) newColor = Color.green * a;
        if (GUILayout.Button("blue")) newColor = Color.blue * a;


        GUILayout.EndHorizontal();
        float newA = GUILayout.HorizontalSlider(a, 0, 1);
        if (newA != a) newColor = new Color(newColor.r, newColor.g, newColor.b, newA);
        if (image.color != newColor)
        {
            performOnComponents<Image>((Image i) => { i.color = newColor; });
        }
    }

    #endregion tabsAndTools

    #region layoutFunctionality

    void createParentPanel()
    {
        RectTransform rect = createPanel(true).GetComponent<RectTransform>();
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
    void switchLayout(GameObject gameObject)
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
                addLayoutElenments(gameObject);
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
                addLayoutElenments(gameObject);
            }
        }
    }

    void addLayoutElenments(GameObject gameObject)
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
    #region functionality



    void EditHelperCode()
    {
        string finalFileName = Application.dataPath + "/Plugins/zMisc/Editor/zEditorHelper.cs";
        System.Diagnostics.ProcessStartInfo thisStartInfo = new System.Diagnostics.ProcessStartInfo();
        thisStartInfo.FileName = "code";
        thisStartInfo.Arguments = "\"" + finalFileName + "\"";
        thisStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        System.Diagnostics.Process.Start(thisStartInfo);

    }
    public static void EditComponentSource(Component component, int gotoLine)
    {
        string[] fileNames = Directory.GetFiles(Application.dataPath, component.GetType().ToString() + ".cs", SearchOption.AllDirectories);
        if (fileNames.Length > 0)
        {
            string finalFileName = Path.GetFullPath(fileNames[0]);
            System.Diagnostics.ProcessStartInfo thisStartInfo = new System.Diagnostics.ProcessStartInfo();
            thisStartInfo.FileName = "code";
            thisStartInfo.Arguments = "\"" + finalFileName + "\"";
            thisStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            System.Diagnostics.Process.Start(thisStartInfo);
        }
        else
            Debug.Log("File Not Found:" + component.GetType().ToString() + ".cs");
    }



    void setTags(string tag)
    {
        if (applyToChildren)
            for (int i = 0; i < Selection.gameObjects.Length; i++)
                Selection.gameObjects[i].tag = tag;
        else
            selObj.tag = tag;
    }
    GameObject handleParentSelectionContext(GameObject newGo)
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


    void textChange(Text t)
    {
        string currentText = t.text;
        string newText = GUILayout.TextArea(currentText);
        if (currentText == newText) return;
        t.text = newText;
        EditorGUIUtility.PingObject(t);
    }




    #endregion functionality

    #region objectCreators
    GameObject createPanel(bool filling = false)
    {

        GameObject gameObject = createChild("Panel", GetUIParent()).gameObject;

        Image i = gameObject.AddComponent<Image>();
        i.color = Color.black * 0.5f + (new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value)) * 0.2f;
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
    GameObject createText()
    {
        RectTransform rect = createChild("Text", GetUIParent());
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        Text text = rect.gameObject.AddComponent<Text>();
        text.text = "New Text";
        text.color = defaultTextColor;
        return rect.gameObject;
    }


    void createRawImage()
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

    RectTransform createChild(string name, Transform parent = null)
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

    /*
        void dumpObject(GameObject source)
        {
               RectTransform s=selObj.GetComponent<RectTransform>();
               System.Text.StringBuilder sb =new System.Text.StringBuilder();

               GameObject gameObject=new GameObject(selObj.name+"s");
               sb.Append("GameObject gameObject=new GameObject(\""); sb.Append(selObj.name); sb.Append("\"");
               RectTransform rect=gameObject.AddComponent<RectTransform>();
               rect.SetParent(selObj.transform.parent);       
               rect.localScale=new Vector3(s.localScale.x,s.localScale.y,s.localScale.z);
               rect.anchorMin=new Vector2(s.anchorMin.x,s.anchorMin.y);
               rect.anchorMax=new Vector2(s.anchorMax.x,s.anchorMax.y);
               rect.offsetMin=new Vector2(s.offsetMin.x,s.offsetMin.y);
               rect.offsetMax=new Vector2(s.offsetMax.x,s.offsetMax.y);
               rect.position=new Vector3(s.position.x,s.position.y,s.position.z);
                Debug.Log(sb.ToString());
        }
           void dumpStructure()
           {
               string dump="";

           }*/

    #region helpers

    Transform GetUIParent()
    {
        if (objectIsSelected)
            return selObj.transform;
        else
            return getCanvasTransform().transform;
    }


    void collapseComponents(bool collapse)
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
    string stripUnity(string s)
    {

        if (s.Contains("UnityEngine"))
        {
            return s.Substring(12);
        }
        return s;
    }
    T createObjectWithComponent<T>(string name) where T : Component
    {
        return createChild(name).gameObject.AddComponent<T>();
    }

    Transform getCanvasTransform()
    {
        Canvas c = GameObject.FindObjectOfType<Canvas>();
        if (c == null)
        {
            c = createObjectWithComponent<Canvas>("Canvas");
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler cs = c.gameObject.AddComponent<CanvasScaler>();
            cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            cs.referenceResolution = new Vector2(1920, 1080);
            c.gameObject.AddComponent<GraphicRaycaster>();
            EventSystem e = createObjectWithComponent<EventSystem>("EventSystem");
            e.gameObject.AddComponent<StandaloneInputModule>();
            c.transform.SetParent(null);
        }
        return c.transform;
    }

    /// <summary>
    /// Returns all the components of given type within selected gameObject (and their children with optional flag)
    /// </summary>
    void performOnComponents<T>(Action<T> actionToPerform, bool children = false) where T : Component
    {


        if (!objectCacheValid)
        {
            listOfComponents = new List<Component>();
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                if (!children)
                {
                    listOfComponents.Add(Selection.gameObjects[i].GetComponent<T>());
                }
                else
                {
                    T[] myComponents = Selection.gameObjects[i].GetComponentsInChildren<T>();
                    for (int j = 0; j < myComponents.Length; j++)
                    {
                        if (!listOfComponents.Contains(myComponents[j]))
                            listOfComponents.Add(myComponents[j]);

                    }
                }
            }
            objectCacheValid = true;
        }
        for (int i = 0; i < listOfComponents.Count; i++)
        {
            Undo.RecordObject(listOfComponents[i], "utility");
            actionToPerform((T)listOfComponents[i]);

        }


    }



    #endregion helpers
}


#if UNITY_55
public static class PosRot
{
    public static SetPositionAndRotation(this transform, Vector3 pos, Quaternion rot)
    {
        transform.position=pos;
        transform.rot=rot;
    }
}
#endif
