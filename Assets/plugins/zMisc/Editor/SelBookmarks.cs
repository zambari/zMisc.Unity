using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


/*
 Title : SelBookmarks ™ 
 Part of  Toucan Universal Libraries
 Autor by : Zambari at http://toucan-systems.pl
 
 Editor window enabling you to remember and restore Hierarchy selections.
 Aslo searches for simiarily named objects within hierarchy (for example objects called Text).
 Theres also a shortcut for toggling the active state of bookmarked objects

 version 1.01    (2017.09.04)

*/

public class SelBookmarks : EditorWindow
{
    [MenuItem("Tools/Open Selection Bookmarks")]
    static void Init()
    {
#pragma warning disable 219
        SelBookmarks window =
            (SelBookmarks)EditorWindow.GetWindow(typeof(SelBookmarks));
#pragma warning restore 219
    }
    [SerializeField]
    List<GameObject> objects;
    GameObject[] namedTheSameLOnLevel0;
    GameObject[] namedTheSameLOnLevel1;
    GameObject[] namedTheSameLOnLevel2;
    GameObject[] namedTheSameLOnLevel3;
    object[] lastSelectionState;
    void OnGUI()
    {
        if (objects == null) objects = new List<GameObject>();
        else
        {
            int i = 0;
            while (i < objects.Count)
            {
                if (objects[i] == null) objects.RemoveAt(i); else i++;
            }
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
                if (!objects.Contains(Selection.activeGameObject))
                    objects.Add(Selection.activeGameObject);
                else
                {
                    objects.Remove(Selection.activeGameObject);
                    objects.Insert(0, Selection.activeGameObject);
                }
            }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        if (objects.Count > 0)
            for (int i = 0; i < objects.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Toggle(objects[i].activeInHierarchy, "")) 
                    objects[i].SetActive(true);
                else
                    objects[i].SetActive(false);


                if (GUILayout.Button("*", EditorStyles.label))
                    EditorGUIUtility.PingObject(objects[i]);
                if (GUILayout.Button(" Select: " + objects[i].name + " ")) 
                    Selection.activeGameObject = objects[i];

                GUILayout.FlexibleSpace();
                if (i > 0)
                    if (GUILayout.Button("↟", EditorStyles.label)) 
                    {
                        GameObject ob = objects[i];

                        objects.RemoveAt(i);
                        objects.Insert(i - 1, ob);
                    }
                if (i < objects.Count - 1)
                    if (GUILayout.Button("↡", EditorStyles.label)) 
                    {
                        GameObject ob = objects[i];
                        objects.RemoveAt(i);
                        objects.Insert(i + 1, ob);
                    }
                if (GUILayout.Button("X"))
                    objects.RemoveAt(i);
                EditorGUILayout.EndHorizontal();
            }
        GUILayout.FlexibleSpace();
        if (Selection.activeGameObject != null)
        {
            if (Selection.objects!=lastSelectionState)
            {
            lastSelectionState=Selection.objects; 
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
