using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsIntStore : MonoBehaviour
{
    public string settingName;
    public void TakeSetting(int param)
    {
		Settings.SetValue(settingName,param);
   }

}
