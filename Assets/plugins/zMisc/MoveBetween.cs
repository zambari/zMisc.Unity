using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveBetween : MonoRect
{

    public Vector3 startPos;
    public Vector3 endPos;
	[SerializeField]
    [Range(0, 1)]
    float _pos;

    void OnValidate()
    {
        pos = _pos;
    }

    public float pos
    {
        get { return _pos; }
        set
        {
            _pos = value;
            transform.position = Vector3.Lerp(startPos, endPos, value);
        }
    }

    [ExposeMethodInEditor]
    public void saveAsStart()
    {
        startPos = transform.position;
    }

    [ExposeMethodInEditor]
    public void saveAsEnd()
    {
        endPos = transform.position;

    }



}
