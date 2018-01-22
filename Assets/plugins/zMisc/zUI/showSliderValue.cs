using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class showSliderValue : MonoBehaviour
{
    Text text;
    void Start()
    {
        Slider slider = GetComponentInParent<Slider>();
        text = GetComponent<Text>();

        text.raycastTarget=false;
        if (slider != null)
		{
            slider.onValueChanged.AddListener(showAsText);
			showAsText(slider.value);
		}
    }
    void showAsText(float f)
    {
        text.text = f.ToShortString();
    }
}
