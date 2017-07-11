using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileSaveHelper : MonoRect {

public Button saveButton;
//public Button CancelButton;
public zFileBrowser fileBrowser;
//public Toggle savePanelToggle;
public InputField inputField;


void Start()
{
		saveButton.onClick.AddListener(onSaveButton);
			if (inputField.text.Length==0)
			 saveButton.interactable=false;
			inputField.onEndEdit.AddListener(onEndEdit);
			inputField.onValueChanged.AddListener(onTextChange);
			
}
	public void onTextChange(string c)
	{
		if (c.Length>0)
	
			saveButton.interactable=true; else saveButton.interactable=false;
	

	}

	public void increment()
	{
inputField.text=inputField.text+"+";

	}
	public void onEndEdit(string s)
	{
		onSaveButton();
	}
	public void onSaveButton()
	{
	if (inputField.text.Length>0)
		fileBrowser.saveClicked(inputField.text);
	}
	
	
}
