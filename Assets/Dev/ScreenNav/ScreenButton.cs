using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization; //remove
using UnityEngine.UI;



/*

 Title : ScreenNavigation
 Part of  Toucan Universal Libraries
 Full description of the module in ScreenHub.cs file

 This Class overview: 
 Added to a button it binds to its OnClick event and requests the hub to switch to a desired screen
 
*/


[RequireComponent(typeof(Button))]
public class ScreenButton : MonoBehaviour
{

[FormerlySerializedAs("newTargetScreen")]
[SerializeField]
	[Header("Drag screen object")]
    public SubScreen targetScreen;

	[Header("Press to refresh screen name")]
	public bool backButton;
	private  ScreenHub hub;
	private Button _b;
	private Button b { get { if (_b==null) _b=GetComponent<Button>(); return _b; }}


	void OnValidate()
	{
 		if (hub==null) hub=GetComponentInParent<ScreenHub>();
		ScreenHub.CollapseComponent(GetComponent<RectTransform>());
		ScreenHub.CollapseComponent(GetComponent<Image>());
		ScreenHub.CollapseComponent(GetComponent<Button>());
		ScreenHub.CollapseComponent(GetComponent<LayoutElement>());
		ScreenHub.RefreshSelection(this);
		if (targetScreen!=null && backButton) Debug.Log("Press delete on the target screen to enable baac");
		if (targetScreen==null)
		{
			backButton=true;
		} else backButton=false;
		
		if (targetScreen!=null)
		b.name="BTN->"+ScreenHub.RemoveMarker(targetScreen.screenName);
		else
		b.name="BTN->back";
	}

    void Start()
    {
        b.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
      //  if (targetScreen == null) 
		//hub.Back();
	//	else
        hub.ChangeToScreen(targetScreen);
    }


}
