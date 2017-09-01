using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TagHider : MonoBehaviour
{
    public string[] tags = new string[] { "f1", "f2", "f3", "f4", "f5", "f6" };
    public KeyCode startingKeycode = KeyCode.F1;
    public KeyCode[] keycodes;

    public GameObject[] objects;
    public bool scanObjects;
    public bool clearObjectsWithtags;
    void OnValidate()
    {
        clearObjectsWithtags = clearObjectsWithtags.executeIfTrue(clear);
        keycodes = new KeyCode[tags.Length];
        for (int i = 0; i < tags.Length; i++)
            keycodes[i] = startingKeycode + i;

        if (objects == null || objects.Length < tags.Length) objects = new GameObject[tags.Length];

        for (int i = 0; i < tags.Length; i++)
            if (objects[i] == null)

                objects[i] = GameObject.FindGameObjectWithTag(tags[i]);
        scanObjects = false;
        //   }
    }

    void clear()
    {
        for (int i = 0; i < tags.Length; i++)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag(tags[i]);
            for (int k = 0; k < objects.Length; k++)
                objects[k].tag = "Untagged";

        }

    }

    void Update()
    {
        for (int i = 0; i < keycodes.Length; i++)
            if (Input.GetKeyDown(keycodes[i]))
            {
                if (objects[i] != null)
                {

                    objects[i].SetActive(!objects[i].activeInHierarchy);
                   // zLog.Log("state of " + objects[i] + "toggled");
                }
              //  else zLog.Log("no object wit tag " + tags[i]);

            }
    }




}
