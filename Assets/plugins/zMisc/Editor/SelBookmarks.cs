using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

/*
 Title : SelBookmarks ™ 
 Part of  Toucan Universal Libraries
 Autor by : Zambari at http://toucan-systems.pl
 
 MenuItem: Tools/Open Selection Bookmarks
 Editor window enabling you to remember and restore Hierarchy selections.
 Aslo searches for simiarily named objects within hierarchy (for example objects called Text).
 Theres also a shortcut for toggling the active state of bookmarked objects
 version 1.03    (2017.10.30) removed 'b'  shortcut
 version 1.02  mutliple selections for active toggle now working   (2017.10.11)
 version 1.01    (2017.09.04)
 

*/


public static class ObjectEnableToggle
{
  /*  [MenuItem("Tools/Actions/Boomark  _b")]
    static void BookmarkSelected()
    {
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            GameObject thisGameObject = Selection.gameObjects[i];
            SelBookmarks.AddBookmark(Selection.activeGameObject);
        }

    }*/

    [MenuItem("Tools/Actions/Toggle Enabled  _`")]
    static void toggleEnabled()
    {
        if (Selection.activeGameObject != null)
        {
            bool newActiveStatus = !Selection.activeGameObject.activeSelf;
            for (int i = 0; i < Selection.gameObjects.Length; i++)
                toggleActiveStatus(Selection.gameObjects[i], newActiveStatus);
        }
    }

    static void toggleActiveStatus(GameObject o, bool status)
    {
        o.SetActive(status);
        if (o.activeSelf
         && !o.activeInHierarchy)
        {
            Transform thisTransform = o.transform.parent;
            while (thisTransform != null)
            {
                if (thisTransform.gameObject.activeInHierarchy == false)
                    thisTransform.gameObject.SetActive(true);

                thisTransform = thisTransform.parent;
            }
        }


    }

}


public class SelBookmarks : EditorWindow
{ static SelBookmarks window ;
    [MenuItem("Tools/Open Selection Bookmarks")]
    static void Init()
    {
#pragma warning disable 219
         window =
            (SelBookmarks)EditorWindow.GetWindow(typeof(SelBookmarks));
#pragma warning restore 219
    }
    [SerializeField]
    static List<GameObject> bookmarkedObjects;
    GameObject[] namedTheSameLOnLevel0;
    GameObject[] namedTheSameLOnLevel1;
    GameObject[] namedTheSameLOnLevel2;
    GameObject[] namedTheSameLOnLevel3;
    object[] lastSelectionState;
    bool sorted;
    bool soloMode;
    public static void AddBookmark(GameObject bookmark)
    {
        if (bookmarkedObjects == null) bookmarkedObjects = new List<GameObject>();
        if (!bookmarkedObjects.Contains(bookmark))
        {
            bookmarkedObjects.Add(bookmark);
        }
        else
        {
            bookmarkedObjects.Remove(bookmark);
            bookmarkedObjects.Insert(0, bookmark);
        }
        if (window!=null)
            window.Repaint();
    }


    public static void removeBookmark(GameObject bookmark)
    {

    }
    void OnGUI()
    {
        if (bookmarkedObjects == null) bookmarkedObjects = new List<GameObject>();
        else
        {
            int i = 0;
            while (i < bookmarkedObjects.Count)
                if (bookmarkedObjects[i] == null) bookmarkedObjects.RemoveAt(i); else i++;
        }
        EditorGUILayout.BeginHorizontal();
        if (Selection.activeGameObject == null)
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(6);
            GUILayout.Label("No object selected");
            EditorGUILayout.EndVertical();
        }
        else

        if (GUILayout.Button("Bookmark Selected")) //, GUIStyle.none
            if (Selection.activeObject != null)
            {
                sorted = false;
                for (int i = 0; i < Selection.gameObjects.Length; i++)
                {
                    var thisGameObject = Selection.gameObjects[i];
                    AddBookmark(thisGameObject);
                }
            }
        if (bookmarkedObjects.Count > 1 && !sorted)
            if (GUILayout.Button("Sort")) //, GUIStyle.none
                sortObjects();

        soloMode = GUILayout.Toggle(soloMode, "Solo Mode");

        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        if (bookmarkedObjects.Count > 0)
            for (int i = 0; i < bookmarkedObjects.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Toggle(bookmarkedObjects[i].activeInHierarchy, ""))
                {
                    bookmarkedObjects[i].SetActive(true);
                    if (soloMode)
                    {
                        for (int k = 0; k < bookmarkedObjects.Count; k++)
                        {
                            if (i != k) bookmarkedObjects[k].SetActive(false);
                        }
                    }

                }
                else
                {
                    if (!soloMode)
                    {
                        bookmarkedObjects[i].SetActive(false);
                    }
                    /*  else
                      {   for (int k = 0; k < bookmarkedObjects.Count; k++)
                          {
                            bookmarkedObjects[k].SetActive(true);
                          }

                      }*/
                }

