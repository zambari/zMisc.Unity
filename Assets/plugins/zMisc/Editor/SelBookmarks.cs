using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class SelBookmarks : EditorWindow
{

    [MenuItem("Tools/Open Selection Bookmarks")]
    static void Init()
    {
        SelBookmarks window =
            (SelBookmarks)EditorWindow.GetWindow(typeof(SelBookmarks));
    }
    [SerializeField]
    List<GameObject> objects;

    void OnHierarchyChange()
    {
        Repaint();
    }
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
        if (GUILayout.Button("Mark Selected")) //, GUIStyle.none
        {
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
            return;
        }
        GUILayout.FlexibleSpace();


        EditorGUILayout.EndHorizontal();
        GUILayout.Space(10);
        if (objects.Count > 0)
            for (int i = 0; i < objects.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Toggle(objects[i].activeInHierarchy, "")) //, GUIStyle.none
                     //if (objects[i].activeInHierarchy)
                      objects[i].SetActive(true);
                    else
                    //if (!objects[i].activeInHierarchy) 
                    objects[i].SetActive(false);


                if (GUILayout.Button("*", EditorStyles.label)) //, GUIStyle.none
                    EditorGUIUtility.PingObject(objects[i]);
                //objects[i].SetActive(objects[i].activeInHierarchy);
                //    if (!Selection.activeGameObject == objects[i])
                //   {
                if (GUILayout.Button(" Select: " + objects[i].name + " ")) //, GUIStyle.none
                    Selection.activeGameObject = objects[i];
                //  }
                // else
                //    GUILayout.Label(" Selected: " + objects[i].name, EditorStyles.label);

                GUILayout.FlexibleSpace();
                if (i > 0)
                    if (GUILayout.Button("↟", EditorStyles.label)) //, GUIStyle.none
                    {
                        GameObject ob = objects[i];

                        objects.RemoveAt(i);
                        objects.Insert(i - 1, ob);
                    }
                if (i < objects.Count - 1)
                    if (GUILayout.Button("↡", EditorStyles.label)) //, GUIStyle.none
                    {
                        GameObject ob = objects[i];
                        objects.RemoveAt(i);
                        objects.Insert(i + 1, ob);
                    }
                if (GUILayout.Button("X")) //, GUIStyle.none
                    objects.RemoveAt(i);
                EditorGUILayout.EndHorizontal();
            }

    }

}
