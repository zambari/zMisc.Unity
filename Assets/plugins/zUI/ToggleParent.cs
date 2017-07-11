//z2k17

using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
//using System.Collections;
//using System.Collections.Generic;

public class ToggleParent : MonoBehaviour
{

   public RectTransform baseParent;
    public RectTransform alternativeParent;
    public bool useAlternative;
    int baseIndex=-1;
    void Reset()
    {
      if (!useAlternative)  baseParent=transform.parent.GetComponent<RectTransform>();;

    }

    public bool switchParent
    {
        get
        {
            return useAlternative;
        }
        set
        {
            useAlternative = value;
            if (useAlternative) baseIndex=transform.GetSiblingIndex();
            if (baseParent != null && alternativeParent != null)
                transform.SetParent(value ? alternativeParent : baseParent);
                if (!useAlternative) 
                {
                    if (baseIndex!=-1) transform.SetSiblingIndex(baseIndex);
                }
            //    Canvas.ForceUpdateCanvases();
        }
    }
    void OnValidate()
    {    
        switchParent=useAlternative;
    }
}
