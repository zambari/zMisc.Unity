using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;
using UnityEngine.EventSystems;

/// Zambari 2017
/// v.1.07 scale is now squared, moved to window 
/// 

public class zEditorHelper : zEditorTemplate
{
    [MenuItem("Window/Open zEditorHelper")]
    static void Init()
    {
        BaseInit(typeof(zEditorHelper));
    }
    protected override void AddTabs()
    {
        AddTab("CMP");
       // AddTab("LRS");
        AddTab("TRA");
        AddTab("TXT");
        AddTab("MSC");
        AddTab("LAY");
    }
    protected override void AddOptions()
    {
        AddOption("SetTextObjectName",true);

    }
    protected override void DisplayTab(string toolName)
    {
        if (toolName == "LRS") DisplayTagger();
        if (toolName == "TRA") DisplayTransform();
        if (toolName == "MSC") DisplayMisc();
        if (toolName == "TXT") DisplayTexts();
        if (toolName == "CMP") displayComponents();
        if (toolName == "LAY") DisplayLayout();
    }
    #region defines
    static string[] bgColors = { "SkyBox", "Black", "Bluish", "Random", "White" };
    Type[] listOfTypesToAdd = new Type[] { typeof(LayoutElement), typeof(Rigidbody)/*, typeof(BrownianMotionZ)*/, typeof(RawImage), typeof(Image), typeof(MeshCollider), typeof(SphereCollider), typeof(BoxCollider) };
    float scaleSliderVal;
    bool showParentLayout;
    float lastCloneSpread;
    bool applyToChildren;
    bool showTextsInLayout;
    public static string[] tags = { "None", "Layer0", "Layer1", "Layer2", "Layer3" };

    #endregion defines

    #region unity

    /*
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
                float newVal = Handles.ScaleSlider(layoutElement.preferredWidth, layoutElement.transform.position/*& (corners[2] + corners[3])  , Vector3.right, Quaternion.identity, 50 + layoutElement.preferredWidth / 3, 0);
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
 */


    #endregion unity
    #region tabsAndTools
    void DisplayMisc()
    {
        BH();
        if (objectIsSelected)
            if (Button("Parent " + selObj.name))
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
        IfButton("createChild", () => { HandleParentSelectionContext(new GameObject("gameobject")); });

        if (objectIsSelected && Button("as sibling"))
        {
            GameObject newGo = new GameObject("gameobject");
            Undo.RegisterCreatedObjectUndo(newGo, "created via helper");
            newGo.transform.SetParent(selObj.transform.parent);
            newGo.transform.position = selObj.transform.position;
            newGo.transform.rotation = selObj.transform.rotation;
            newGo.transform.SetSiblingIndex(selObj.transform.GetSiblingIndex() + 1);
            selObj = newGo;
        }

        EH();
        BH();
        IfButton("Plane", () => { HandleParentSelectionContext(GameObject.CreatePrimitive(PrimitiveType.Plane)); });
        IfButton("Sphere", () => { HandleParentSelectionContext(GameObject.CreatePrimitive(PrimitiveType.Sphere)); });
        IfButton("Cube", () => { HandleParentSelectionContext(GameObject.CreatePrimitive(PrimitiveType.Cube)); });
        IfButton("Capsule", () => { HandleParentSelectionContext(GameObject.CreatePrimitive(PrimitiveType.Capsule)); });
        IfButton("RawImage", () => { CreateRawImage(); });
        EH();
        DisplayCloner();
        FlexibleSpace();


    }

    void DisplayComponentEditRemove()
    {
        Component[] components = selObj.GetComponents<Component>();
        //EditorGUILayout.BeginVertical();
        BV();
        {
            BH();
            if (GUILayout.Button("collapse all", EditorStyles.miniButton))
                CollapseComponents(true);
            if (GUILayout.Button("expand all", EditorStyles.miniButton))
                CollapseComponents(false);
            EH();
            Label("Edit/Remove");
            for (int i = 1; i < components.Length; i++)
            {
                BH();
                Component thisComponent = components[i];
                if (thisComponent == null) break;
                string fullName = thisComponent.GetType().ToString();
                string labelString = StripUnityComponentName(fullName);
                if (GUILayout.Button("X", buttonStyle, GUILayout.Width(20)))
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
                    if (GUILayout.Button("Edit", buttonStyle, GUILayout.Width(40)))
                        EditComponentSource(thisComponent, 0);
                Label(labelString);
                EH();
            }
        }
        FlexibleSpace();
        EV();
    }



