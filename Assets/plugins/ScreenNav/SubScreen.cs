using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*

 Title : ScreenNavigation
 Part of  Toucan Universal Libraries
 Full description of the module in ScreenHub.cs file

 This Class overview: 
 Added to a game object, it gets signed onto a list by screenhub, when activated, it requests deactivation of other screens in the same hub, 
 changes the name of the gameobject to [X]ScreenName or [ ]ScreenName, depending on its active state, which makes the state obvious to see in the hierarchy

 This class has no public method and one public field used for Editor labels, it functions as a pointer to gameobejct to be enabled/disabled
 
*/

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasGroup))]
public class SubScreen : MonoBehaviour
{
    public string screenName="screen";
    public bool isDefaultScreen;
	ScreenHub hub;

    void Reset()
    {
        screenName=name;
    }
    void OnValidate()
    {
CanvasGroup canvasGroup=GetComponent<CanvasGroup>();
if (canvasGroup==null) canvasGroup=gameObject.AddComponent<CanvasGroup>();
        screenName = ScreenHub.ActiveName(screenName, gameObject.activeInHierarchy);
        name = screenName;
    }

    void OnEnable()
    {  
        hub=GetComponentInParent<ScreenHub>();
        if (hub==null) return;
        OnValidate();
	    hub.RegisterScreen(this); // done in case screen is loaded dynamically
		hub.ChangeToScreen(this);
//      Debug.Log(name+" called "+hub.name,gameObject);
    }

    void OnDisable()
    {
        screenName = ScreenHub.ActiveName(screenName, false);
        name = screenName;
    }

}
