using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(ToggleGroupHelper))]
public class TogglePickActive : MonoBehaviour {
public GameObject[] objects;
	void Start()
	{
		ToggleGroupHelper tg=GetComponent<ToggleGroupHelper>();
		tg.onActiveToggleChange.AddListener(onChange);
	}
	void onChange(int v)
	{
		if (v<0 || v>=objects.Length) { Debug.Log("invalid selection "+v,gameObject); return;}
		for (int i=0;i<objects.Length;i++)
				objects[i].SetActive(i==v);
	}
}
