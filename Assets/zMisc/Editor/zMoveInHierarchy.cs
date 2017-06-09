using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
// quickly put together by zambari

public static class zMoveInHierarchyCommand
{
    [MenuItem("Tools/Hierarchy/Set First Child")]
    private static void SetFirstChild()
    {
        Selection.activeTransform.SetSiblingIndex(0);
    }
    [MenuItem("Tools/Hierarchy/Move Up Hierarchy &UP")]
    private static void MoveUpHierachy()
    {
        if (!Selection.activeTransform) return;
        try
        {
            int currentIndex = Selection.activeTransform.GetSiblingIndex();
            Selection.activeTransform.SetSiblingIndex(--currentIndex);
        }
        catch { Debug.Log("command failed"); }
    }
    [MenuItem("Tools/Hierarchy/Move Down Hierarchy &DOWN")]
    private static void DownInierachy()
    {
        if (!Selection.activeTransform) return;
        try
        {
            int currentIndex = Selection.activeTransform.GetSiblingIndex();
            Selection.activeTransform.SetSiblingIndex(++currentIndex);
        }
        catch { Debug.Log("command failed"); }
    }

    [MenuItem("Tools/Hierarchy/Bring Up a Level in Hierarchy (<-unparent) &LEFT")]
    private static void bringUpALevel()
    {
        if (!Selection.activeTransform) return;
        try
        {
            Transform parentTransform = Selection.activeTransform.parent;
            int parentIndex = parentTransform.GetSiblingIndex();
            Selection.activeTransform.parent = parentTransform.parent;
            Selection.activeTransform.SetSiblingIndex(parentIndex);
        }
        catch { Debug.Log("command failed"); }
    }
    [MenuItem("Tools/Hierarchy/Push Down a Level in Hierarchy (->parent to following) &RIGHT")]
    private static void bringDownALevel()
    {
        if (!Selection.activeTransform) return;
        try
        {
            int currentIndex = Selection.activeTransform.GetSiblingIndex();
            Transform nextTransform = Selection.activeTransform.parent.GetChild(currentIndex + 1);
            Selection.activeTransform.parent = nextTransform;
        }
        catch { Debug.Log("command failed"); }
    }

    [MenuItem("Tools/Canvas/DisableAllRaycastTargets")]
    private static void DisableRaycasts()
    {
        Image[] images = Selection.activeGameObject.GetComponentsInChildren<Image>();
        Text[] texts = Selection.activeGameObject.GetComponentsInChildren<Text>();
         RawImage[] raws = Selection.activeGameObject.GetComponentsInChildren<RawImage>();
    //    UILineRenderer[] lrs = Selection.activeGameObject.GetComponentsInChildren<UILineRenderer>();
        for (int i = 0; i < images.Length; i++)
            images[i].raycastTarget = false;
        for (int i = 0; i < texts.Length; i++)
            texts[i].raycastTarget = false;
   //     for (int i = 0; i < lrs.Length; i++)
   //         lrs[i].raycastTarget = false;
        for (int i = 0; i < raws.Length; i++)
            raws[i].raycastTarget = false;
    }
[MenuItem("Tools/Canvas/EnableAllRaycastTargets")]
    private static void EnableRaycasts()
    {
        Image[] images = Selection.activeGameObject.GetComponentsInChildren<Image>();
        Text[] texts = Selection.activeGameObject.GetComponentsInChildren<Text>();
         RawImage[] raws = Selection.activeGameObject.GetComponentsInChildren<RawImage>();
     //   UILineRenderer[] lrs = Selection.activeGameObject.GetComponentsInChildren<UILineRenderer>();
        for (int i = 0; i < images.Length; i++)
            images[i].raycastTarget = true;
        for (int i = 0; i < texts.Length; i++)
            texts[i].raycastTarget = true;
     //   for (int i = 0; i < lrs.Length; i++)
    //        lrs[i].raycastTarget = true;
        for (int i = 0; i < raws.Length; i++)
            raws[i].raycastTarget = true;
    }
}