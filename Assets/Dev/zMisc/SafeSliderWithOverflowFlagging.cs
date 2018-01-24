
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
[ExecuteInEditMode]
public class SafeSliderWithOverflowFlagging:  SafeSlider 
{
	public FloatEvent OnValueOverflow;
	
}
