using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SimpleConsole : MonoRect
{
    public static SimpleConsole instance;
    List<string> logList;
    public bool alsoLogToConsole;

    public int maxLines = 3;
    public bool autoHideLines;
    public float autoHideTime = 5;
    public bool test;
    public bool clearNow;
    void Clear()
    {
        logList = new List<string>();
        buildLog();
    }
    void OnValidate()
    {
        if (test)
        {
            SimpleConsole.Log("rra" + Random.value);
        }
    }
    public static void Log(string logentry)
    {

        if (instance == null)
        {
            Debug.LogWarning(" simple log not present !");
        }
        else
        {
            if (instance.logList.Count >= instance.maxLines) instance.logList.RemoveAt(0);
            instance.logList.Add(logentry);
            instance.buildLog();
            if (instance.alsoLogToConsole) Debug.Log(logentry);
            if (instance.autoHideLines)
                instance.StartCoroutine(instance.Remover());

        }
    }


    IEnumerator Remover()
    {
        yield return new WaitForSeconds(autoHideTime);
        {
            if (logList.Count > 1) logList.RemoveAt(0);
            buildLog();
        }
        // code here
    }

    void buildLog()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < logList.Count; i++)
        {
            sb.Append(logList[i]); sb.Append("\n");
        }
        text.text = sb.ToString();

    }
    void Awake()
    {
        if (instance != null) { Debug.LogWarning("another SimplEonsole :" + instance.name + " we are " + name, gameObject); }
        instance = this;
        logList = new List<string>();
    }


    void Reset()
    {
        if (text == null)
        {
            GameObject g = new GameObject("ConsoleText");
            RectTransform t = g.AddComponent<RectTransform>();
            text = g.AddComponent<Text>();
            t.SetParent(transform);
            t.anchorMin = new Vector2(0, 0);
            t.anchorMax = new Vector2(1, 1);
            t.offsetMin = new Vector2(5, 5);
            t.offsetMax = new Vector2(-5, -5);
            image.raycastTarget = false;
            text.raycastTarget = false;
            name = "SimpleConsole";
        }
    }

}
