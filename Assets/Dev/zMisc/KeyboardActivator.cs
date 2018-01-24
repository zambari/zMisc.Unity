using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class KeyboardActivationInfo
{
	public KeyCode key=KeyCode.A;
	public GameObject gameObject;
}

public class KeyboardActivator : MonoBehaviour {
public KeyboardActivationInfo[] objectList;

	void Update()
	{
		for (int i=0;i<objectList.Length;i++)
		{
			if (objectList[i].gameObject!=null && Input.GetKeyDown(objectList[i].key))
			{
				objectList[i].gameObject.SetActive( !objectList[i].gameObject.activeSelf );
			}

		}
	}
	
	
	
}
