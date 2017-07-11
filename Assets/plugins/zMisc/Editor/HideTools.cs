#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class HideTools : MonoBehaviour
{
 

    [MenuItem("GameObject/Hide/Hide all on layer as current")]
    public static void HideAsCurrent()
    {
        if (Selection.activeGameObject==null) Debug.Log("need an object selected");
        int layer = Selection.activeGameObject.layer;
        Debug.Log("layer=" + layer);
        HideLayer(layer, true);
    }

    [MenuItem("GameObject/UnHide/Hide  all")]
    public static void UnhideallCurrent()
    {
        HideLayer(-1, false);
    }
     [MenuItem("GameObject/UnHide/Hide Layer 8")]
    public static void Unhide8()
    {
        HideLayer(8, false);
    }
 [MenuItem("GameObject/UnHide/Hide Layer 8")]
    public static void Unhide9()
    {
        HideLayer(9, false);
    }
     [MenuItem("GameObject/UnHide/Hide Layer 10")]
    public static void Unhide1()
    {
        HideLayer(10, false);
    }
     [MenuItem("GameObject/UnHide/Hide Layer 11")]
    public static void Unhide11()
    {
        HideLayer(11, false);
    }
     [MenuItem("GameObject/UnHide/Hide Layer 12")]
    public static void Unhide12()
    {
        HideLayer(12, false);
    }
     [MenuItem("GameObject/UnHide/Hide Layer 13")]
    public static void Unhide13()
    {
        HideLayer(8, false);
    }
  [MenuItem("GameObject/Hide/Hide Layer 8")]
    public static void Hide8()
    {
        HideLayer(8, true);
    }
 [MenuItem("GameObject/Hide/Hide Layer 8")]
    public static void Hide9()
    {
        HideLayer(9, true);
    }
     [MenuItem("GameObject/Hide/Hide Layer 10")]
    public static void Hide10()
    {
        HideLayer(10, true);
    }
     [MenuItem("GameObject/Hide/Hide Layer 11")]
    public static void Hide11()
    {
        HideLayer(11, true);
    }
     [MenuItem("GameObject/Hide/Hide Layer 12")]
    public static void Hide12()
    {
        HideLayer(12, true);
    }
     [MenuItem("GameObject/Hide/Hide Layer 13")]
    public static void Hide14()
    {
        HideLayer(8, true);
    }


         [MenuItem("GameObject/Hide/Hide Layer 15")]
    public static void Hide15()
    {
        HideLayer(15, true);
    }   [MenuItem("GameObject/UnHide/Hide Layer 15")]
    public static void Unhide15()
    {
        HideLayer(15, false);
    }
        [MenuItem("GameObject/Hide/Hide Layer 16")]
    public static void Hide16()
    {
        HideLayer(16, true);
    }   [MenuItem("GameObject/UnHide/Hide Layer 16")]
    public static void Unhide16()
    {
        HideLayer(16, false);
    }
    public static void Hide17()
    {
        HideLayer(17, true);
    }   [MenuItem("GameObject/UnHide/Hide Layer 17")]
    public static void Unhide17()
    {
        HideLayer(16, false);
    }


    public static void HideLayer(int layer, bool val)
    {
        Object[] allobj = Resources.FindObjectsOfTypeAll(typeof(GameObject));
        int c = 0;
        Debug.Log(" obj count " + allobj.Length);
        for (int i = 0; i < allobj.Length; i++)
        {
            GameObject g = allobj[i] as GameObject;
            if (g != null)
            {
                c++;
                if (g.layer == layer || layer == -1)
                    g.hideFlags = (val ? HideFlags.HideInHierarchy : HideFlags.None);
            }
        }
        Debug.Log(" changed " + c);
        EditorApplication.RepaintHierarchyWindow();
    }
}
#endif