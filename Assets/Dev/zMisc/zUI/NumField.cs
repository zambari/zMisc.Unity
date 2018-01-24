using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumField : MonoBehaviour {
	public InputField inputField;
	public int value;
	public Button increaseButton;
	public Button decreaseButton;
	public int minValue=0;
	public int maxValue=10;
	public IntEvent onValueChanged;

	void Reset()
	{
		Button[] btns=GetComponentsInChildren<Button>();
		if (btns.Length==2) 
		 { decreaseButton=btns[0];
		    increaseButton=btns[1];

		 }
		 inputField=GetComponentInChildren<InputField>();
	}
	void Start()
     {
		if (inputField==null) inputField=GetComponentInChildren<InputField>();
		inputField.onEndEdit.AddListener(checkRangeFromString);
		if (increaseButton!=null) increaseButton.onClick.AddListener(IncreaseValue);
		if (decreaseButton!=null) decreaseButton.onClick.AddListener(DecreaseValue);	

	}


	public void IncreaseValue()
	{
		checkValueRange(value+1);
	}

	public void DecreaseValue()
	{

		checkValueRange(value-1);
	}

	void checkValueRange(int k)
	{
	    if (k<minValue) k=minValue;
		if (k>maxValue) k=maxValue;
		value=k;
		if  (decreaseButton!=null) decreaseButton.interactable=(value!=minValue);
		if  (increaseButton!=null) increaseButton.interactable=(value!=maxValue);
		inputField.text=value.ToString();
		onValueChanged.Invoke(value);
	}


	void checkRangeFromString(string s)
	{
		int k=0;
		if (Int32.TryParse(s,out k))
	
		 value=k;		
	}
	
}
