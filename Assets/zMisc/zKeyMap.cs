//zambari codes unity

using UnityEngine;
using System.Collections.Generic;
using System;

public class zKeyMap : MonoBehaviour
{
    public bool allowMappingMultiple=true;
    [Header("Preview")]
    public List<string> mappedKeysPreview;
    public static List<Mapping> mappings;
    static bool _allowMappingMultiple;
    static int lastMapping;
    static zKeyMap instance;
    public static bool map(MonoBehaviour mono, Action action, KeyCode key, bool checkIfActive = true)
    {
        if (instance == null)
        {
            GameObject keyMap = new GameObject("zKeyMap");
            keyMap.gameObject.AddComponent<zKeyMap>();

        }
        if (mappings == null)
        {
            mappings = new List<Mapping>();
            instance.mappedKeysPreview = new List<string>();
        }
        if (!_allowMappingMultiple)
            for (int i = 0; i < mappings.Count; i++)
            {
                if (mappings[i].k == key)
                {
                    Debug.Log("key " + key.ToString() + "is  mapped already, sorry");
                    return false;
                }
            }
        Mapping thisMapping = new Mapping();
        thisMapping.k = key;
        thisMapping.m = mono;
        thisMapping.a = action;
        lastMapping++;
        thisMapping.id = lastMapping;
        thisMapping.checkIfActive = checkIfActive;
        mappings.Add(thisMapping);
        if (instance != null) instance.mappedKeysPreview.Add(key.ToString());
        return true;
    }

    void OnDisable()

    {
//        Debug.LogWarning("zKeyMap has been disasbled");
    }
    void OnValidate()
    {
        _allowMappingMultiple = allowMappingMultiple;
    }
    public class Mapping
    {
        public KeyCode k;
        public MonoBehaviour m;
        public Action a;
        public int id;
        public bool checkIfActive;
    }
    public static bool unmap(MonoBehaviour mono, Action action, KeyCode key)
    {
        if (mappings == null) return false;
        for (int i = 0; i < mappings.Count; i++)
        {
            if (mappings[i].k == key && (mappings[i].m == mono))
            {
                mappings.RemoveAt(i);
                if (instance != null) instance.mappedKeysPreview.Remove(mappings[i].k.ToString());
                return true;

            }
        }
        return false;
    }

    public static void unmap(int id)
    {
        if (mappings == null) return;
        for (int i = 0; i < mappings.Count; i++)

            if (mappings[i].id == id)
            {
                mappings.RemoveAt(i);
                if (instance != null) instance.mappedKeysPreview.Remove(mappings[id].k.ToString());
                return;
            }
    }


    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else instance = this;
         _allowMappingMultiple= allowMappingMultiple;
        if (mappings == null)
        {
            mappings = new List<Mapping>();
            mappedKeysPreview = new List<string>();
        }
    }

    void Update()
    {
        for (int i = 0; i < mappings.Count; i++)
            if (Input.GetKeyDown(mappings[i].k))
            {
                Mapping m = mappings[i];
                if (m.m == null) { unmap(m.id); return; }
                if (m.checkIfActive)
                {
                    if (m.m.enabled == false || !m.m.gameObject.activeInHierarchy) return;
                }
                if (m.a != null) m.a();
            }
    }

}
