//zambari codes unity

using UnityEngine;


public class BenchmarkClones : MonoBehaviour
{

    public GameObject source;
    public int howMany=1000;
    void Awake()
    {
        System.Diagnostics.Stopwatch stopwach=new  System.Diagnostics.Stopwatch();
        
        if (source != null)
        {
            stopwach.Start();
            Debug.Log("Creating " + howMany + " clones..");
            for (int i = 0; i < howMany; i++)
                Instantiate(source, transform);
            stopwach.Stop();
            Debug.Log("... and we're done, this took "+stopwach.ElapsedMilliseconds+" ms");
        }
        else Debug.Log("pick sourc object");
        minTime = 1 / 55f;
    }
    int frameCounter;
    float startTime;
    public int reportTimAfterFrameNr = 100;
    float minTime;
    void OnValidate()
    {
        if (reportTimAfterFrameNr < 10) reportTimAfterFrameNr = 100;
        if (reportTimAfterFrameNr > 500) reportTimAfterFrameNr = 100;
    }
    void Update()
    {
        if (frameCounter == 0)
        {
            startTime = Time.time;
        }
        frameCounter++;
        if (frameCounter >= reportTimAfterFrameNr)
        {
            frameCounter = 0;
            float dt = Time.time - startTime;
            float averageFrameTime = dt / reportTimAfterFrameNr;
            bool warning = averageFrameTime < minTime;
            float fps = Mathf.Round(1 / averageFrameTime * 100) / 100f;
            dt = Mathf.Round(dt * 100) / 100;
            averageFrameTime = Mathf.Round(averageFrameTime * 1000) / 1000;
            Debug.Log(" Time rendering "+reportTimAfterFrameNr+" frames: " + dt + " (" + fps + " FPS)" + (warning ? " [this is too fast, consider adding more clones]" : ""));
        }

    }
}