                if (GUILayout.Button("*", EditorStyles.label))
                    EditorGUIUtility.PingObject(bookmarkedObjects[i]);
                if (GUILayout.Button("" + bookmarkedObjects[i].name + " ", EditorStyles.miniButton))
                    Selection.activeGameObject = bookmarkedObjects[i];
                GUILayout.FlexibleSpace();
                if (i > 0)
                    if (GUILayout.Button("↟", EditorStyles.label))
                    {
                        GameObject ob = bookmarkedObjects[i];
                        bookmarkedObjects.RemoveAt(i);
                        bookmarkedObjects.Insert(i - 1, ob);
                        sorted = false;
                    }
                if (i < bookmarkedObjects.Count - 1)
                {
                    if (GUILayout.Button("↡", EditorStyles.label))
                    {
                        GameObject ob = bookmarkedObjects[i];
                        bookmarkedObjects.RemoveAt(i);
                        bookmarkedObjects.Insert(i + 1, ob);
                        sorted = false;
                    }
                }
                else GUILayout.Label(" ");
                if (GUILayout.Button("X", EditorStyles.miniButton))
                    bookmarkedObjects.RemoveAt(i);
                EditorGUILayout.EndHorizontal();
            }
        GUILayout.FlexibleSpace();
        if (Selection.activeGameObject != null)
        {
            if (Selection.objects != lastSelectionState)
            {
                lastSelectionState = Selection.objects;
                namedTheSameLOnLevel0 = getObjectsNamedTheSameAsCurernt(0); // this is a relatively expensive traversal
                namedTheSameLOnLevel1 = getObjectsNamedTheSameAsCurernt(1); // so we cache the results
                namedTheSameLOnLevel2 = getObjectsNamedTheSameAsCurernt(2); // and only do the search if the selection state
                namedTheSameLOnLevel3 = getObjectsNamedTheSameAsCurernt(3); // is different than on last redraw
            }
            string currentName = Selection.activeGameObject.name;
            if (namedTheSameLOnLevel0.Length > 1 || namedTheSameLOnLevel1.Length > 1 || namedTheSameLOnLevel2.Length > 1 || namedTheSameLOnLevel3.Length > 1)
            {
                GUILayout.Label("Found some objects also named \"" + currentName + "\"");
                EditorGUILayout.BeginHorizontal();
                if (namedTheSameLOnLevel0.Length > 1)
                    if (GUILayout.Button(namedTheSameLOnLevel0.Length + " siblings"))
                        Selection.objects = namedTheSameLOnLevel0;
                if (namedTheSameLOnLevel1.Length > 1)
                    if (GUILayout.Button(namedTheSameLOnLevel1.Length + " one level up"))
                        Selection.objects = namedTheSameLOnLevel1;
                if (namedTheSameLOnLevel2.Length > 1)
                    if (GUILayout.Button(namedTheSameLOnLevel2.Length + " two levels up"))
                        Selection.objects = namedTheSameLOnLevel2;
                if (namedTheSameLOnLevel3.Length > 1)
                    if (GUILayout.Button(namedTheSameLOnLevel3.Length + " three levels up"))
                        Selection.objects = namedTheSameLOnLevel3;
                EditorGUILayout.EndHorizontal();
            }
        }
    }

    void sortObjects()
    {
        Debug.Log("soring");
        List<GameObject> sortedList = new List<GameObject>();
        Transform[] allTransforms = GameObject.FindObjectsOfType(typeof(Transform)) as Transform[];

        for (int i = 0; i < allTransforms.Length; i++)
        {
            GameObject thisGameObject = allTransforms[i].gameObject;
            for (int k = 0; k < bookmarkedObjects.Count; k++)
                if (thisGameObject == bookmarkedObjects[k]) sortedList.Add(bookmarkedObjects[k]);
        }
        bookmarkedObjects = sortedList;
        sorted = true;
    }
    public static GameObject[] getObjectsNamedTheSameAsCurernt(int namedTheSameLOnLevel)
    {
        namedTheSameLOnLevel++;
        string selName = Selection.activeGameObject.name;
        Transform thisTransform = Selection.activeGameObject.transform;
        for (int i = 0; i < namedTheSameLOnLevel; i++)
        {
            if (thisTransform.parent == null) return new GameObject[0];
            thisTransform = thisTransform.parent;
        }
        Transform[] all = thisTransform.gameObject.GetComponentsInChildren<Transform>(true);
        List<GameObject> gameObjects = new List<GameObject>();
        int activeObjects = 0;
        int inactiveObjects = 0;
        for (int i = 0; i < all.Length; i++)
            if (all[i].name.Equals(selName))
            {
                gameObjects.Add(all[i].gameObject);
                if (all[i].gameObject.activeInHierarchy) activeObjects++; else inactiveObjects++;
            }
        return gameObjects.ToArray();
    }

    void OnSelectionChange()
    {
        Repaint();
    }
    void OnHierarchyChange()
    {
        Repaint();
    }
}
