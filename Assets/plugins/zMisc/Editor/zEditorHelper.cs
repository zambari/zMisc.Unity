using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using System.IO;


/// Zambari 2017
/// v.1.03
/// 
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

public class zEditorHelper : EditorWindow
{
    Type[] listOfTypesToAdd = new Type[] { typeof(Rigidbody), typeof(BrownianMotionZ), typeof(RawImage), typeof(Image), typeof(MeshCollider), typeof(SphereCollider), typeof(BoxCollider) };
    float scaleSliderVal;
    public static string[] tags = { "None", "Layer0", "Layer1", "Layer2", "Layer3" };
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
        if (Selection.activeGameObject == null)
        {
            GUILayout.Label("nothing selected");
            return;
        }
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
    GUIStyle thisStyle;
    [MenuItem("Tools/Open zEditorHelper")]
    static void Init()
    {
        zEditorHelper window =
            (zEditorHelper)EditorWindow.GetWindow(typeof(zEditorHelper));
        window.Show();
    }
    int screeenNr;
    string[] tools = { " Components", "Tagger", "Transform", "Misc" };
    void OnSelectionChange()
    {
        if (Selection.activeGameObject != null)
            scaleSliderVal = Selection.activeGameObject.transform.localScale.x;
        Repaint();
    }
    void drawTransform()
    {
        if (Selection.activeGameObject == null)
        {
            GUILayout.Label("nothing selected");
            return;
        }
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

    void drawTagger()
    {
        applyToChildren = GUILayout.Toggle(applyToChildren, "Apply to children");
        if (Selection.activeGameObject == null)
        {
            GUILayout.Label("nothing to tag");
            return;
        }
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
                
                newGO.transform.position=sel.position;
                newGO.transform.rotation=sel.rotation;
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
            newGo.transform.position=Selection.activeGameObject.transform.position;
            newGo.transform.rotation= Selection.activeGameObject.transform.rotation;
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
    void OnGUI()
    {
        GUILayout.Space(6);
        thisStyle = EditorStyles.miniButton;
        screeenNr = GUILayout.Toolbar(screeenNr, tools);
        GUILayout.Space(6);
        if (screeenNr == 0) drawComponents();
        if (screeenNr == 1) drawTagger();
        if (screeenNr == 2) drawTransform();
        if (screeenNr == 3) drawMisc();
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
