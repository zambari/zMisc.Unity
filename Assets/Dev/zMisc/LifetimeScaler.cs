using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifetimeScaler : MonoBehaviour
{
    [Range(1, 40)]
    public float life = 5;
    [Range(0, 5)]
    public float shrinkTime = 1;
    [Range(0, 5)]
    public float growTime = 1;

    void Start()
    {
        StartCoroutine(LifeTime());
    }
    IEnumerator LifeTime()
    {
        float startTime = Time.time;
        Vector3 startScale = transform.localScale;
        transform.localScale = Vector3.zero;
        yield return null;
        float normalisedTime = 0;
    if (growTime>0)
        while (normalisedTime < 1)
        {
            normalisedTime = (Time.time - startTime) / growTime;
            transform.localScale = startScale * normalisedTime;
            yield return null;
        } else   transform.localScale = startScale;
        yield return new WaitForSeconds(life);
        normalisedTime = 0;
        float shrinkStartTime = Time.time;
        if (shrinkTime>0)
        while (normalisedTime < 1)
        {
            normalisedTime = (Time.time - shrinkStartTime) / shrinkTime;
            transform.localScale = startScale * (1 - normalisedTime);
            yield return null;
        }
        Destroy(gameObject);
    }



}
