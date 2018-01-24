using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 414
[RequireComponent(typeof(Toggle))]
[DisallowMultipleComponent]
public class ToggleHelper : MonoBehaviour {
 
 public int id=-1;
public string label;
[HideInInspector]
[SerializeField]
Toggle toggle;
[HideInInspector]

[SerializeField]
ToggleGroupHelper helper;

[HideInInspector]
[SerializeField]
Text text;
 bool state;
void Reset()
{
	toggle=GetComponent<Toggle>();
	helper=GetComponentInParent<ToggleGroupHelper>();
	text=GetComponentInChildren<Text>();
	if (string.IsNullOrEmpty(label)) 
	if (text!=null) text.text=name;
}
//void OnValu
 void Start()
 {	
	 if (helper==null)
	 helper=GetComponentInParent<ToggleGroupHelper>();
 	 toggle.onValueChanged.AddListener(onValueChanged);

 }

 public void OnValidate()
 {
	if (!string.IsNullOrEmpty(label))
	 {
		if (text==null) 	text=GetComponentInChildren<Text>();
	 
	 } else 
	 {
		 label=name;
	 }
	if (text!=null)		text.text=label;
		name="TGL ["+id+"] "+label; 
 }

 void onValueChanged(bool value)
 {	state=value;
 	if (value)
	if (helper!=null) helper.ToggleSelect(id);
 }

	
	
	
	
}
