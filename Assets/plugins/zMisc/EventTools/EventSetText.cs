using System;
using UnityEngine;
using UnityEngine.UI;

public class EventSetText : MonoRect
{
public float mulitiplier=100;
public string unit=" %";

    public void setText(string s)
    {
        text.text = s;
    }
    public void setText(int s)
    {
        text.text = s.ToString(); ;
    }
    public void setFloat(float s)
    {
        text.text = (s*mulitiplier).ToShortString()+unit;
    }
    public void setFloatAsTime(float s)
    {
        TimeSpan t = TimeSpan.FromSeconds(s);
        string answer = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}",
                        t.Hours,
                        t.Minutes,
                        t.Seconds,
                        t.Milliseconds);

        text.text = answer;
    }
}
