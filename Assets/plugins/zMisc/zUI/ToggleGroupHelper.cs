using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 414
[RequireComponent(typeof(ToggleGroup))]
public class ToggleGroupHelper : MonoRect
{
    
    [ReadOnly]
    [SerializeField]
    int activeToggle = -1;
    
	/// <summary>
	/// Called when an active toggle is changed
	/// </summary>
	
	public IntEvent onActiveToggleChange;

	/// <summary>
	/// To be called from ToggleHelper, gets forwarded to onActiveToggleChange event
	/// </summary>
	/// <param name="id"></param>
    public void ToggleSelect(int id)
    {
        activeToggle = id;
        onActiveToggleChange.Invoke(id);
    }

    void Reset()
    {
        AddHelpers();
    }
    void OnDisable()
    {
        activeToggle = -1;
    }

    [ExposeMethodInEditor]
    void AddHelpers()
    {
        if (Application.isPlaying)
        {
            Debug.Log("editor only please", gameObject);
            return;
        }
		ToggleGroup tg=GetComponent<ToggleGroup>();
        Toggle[] t = GetComponentsInChildren<Toggle>();
        for (int i = 0; i < t.Length; i++)
        {
            ToggleHelper th = t[i].GetComponent<ToggleHelper>();
            if (th == null) th = t[i].gameObject.AddComponent<ToggleHelper>();
            th.id = i;
			t[i].group=tg;
			t[i].isOn=(i==0);
			t[i].enabled=false;
			t[i].enabled=true;
            th.OnValidate();
        }
    }

}
