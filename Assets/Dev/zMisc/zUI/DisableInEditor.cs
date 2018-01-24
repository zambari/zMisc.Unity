using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class DisableInEditor : MonoBehaviour
{

    public enum Target { none, image, rawimage, canvasGroup }

    public Target target;
    void Reset()
    {
        if (GetComponent<Image>() != null) target = Target.image;
        else
        if (GetComponent<RawImage>() != null) target = Target.image;
        else
        if (GetComponent<CanvasGroup>() != null) target = Target.image;

    }

    MonoBehaviour getTarget()
    {
        switch (target)
        {
            case Target.image:
                return GetComponent<Image>();
            case Target.rawimage:
                return GetComponent<RawImage>();
          //  case Target.canvasGroup:
         //       return GetComponent<CanvasGroup>();

        }
        return null;
    }

    void Start()
    {
	 MonoBehaviour c=getTarget();
	 if (target==Target.canvasGroup)  GetComponent<CanvasGroup>().alpha=(Application.isPlaying?1:0); else
	 if (c!=null) c.enabled=Application.isPlaying;
    }


}
