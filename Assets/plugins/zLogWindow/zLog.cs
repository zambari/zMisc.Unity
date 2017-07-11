using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics;
using System;
using System.Text;
using Z;

public class zLog : zNodeController
{
    public bool autoSize;
    public static zLog instance;
    public bool isMasterInstance = true;
    protected List<zLogRow> rowList;
    public Vector2 minimizedSize = new Vector2(280, 25);
    Vector2 savedSize;
    public int maxRows = 5;
    Coroutine fade;
    public float fadeTime = 1;
    public Color bgColor = new Color(0, 0, 0, 0.3f);
    public Color bgColorFaded = new Color(0, 0, 0, 0.1f);
    public bool logWhenLog;
    public static void Log(string text)
    {
        if (instance == null)
        {
            UnityEngine.Debug.Log("log: " + text);
            return;
        }
        instance.AddLine(text);

    }

    public void AddLine(string text)
    {
        zLogRow row;
        if (rowList.Count < instance.maxRows)
        {
            row = AddNodeFromTemplate() as zLogRow;
            rowList.Add(row);
            nodes.Add(row);
        }
        else
            row = setup.content.GetChild(0).GetComponent<zLogRow>();

        row.printMessage(new LogMessage(text));
        row.setAlternativeMessage(instance.getStackTrace());

    }

    protected override void Awake()
    {
        base.Awake();
        if (isMasterInstance)
            instance = this;

        if (rowList == null)
            rowList = new List<zLogRow>();
        //        scrollReversed=false;

    }

    protected override void OnValidate()
    {
        base.OnValidate();
        if (setup.image != null)
        {
            setup.image = GetComponent<Image>();
            setup.image.color = bgColor;
        }
        //      ContentSizeFitter sizeFitter=GetComponentInChildren<ContentSizeFitter>();

    }

    public string getStackTrace()
    {
        StringBuilder sb = new StringBuilder();


        StackTrace stackTrace = new StackTrace(true);
        StackFrame[] stackFrames = stackTrace.GetFrames();
        int logStackDepth = stackFrames.Length;
        if (logStackDepth > 5)
            logStackDepth = 5;
        for (int i = stackFrames.Length - 1; i > 2; i--)
        {
            try
            {
                if (i == stackFrames.Length - 1) sb.Append("     >");
                else sb.Append("   ->");
                sb.Append("<i>" + stackFrames[i].GetMethod().Name + "</i>");
                string stackfname = stackFrames[i].GetFileName();
                string[] fnameSplit = stackfname.Split('/');
                stackfname = fnameSplit[fnameSplit.Length - 1];
                if (stackfname.Length < 3)
                    continue;
                string[] tmp = stackfname.Split('\\');
                string fname = tmp[tmp.Length - 1];
                sb.Append("   (" + fname + ")");
                sb.Append("  (line  : " + stackFrames[i].GetFileLineNumber().ToString() + ") ");
                if (i != 2) sb.Append("\n");
            }
            catch (Exception e)
            {
                sb.Append(" eception when parsing stack " + e.Message);
            }


        }

        return sb.ToString();

    }

    public static void Log(string text, GameObject g)
    {
        Log(text);
    }
    public static void log(string text)
    {
        Log(text);
    }


    public static void show()
    {
        if (instance != null) instance.canvas.enabled = true;
    }
    public static void hide()
    {
        if (instance != null) instance.canvas.enabled = false;
    }
    /* 
     public void OnBeginDrag(BaseEventData eventData)
     {
         startPosition = transform.position;
         startCursorPosition = Input.mousePosition;
     }
     public void OnDrag(BaseEventData eventData)
     {
         Vector3 drag = Input.mousePosition - startCursorPosition;
         transform.position = startPosition + drag;
     }
  */
    IEnumerator fadeOutWindow()
    {
        float fadeStart;
        canvas.enabled = true;
        setup.image.color = bgColor;

        bool someActive = true;

        while (someActive)
        {
            someActive = false;
            yield return new WaitForSeconds(0.2f);

            for (int i = 0; i < rowList.Count; i++)
            {
                if (!rowList[i].isDisabled)
                {
                    someActive = true;

                }
            }
        }
        fadeStart = Time.time;

        while (Time.time < fadeStart + fadeTime)
        {
            float r = (Time.time - fadeStart) / fadeTime;
            setup.image.color = Color.Lerp(bgColor, bgColorFaded, r);
            yield return null;
        }
    }
    public void minimize()
    {
        savedSize = rect.sizeDelta;
        rect.sizeDelta = minimizedSize;
        rect.localScale = new Vector3(0.75f, 0.75f, 0.75f);
    }
    public void restore()
    {
        rect.localScale = new Vector3(1, 1, 1);
        rect.sizeDelta = savedSize;

    }
}
