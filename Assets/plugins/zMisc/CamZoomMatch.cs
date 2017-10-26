using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class CamZoomMatch : MonoBehaviour
{
    public float frustumHeight = 2;
    [Range(2, 20)]
    public float distance;
    public float angleY;
    new Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        camera.fieldOfView = 2.0f * Mathf.Atan(frustumHeight * 0.5f / distance) * Mathf.Rad2Deg;
        //frustumHeight = 2.0f * distance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
    }


}
