using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace zUI
{
public class SeparatorAdder : MonoBehaviour {
	public GameObject separatorPrefab;
		GameObject[] controlledObjects ;
	// Use this for initialization
	void Start () {
		checkIfClean();
		AddSeparators();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


void checkIfClean()
{
	var seps=GetComponentsInChildren<PanelSeparatorBeta>();
	if (seps.Length>0) Debug.LogWarning("existing separator");
}
[ExposeMethodInEditor]
	void AddSeparators()
	{checkIfClean();
		controlledObjects = new GameObject[transform.childCount];
		for (int i=0;i<controlledObjects.Length-1;i++)
		{
			if (controlledObjects[i]!=null)
		{
			GameObject thisSeparator=Instantiate(separatorPrefab,transform);
			thisSeparator.transform.SetSiblingIndex(controlledObjects[i].transform.GetSiblingIndex());
		}

		}

	}
}
}