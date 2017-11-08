using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;


/// Zambari 2017
/// v.1.04
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
    static string[] tools = { "Components", "Layers", "Transform", "Text", "Misc", "Layout", "Config" };
    static string[] activeTools;
    string activeTool;
    static bool[] toolsHidden;
    Type[] listOfTypesToAdd = new Type[] { typeof(LayoutElement), typeof(Rigidbody), typeof(BrownianMotionZ), typeof(RawImage), typeof(Image), typeof(MeshCollider), typeof(SphereCollider), typeof(BoxCollider) };
    float scaleSliderVal;
    Color defaultTextColor = Color.white;

    string[] layouts = { "None", "Horizontal", "Vertical" };

    GUIStyle thisStyle;

    static int screenNr;
    void OnEnable()
    {
        SceneView.onSceneGUIDelegate += this.OnSceneGUI;
    }

    void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= this.OnSceneGUI;
    }
    void OnSceneGUI(SceneView sceneView)
    {
        if (activeTool != "Layout") return;
        if (Selection.activeGameObject == null) return;
        if (SceneView.lastActiveSceneView != null)
        {
            LayoutElement layoutElement = Selection.activeGameObject.GetComponent<LayoutElement>();
            if (layoutElement == null) return;
       //     RectTransform rect = Selection.activeGameObject.GetComponent<RectTransform>();
        //    Vector3[] corners = new Vector3[4];
         //   rect.GetWorldCorners(corners);

     float handleSize=50;
     float valueDivide=200;
            if (layoutElement.GetComponentInParent<HorizontalLayoutGroup>()!=null&& layoutElement.preferredWidth >0)
            {
                float newVal = Handles.ScaleSlider(layoutElement.preferredWidth, layoutElement.transform.position/*& (corners[2] + corners[3]) / 2*/, Vector3.right, Quaternion.identity, 50+ layoutElement.preferredWidth /3, 0);
                float delta = newVal - layoutElement.preferredWidth;
                if (delta != 0) delta /= valueDivide;
                if (delta>0||    layoutElement.preferredWidth>10)
                layoutElement.preferredWidth += delta;
            }

            if (layoutElement.GetComponentInParent<VerticalLayoutGroup>()!=null&& layoutElement.preferredHeight >0)
            {
                float newVal = Handles.ScaleSlider(layoutElement.preferredHeight,layoutElement.transform.position, -Vector3.up, Quaternion.identity,50+ layoutElement.preferredHeight /3, 0);
             //           float newVal = Handles.ScaleSlider(layoutElement.preferredHeight, (corners[3] + corners[0]) / 2, -Vector3.up, Quaternion.identity, handleSize, 0);
                float delta = newVal - layoutElement.preferredHeight;
                if (delta != 0) { delta /= valueDivide;
                if (delta>0||    layoutElement.preferredHeight>10)
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
        screenNr = GUILayout.Toolbar(screenNr, activeTools);
        GUILayout.Space(6);
        activeTool = activeTools[screenNr];
        if (activeTool == "Layers") drawTagger();
        if (activeTool == "Transform") drawTransform();
        if (activeTool == "Misc") drawMisc();
        if (activeTool == "Text") drawTexts();
        if (activeTool == "Components") drawComponents();
        if (activeTool == "Config") drawConfig();
        if (activeTool == "Layout") drawLayout();
    }
    RectTransform createChild(string name)
    {
        GameObject gameObject = new GameObject(name);
        Undo.RegisterCreatedObjectUndo(gameObject, "Created " + name);
        RectTransform rect = gameObject.AddComponent<RectTransform>();
        gameObject.transform.SetParent(Selection.activeGameObject.transform);
        gameObject.transform.localPosition = Vector3.zero;
        return rect;
    }
    GameObject createPanel()
    {
        GameObject gameObject = createChild("Panel").gameObject;
        Image i = gameObject.AddComponent<Image>();
        i.color = Color.black * 0.5f;
        return gameObject;
    }
    GameObject createText()
    {
        RectTransform rect = createChild("Text");
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMax = Vector2.zero;
        rect.offsetMin = Vector2.zero;
        Text text = rect.gameObject.AddComponent<Text>();
        text.text = "New Text";
        text.color = defaultTextColor;
        return rect.gameObject;
    }
    void drawLayout()
    {
        if (nothingSelected) return;



        if (Selection.objects.Length > 1)
        {
            GUILayout.Label("please select only one obejct");
            return;
        }
        GUILayout.Label("ParentLayout");
        if (Selection.activeGameObject.transform.parent != null)
            switchLayout(Selection.activeGameObject.transform.parent.gameObject);

        if (GUILayout.Button("create panel"))
        {

            Selection.activeGameObject = createPanel();
        }
        if (GUILayout.Button("create Text"))
        {

            Selection.activeGameObject = createText();
        }
        GUILayout.Label("Sub Layout");
        switchLayout(Selection.activeGameObject);
        

    }


    void switchLayout(GameObject gameObject)
    {
        Undo.RecordObject(gameObject,"Layout");
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
            if (newLayout == 0)
            {

                return;
            }
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

    bool nothingSelected { get { if (Selection.activeGameObject == null) { GUILayout.Label("nothing selected"); return true; } return false; } }
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
        {
            if (!toolsHidden[i]) newToolList.Add(tools[i]);
        }
        for (int i = 0; i < tools.Length - 1; i++)
        {
        }
        newToolList.Add(tools[tools.Length - 1]);
        activeTools = newToolList.ToArray();
        screenNr = activeTools.Length - 1;
        for (int i = 0; i < toolsHidden.Length; i++)
            PlayerPrefs.SetInt("zEditorTool_" + tools[i], (toolsHidden[i] ? 1 : 0));
    }

    void drawConfig()
    {
        for (int i = 0; i < tools.Length - 1; i++)
        {
            bool newVal = !GUILayout.Toggle(!toolsHidden[i], tools[i]);
            if (newVal != toolsHidden[i])
            {
                toolsHidden[i] = newVal;
                rebuldTools();
            }
        }
    }
    void drawTexts()
    {
        if (nothingSelected) return;
        performOnComponents<Text>(textChange, true);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("makeWhite"))
            performOnComponents<Text>((Text t) => t.color = Color.white, true);
        if (GUILayout.Button("makeBlack"))
            performOnComponents<Text>((Text t) => t.color = Color.black, true);
        GUILayout.EndHorizontal();

    }
    void textChange(Text t)
    {
        string currentText = t.text;
        string newText = GUILayout.TextArea(currentText);
        if (currentText == newText) return;
        t.text = newText;
        EditorGUIUtility.PingObject(t);

    }
    void performOnComponents<T>(Action<T> actionToPerform, bool children = false) where T : Component
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            if (!children)
            {
                T myComponent = Selection.gameObjects[i].GetComponent<T>();
                Undo.RecordObject(myComponent, "utility");
                actionToPerform(myComponent);
            }
            else
            {
                T[] myComponents = Selection.gameObjects[i].GetComponentsInChildren<T>();
                for (int j = 0; j < myComponents.Length; j++)
                {
                    Undo.RecordObject(myComponents[j], "utility");
                    actionToPerform(myComponents[j]);
                }
            }
        }
    }

    bool applyToChildren;
    string lastStatus;
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
    void drawComponentEditRemove()
    {
        Component[] components = Selection.activeGameObject.GetComponents<Component>();
        //    if (components == null)
        //      return;
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

    void OnSelectionChange()
    {
        if (Selection.activeGameObject != null)
            scaleSliderVal = Selection.activeGameObject.transform.localScale.x;
        Repaint();
    }
    void drawTransform()
    {
        if (nothingSelected) return;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset Local Position"))
            Selection.activeGameObject.transform.localPosition = Vector3.zero;
        if (GUILayout.Button("Reset Global Position"))
            Selection.activeGameObject.transform.position = Vector3.zero;

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset Local Rotation"))
            Selection.activeGameObject.transform.localRotation = Quaternion.identity;
        if (GUILayout.Button("Reset Global Rotation"))
            Selection.activeGameObject.transform.rotation = Quaternion.identity;

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Reset Scale"))
            Selection.activeGameObject.transform.localScale = Vector3.one;
        EditorGUILayout.EndHorizontal();
        Vector3 currentScale = Selection.activeGameObject.transform.localScale;
        if (currentScale.x == currentScale.y && currentScale.y == currentScale.z)
            scaleSliderVal = GUILayout.HorizontalSlider(currentScale.x, 0.001f, 2);
        if (scaleSliderVal != currentScale.x) Selection.activeGameObject.transform.localScale = new Vector3(scaleSliderVal, scaleSliderVal, scaleSliderVal);
        drawflipAxis();
    }
    void drawflipAxis()
    {
        EditorGUILayout.BeginHorizontal();
        Vector3 cur = Selection.activeGameObject.transform.eulerAngles;
        if (GUILayout.Button("x +"))
            Selection.activeGameObject.transform.localEulerAngles = new Vector3(Mathf.Round(cur.x + 90), cur.y, cur.z);
        if (GUILayout.Button("x -"))
            Selection.activeGameObject.transform.localEulerAngles = new Vector3(Mathf.Round(cur.x - 90), cur.y, cur.z);
        if (GUILayout.Button("y +"))
            Selection.activeGameObject.transform.localEulerAngles = new Vector3(cur.x, Mathf.Round(cur.y + 90), cur.z);
        if (GUILayout.Button("y +"))
            Selection.activeGameObject.transform.localEulerAngles = new Vector3(cur.x, Mathf.Round(cur.y - 90), cur.z);
        if (GUILayout.Button("z +"))
            Selection.activeGameObject.transform.localEulerAngles = new Vector3(cur.x, cur.y, Mathf.Round(cur.z + 90));
        if (GUILayout.Button("z -"))
            Selection.activeGameObject.transform.localEulerAngles = new Vector3(cur.x, cur.y, Mathf.Round(cur.z - 90));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("A"))
        {
            Selection.activeGameObject.transform.position = new Vector3(0, 0, 0);
            Selection.activeGameObject.transform.eulerAngles = new Vector3(-45, 0, 0);

        }

        if (GUILayout.Button("B"))
        {
            Selection.activeGameObject.transform.position = new Vector3(0, 0, 0);
            Selection.activeGameObject.transform.eulerAngles = new Vector3(-45, -90, 0);

        }
        if (GUILayout.Button("A front"))
        {
            Selection.activeGameObject.transform.position = new Vector3(0, -0.8f, -2);
            Selection.activeGameObject.transform.eulerAngles = new Vector3(0, 0, 0);

        }
        if (GUILayout.Button("B Front"))
        {
            Selection.activeGameObject.transform.position = new Vector3(2, -0.8f, 0);
            Selection.activeGameObject.transform.eulerAngles = new Vector3(0, -90, 0);

        }
        if (GUILayout.Button("Reset"))
        {
            Selection.activeGameObject.transform.position = new Vector3(0, 0, 0);
            Selection.activeGameObject.transform.eulerAngles = new Vector3(0, 0, 0);

        }
        EditorGUILayout.EndHorizontal();
    }
    GameObject handleParentSelectionContext(GameObject newGo)
    {
        Undo.RegisterCreatedObjectUndo(newGo, "created via helper");
        if (Selection.activeGameObject != null)
        {
            newGo.transform.SetParent(Selection.activeGameObject.transform);
            newGo.transform.position = Vector3.zero;
            newGo.transform.rotation = Quaternion.identity;
        }
        Selection.activeGameObject = newGo;
        return newGo;
    }


    void setTags(string tag)
    {

        if (applyToChildren)
        {
            for (int i = 0; i < Selection.gameObjects.Length; i++)
            {
                Selection.gameObjects[i].tag = tag;
            }
        }
        else
            Selection.activeGameObject.tag = tag;
    }
    public static string[] tags = { "None", "Layer0", "Layer1", "Layer2", "Layer3" };
    void drawTagger()
    {

        applyToChildren = GUILayout.Toggle(applyToChildren, "Apply to children");
        if (nothingSelected) return;
        string currentTag = Selection.activeGameObject.tag;
        for (int i = 0; i < tags.Length; i++)
        {
            if (tags[i] == currentTag)
            {
                if (GUILayout.Button(tags[i], EditorStyles.toolbarButton))
                    setTags(tags[i]);
            }
            else
            {
                if (GUILayout.Button(tags[i])) setTags(tags[i]);
            }
        }

    }
    void drawMisc()
    {
        GUILayout.BeginHorizontal();
        if (Selection.activeGameObject != null)
            if (GUILayout.Button("Parent " + Selection.activeGameObject.name))
            {
                Transform sel = Selection.activeGameObject.transform;
                GameObject newGO = new GameObject("Parent " + Selection.activeGameObject.name);
                if (sel.parent != null)
                    newGO.transform.SetParent(sel.parent);

                newGO.transform.position = sel.position;
                newGO.transform.rotation = sel.rotation;
                newGO.transform.SetSiblingIndex(sel.GetSiblingIndex());
                sel.SetParent(newGO.transform);
            }
        if (GUILayout.Button("Create Child"))
            handleParentSelectionContext(new GameObject("gameobject"));
        if (Selection.activeGameObject != null && GUILayout.Button("as sibling"))
        {
            GameObject newGo = new GameObject("gameobject");
            Undo.RegisterCreatedObjectUndo(newGo, "created via helper");
            newGo.transform.SetParent(Selection.activeGameObject.transform.parent);
            newGo.transform.position = Selection.activeGameObject.transform.position;
            newGo.transform.rotation = Selection.activeGameObject.transform.rotation;
            newGo.transform.SetSiblingIndex(Selection.activeGameObject.transform.GetSiblingIndex() + 1);
            Selection.activeGameObject = newGo;
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
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("edit helper"))
            EditHelperCode();

    }
    float lastCloneSpread;
    void drawCloner()
    {

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("create 10 copies"))
        {
            Transform group = (new GameObject("Group")).transform;
            for (int i = 0; i < 10; i++)
            {
                GameObject newGo = Instantiate(Selection.activeGameObject);
                newGo.transform.position += (new Vector3(0.5f - UnityEngine.Random.value, 1 * i, 0.5f - UnityEngine.Random.value)) * lastCloneSpread;
                newGo.transform.SetParent(group);
            }
        }
        if (GUILayout.Button("create 10 sphere"))
        {
            Transform group = (new GameObject("Group")).transform;
            for (int i = 0; i < 10; i++)
            {
                GameObject newGo = Instantiate(Selection.activeGameObject);
                newGo.transform.position += Selection.activeGameObject.transform.position + UnityEngine.Random.onUnitSphere * lastCloneSpread;
                newGo.transform.SetParent(group);
            }
        }
        lastCloneSpread = GUILayout.HorizontalSlider(lastCloneSpread, 0.4f, 3);
        GUILayout.EndHorizontal();

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

    void collapseComponents(bool collapse)
    {
        Component[] components = Selection.activeGameObject.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(components[i], !collapse);
        }

        GameObject g = Selection.activeGameObject;
        Selection.activeGameObject = null;
        EditorApplication.delayCall += () => Selection.activeGameObject = g;
    }
    string stripUnity(string s)
    {

        if (s.Contains("UnityEngine"))
        {
            return s.Substring(12);
        }
        return s;
    }

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
