using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class DelayedNumber
{
    [Range(0, 5)]
    public float delay = 0.5f;
    [SerializeField]
    [HideInInspector]
   public MonoBehaviour mono;
  public  List<Vector2> timesAndValues;
    public FloatEvent delayedOutput;
    public System.Action<float> delayedOutputAction;
    public DelayedNumber(MonoBehaviour monoBehaviour)
    {
        mono = monoBehaviour;
        timesAndValues = new List<Vector2>();
    }
    Coroutine replayRoutine;
    [ReadOnly]
    public int queueLen;
    public void QueueValue(float f)
    {
        if (mono == null)
        { Debug.LogError("Please initialise DelayedNumber with a (MonoBehaviour) constructor"); return; }

        if (timesAndValues == null) timesAndValues = new List<Vector2>();
        timesAndValues.Add(new Vector2(Time.time + delay, f));
        queueLen++;
        if (!isRunning) mono.StartCoroutine(delayRoutine());
    }
    public bool isRunning;
    IEnumerator delayRoutine()
    {
        isRunning = true;
        while (timesAndValues.Count > 0)
        {
            float nextTime = timesAndValues[0].x;
            float nextValue = timesAndValues[0].y;

            if (Time.time<nextTime )
            {
                yield return new WaitForSeconds(nextTime-Time.time);
            }
            delayedOutput.Invoke(nextValue);
            if (delayedOutputAction != null) delayedOutputAction.Invoke(nextValue);
            timesAndValues.RemoveAt(0);
            queueLen--;
        }
        isRunning = false;
        yield return null;
    }

}

public class delaytest : MonoBehaviour
{

    // Use this for initialization
    public Slider slider1;
    public Slider slider2;
    public DelayedNumber delayedNumber;
    void Reset()
    {
        delayedNumber = new DelayedNumber(this);
    }
    void Start()
    {
		delayedNumber.mono=this;
        slider1.onValueChanged.AddListener(slidval);
        delayedNumber.delayedOutputAction += (x) => slider2.value = x;
    }
    void slidval(float f)
    {
        delayedNumber.QueueValue(f);
        //slider2.value=f;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
