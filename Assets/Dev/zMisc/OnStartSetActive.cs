using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z {
public class OnStartSetActive : MonoBehaviour
{
    public GameObject target;
    public bool activeState = true;
    public bool executeOnStart = true;
    public bool executeOnEnable = false;
    void OnEnable()
    {
        if (executeOnEnable)
            if (target != null)
                target.SetActive(activeState);
    }
    void Reset()
    {
        target = gameObject;
    }
    void Start()
    {
        if (executeOnStart)
            if (target != null)
                target.SetActive(activeState);
    }
}
}