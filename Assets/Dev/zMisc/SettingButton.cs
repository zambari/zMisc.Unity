using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z{

public class SettingButton : MonoBehaviour {

public string settingName="default";
public string value;

public void setSettingName(string v)
{
	settingName=v;
}
public void setStringValue(string v)
{
	value=v.ToString();
}
public void setIntValue(int v)
{
	value=v.ToString();
}
public void setFloatValue(float v)
{
	value=v.ToString();
}
void OnEnable()
{
	if (settingName!="default")
		Settings.SetValue(settingName,value);
}
void OnValidate()
{
	if (settingName.Length==0) settingName="def";
}
void Start()
{
	Button button=GetComponent<Button>();
	if (button!=null)
		button.onClick.AddListener(SendChange);
}

public void SendChange()
{
	Settings.SetValue(settingName,value);
}
	
	
}
}