using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Z{
public class SettingConditionalTrigger : MonoBehaviour
{
    public bool triggerOnEnable = true;
    public string settingName;
    public string trueValue;
    public VoidEvent ifTrueOnEnable;
    public VoidEvent ifFalseOnEnable;
    public void OnEnable()
    {
        if (triggerOnEnable)
        {
            Trigger();
        }

    }

    void Trigger()
    {

        string currentValue = Settings.GetValue(settingName);
        if (currentValue == trueValue) ifTrueOnEnable.Invoke(); else ifFalseOnEnable.Invoke();
    }



}
}