    void displayComponents()
    {
        if (nothingSelected) return;
        BH();
        DisplayComponentEditRemove();
        DisplayComponentAdd();
        EH();
        FlexibleSpace();
    }
    void DisplayCloner()
    {
        BH();
        if (Button("create 10 copies"))
        {
            Transform group = (new GameObject("Group")).transform;
            for (int i = 0; i < 10; i++)
            {
                GameObject newGo = Instantiate(selObj);
                newGo.transform.position += (new Vector3(0.5f - UnityEngine.Random.value, 1 * i, 0.5f - UnityEngine.Random.value)) * lastCloneSpread;
                newGo.transform.SetParent(group);
            }
        }
        if (Button("create 10 sphere"))
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
        EH();
    }

    void DisplayTagger()
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
                if (Button(tags[i])) setTags(tags[i]);
        }
    }
    void DisplayDefauttTextColor()
    {
        BH();
        Label("Default text color");
        defaultTextColor = EditorGUILayout.ColorField(defaultTextColor);
        EH();
    }

    GameObject lastCreatedObject;
    void DisplayLayout()
    {
        if (objectIsSelected)
        {
            Label("Images");
            DisplayImageColorHelper();
            Label("Texts");
            DisplayImageColorHelper(true);
        }
        if (Selection.objects.Length > 1)
        {
            Label("please select only one obejct");
            return;
        }

        if (showParentLayout)
            if (objectIsSelected && selObj.transform.parent != null)
            {
                Label("ParentLayout");
                SwitchAutoLayoutType(selObj.transform.parent.gameObject);
            }
        BH();
        if (Button("create panel"))

            if (Button("create panel"))
            {
                GameObject myNewObject = CreatePanel(false);
                selObj = myNewObject;
                lastCreatedObject = selObj;
            }
        if (Button("Filling panel")) selObj = CreatePanel(true);
        if (objectIsSelected) Button("Parent panel", CreateParentPanel);
        EH();

        if (Button("Filling Text")) selObj = createText();
        if (objectIsSelected)
        {
            Label("Sub Layout");
            SwitchAutoLayoutType(selObj);
        }
        GUILayout.Space(10);
        if (showTextsInLayout) DisplayTexts();
        GUILayout.Space(10);
        if (objectIsSelected && selObj.transform.localScale != Vector3.one)
            if (Button("Reset Scale (is " + selObj.transform.localScale + ")"))
                PerformOnComponents<Transform>((Transform t) => { t.localScale = Vector3.one; });

    }
    void DisplayTransform()
    {
        if (nothingSelected) return;
        BH();
        if (Button("Reset Local Position"))
            selObj.transform.localPosition = Vector3.zero;
        if (Button("Reset Global Position"))
            selObj.transform.position = Vector3.zero;

        EH();
        BH();
        Button("Reset Local Rotation", () => selObj.transform.localRotation = Quaternion.identity);
        Button("Reset Global Rotation", () => selObj.transform.rotation = Quaternion.identity);


        EH();
        BH();
        Button("Reset Scale", () => selObj.transform.localScale = Vector3.one);
        EH();
        Vector3 currentScale = selObj.transform.localScale;
        if (currentScale.x == currentScale.y && currentScale.y == currentScale.z)
            scaleSliderVal = GUILayout.HorizontalSlider(Mathf.Sqrt(currentScale.x), 0.01f, 3);
        if (scaleSliderVal != currentScale.x) 
        { float f=scaleSliderVal*scaleSliderVal;
            Vector3 newScale=new Vector3(f, f, f);
            for (int i=0;i<Selection.gameObjects.Length;i++)
            {
                Undo.RecordObject(Selection.gameObjects[i],"scale change");
                Selection.gameObjects[i].transform.localScale=newScale;
            }
            
             //selObj.transform.localScale = new Vector3(scaleSliderVal, scaleSliderVal, scaleSliderVal);
        }
        DisplayflipAxis();
    }

    protected override void OnSelectionChange()
    {
        if (objectIsSelected)
            scaleSliderVal = selObj.transform.localScale.x;
        base.OnSelectionChange();
    }

    protected override void DisplayCustomConfig()
    {
        base.DisplayCustomConfig();
        if (GUILayout.Button("edit helper", GUILayout.Width(100))) EditHelperCode();
    }

    void DisplayflipAxis()
    {
        BH();
        Vector3 cur = selObj.transform.eulerAngles;
        Button("x +", () => selObj.transform.localEulerAngles = new Vector3(Mathf.Round(cur.x + 90), cur.y, cur.z));
        Button("x -", () => selObj.transform.localEulerAngles = new Vector3(Mathf.Round(cur.x - 90), cur.y, cur.z));
        Button("y +", () => selObj.transform.localEulerAngles = new Vector3(cur.x, Mathf.Round(cur.y + 90), cur.z));
        Button("y +", () => selObj.transform.localEulerAngles = new Vector3(cur.x, Mathf.Round(cur.y - 90), cur.z));
        Button("z +", () => selObj.transform.localEulerAngles = new Vector3(cur.x, cur.y, Mathf.Round(cur.z + 90)));
        Button("z -", () => selObj.transform.localEulerAngles = new Vector3(cur.x, cur.y, Mathf.Round(cur.z - 90)));
        EH();
        BH();

        Button("A", () =>
         {
             selObj.transform.position = new Vector3(0, 0, 0);
             selObj.transform.eulerAngles = new Vector3(-45, 0, 0);
         });

        Button("B", () =>
     {
         selObj.transform.position = new Vector3(0, 0, 0);
         selObj.transform.eulerAngles = new Vector3(-45, -90, 0);
     });
        Button("A front", () =>
          {
              selObj.transform.position = new Vector3(0, -0.8f, -2);
              selObj.transform.eulerAngles = new Vector3(0, 0, 0);
          });
        Button("B Front", () =>
         {
             selObj.transform.position = new Vector3(2, -0.8f, 0);
             selObj.transform.eulerAngles = new Vector3(0, -90, 0);
         });
        Button("Reset", () =>
           {
               selObj.transform.position = new Vector3(0, 0, 0);
               selObj.transform.eulerAngles = new Vector3(0, 0, 0);
           });
        EH();
    }
    void DisplayComponentAdd()
    {
        EditorGUILayout.BeginVertical();
        Label("Add");
        for (int i = 0; i < listOfTypesToAdd.Length; i++)
            if (GUILayout.Button(StripUnityComponentName(listOfTypesToAdd[i].ToString()), buttonStyle))
                for (int k = 0; k < Selection.gameObjects.Length; k++)
                {
                    GameObject thisObject = Selection.gameObjects[k];
                    if (thisObject.GetComponent(listOfTypesToAdd[i]) == null)
                    {
                        Component thisComponent = Undo.AddComponent(thisObject, listOfTypesToAdd[i]);
                        if (listOfTypesToAdd[i] == typeof(Rigidbody))
                            ((Rigidbody)thisComponent).isKinematic = true;
                    }
                }
        EV();
    }
    void DisplayCameraBg()
    {
        if (Camera.main == null) return;
        Color current = Camera.main.backgroundColor;
        BH();
        bool isSOlid = Camera.main.clearFlags == CameraClearFlags.SolidColor;
        if (GUILayout.Toggle(isSOlid, "Camera background solid"))
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = EditorGUILayout.ColorField(Camera.main.backgroundColor);
        }
        else
            Camera.main.clearFlags = CameraClearFlags.Skybox;

        EH();
    }

    Vector2 scrollPos;

    //enum AnchorNames  {  UpperLeft,UpperCenter,UpperRight,MiddleLeft,MiddleCenter,MiddleRight,LowerLeft,LowerCenter,LowerRight};/
    //  string[] vertAlign = { "Upper", "Middle", "Lower" };
    //    string[] horizAlign = { "Upper", "Middle", "Lower" };

    enum VertAlign { Upper, Middle, Lower };
    enum HorizAlign { Left, Middle, Right };
    int currentAnchror;

    HorizAlign getHorizIndex(TextAnchor anchor)
    {
        if (anchor == TextAnchor.UpperLeft || anchor == TextAnchor.MiddleLeft || anchor == TextAnchor.LowerLeft)
            return HorizAlign.Left;
        if (anchor == TextAnchor.UpperCenter || anchor == TextAnchor.MiddleCenter || anchor == TextAnchor.LowerCenter)
            return HorizAlign.Middle;
        return HorizAlign.Right;
    }

    VertAlign getVertIndex(TextAnchor anchor)
    {
        if (anchor == TextAnchor.UpperLeft || anchor == TextAnchor.UpperCenter || anchor == TextAnchor.UpperRight)
            return VertAlign.Upper;
        if (anchor == TextAnchor.MiddleLeft || anchor == TextAnchor.MiddleCenter || anchor == TextAnchor.MiddleRight)
            return VertAlign.Middle;
        return VertAlign.Lower;
    }
    void DisplayTextAnchors()
    {
        if (selObj == null) return;

        Text text = selObj.GetComponentInChildren<Text>();
        if (text == null) return;

        HorizAlign currentHoriz = getHorizIndex(text.alignment);
        VertAlign currentVert = getVertIndex(text.alignment);
        BH();
        HorizAlign newCurrentHoriz = (HorizAlign)GUILayout.Toolbar((int)currentHoriz, Enum.GetNames(typeof(HorizAlign)));
        VertAlign newCurrentVert = (VertAlign)GUILayout.Toolbar((int)currentVert, Enum.GetNames(typeof(VertAlign)));

        if (newCurrentHoriz != currentHoriz)
        {
            PerformOnComponents<Text>((Text t) => { setTextHorizontal(t, newCurrentHoriz); });
        }
        else
        if (newCurrentVert != currentVert)
        {
            PerformOnComponents<Text>((Text t) => { setTextVertical(t, newCurrentVert); });
        }
        EH();

    }
    void setTextHorizontal(Text text, HorizAlign h)
    {
        VertAlign vert = getVertIndex(text.alignment);
        text.alignment = combineAnchors(h, vert);
    }
    void setTextVertical(Text text, VertAlign v)
    {
        HorizAlign h = getHorizIndex(text.alignment);
        text.alignment = combineAnchors(h, v);
    }
    TextAnchor combineAnchors(HorizAlign h, VertAlign v)
    {
        if (h == HorizAlign.Left)
        {
            if (v == VertAlign.Upper) return TextAnchor.UpperLeft;
            if (v == VertAlign.Middle) return TextAnchor.MiddleLeft;
            if (v == VertAlign.Lower) return TextAnchor.LowerLeft;
        }
        if (h == HorizAlign.Middle)
        {
            if (v == VertAlign.Upper) return TextAnchor.UpperCenter;
            if (v == VertAlign.Middle) return TextAnchor.MiddleCenter;
            if (v == VertAlign.Lower) return TextAnchor.LowerCenter;
        }
        if (h == HorizAlign.Right)
        {
            if (v == VertAlign.Upper) return TextAnchor.UpperRight;
            if (v == VertAlign.Middle) return TextAnchor.MiddleRight;
            if (v == VertAlign.Lower) return TextAnchor.LowerRight;
        }
        return TextAnchor.MiddleCenter;
    }


    void DisplayTexts()
    {
        if (nothingSelected) return;
        

        DisplayTextAnchors();
        BH();
        if (selObj.GetComponentInChildren<Text>() != null)
        {
            if (Button("makeWhite"))
                PerformOnComponents<Text>((Text t) => t.color = Color.white, true);
            if (Button("makeBlack"))
                PerformOnComponents<Text>((Text t) => t.color = Color.black, true);
        }

        EH();
        
        PerformOnComponents<Text>(TextObjectNameChange, true);
    }
    Color DisplayColorPicker(Color c)
    {
        BH();
        Color newColor = EditorGUILayout.ColorField(c);

        float a = newColor.a;
        Button("black", () => newColor = Color.black * a);
        Button("white", () => newColor = Color.white * a);
        Button("red", () => newColor = Color.red * a);
        Button("green", () => newColor = Color.green * a);
        Button("blue", () => newColor = Color.blue * a);


        EH();
        float newA = GUILayout.HorizontalSlider(a, 0, 1);
        if (newA != a) newColor = new Color(newColor.r, newColor.g, newColor.b, newA);
        return newColor;

    }
    void DisplayImageColorHelper(bool changeText = false)
    {
        if (selObj == null) return;
        if (!changeText)
        {
            Image image = selObj.GetComponent<Image>();
            if (image == null) return;

            Color newColor = DisplayColorPicker(image.color);
            if (image.color != newColor)
            {
                PerformOnComponents<Image>((Image i) => { i.color = newColor; });
            }
        }
        else
        {

            if (!changeText)
            {
                Text t = selObj.GetComponent<Text>();
                if (t == null) return;

                Color newColor = DisplayColorPicker(t.color);
                if (t.color != newColor)
                {
                    PerformOnComponents<Text>((Text i) => { i.color = newColor; });
                }
            }

        }
    }

    #endregion tabsAndTools

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


    #endregion functionality

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

