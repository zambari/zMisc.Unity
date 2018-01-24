using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
// this class simplifies using sliders from code

// remember to call its onvalidate !
namespace 
Z{
[System.Serializable]
public class FloatEventDrawer
{
  public FloatEvent OnValueChange;
}

}
[System.Serializable]
public class SliderLink
{

    [Range(0, 1)]
    public float valueSet;
    public Slider sliderReference;
	
	public FloatEventDrawer events;
    public FloatEvent OnValueChange{
		get {return events.OnValueChange; }
	}
    public void OnValidate()
    {
        value = valueSet;
        if (!prepared)
   			if (sliderReference!=null)
            { 
	  		  //  sliderReference.onValueChanged.RemoveAllListeners();
			 	 sliderReference.onValueChanged.AddListener(sliderValueChange);
                 prepared = true;
			}
       }

    public void sliderValueChange(float f)
    {
        {
            events.OnValueChange.Invoke(f);
            valueSet = f;
            _value = f;
        }
    }
    float _value;
    bool ignoreEvent;
    bool prepared;
    public float value
    {
        get
        {
            return _value;
        }
        set
        {
            if (_value == value) return;
			if (ignoreEvent) Debug.Log("val changed while ignoring?");
            _value = value;
            ignoreEvent = true;
             sliderReference.value = value;
            ignoreEvent = false;
            valueSet = value;
        }
    }

}