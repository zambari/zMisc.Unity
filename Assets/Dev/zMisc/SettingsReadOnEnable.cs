using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsReadOnEnable : MonoBehaviour
{

    public string settingName;
    int val;
    public IntEvent executeSetting;
    void OnEnable()
    {
        val = Settings.GetIntValue(settingName);
        executeSetting.Invoke(val);
    }




}
