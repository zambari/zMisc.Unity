using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace zUI
{
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class PanelSeparatorBeta : MonoBehaviour {
public Image showWhenOn;
GameObject myControlledObject;
Transform myControlledTransform;
RectTransform myRect;
RectTransform controlledRect;


	public enum DeactivationMethods { setActive, canvas, layoutelement };
	public DeactivationMethods deactivationMethod;
    [Header("Bind to state changes")]

    public UnityEvent onToggleOn;
    public UnityEvent onToggleOff;
    public BoolEvent onStateChange;
    [SerializeField]
    [HideInInspector]
    Text text;
    [SerializeField]
    [HideInInspector]
    Button b;



   // [SerializeField]

    public string labelPrimary = "button off";
    public string labelSecondary = "button on";
    private bool _state;
	private int _myCachedIndex;
	// Use this for initialization
void Reset()
{
	name="---PanelSeparatorBeta";
	
	text=GetComponentInChildren<Text>();
	if (text!=null) labelPrimary=text.text;
}
void afix()
{
	_myCachedIndex=transform.GetSiblingIndex();
	if (_myCachedIndex>=transform.parent.childCount-1)
	{
		Debug.Log("abotring afix");
		return;
	}
	myControlledTransform= transform.parent.GetChild(_myCachedIndex+1);
	myControlledObject=myControlledTransform.gameObject;
	if (myControlledObject!=null){
	myRect=GetComponent<RectTransform>();
 	controlledRect=myControlledObject.GetComponent<RectTransform>();
	 name="------"+myControlledObject.name;
	}
}

    public bool state
    {
        get
        {
            return _state;
        }
        set
        {
			
			if (_state == value) return;
            _state = value;
            if (text != null)
                text.text = (_state ? labelSecondary : labelPrimary);
            if (_state) onToggleOn.Invoke(); else onToggleOff.Invoke();
            onStateChange.Invoke(_state);
            if (showWhenOn != null) showWhenOn.enabled = _state;
        }
    }

    protected virtual void OnValidate()
    {
        if (text == null) text = GetComponentInChildren<Text>();
        if (b == null) b = GetComponent<Button>();
		
        state = _state;

    }
    public void ToggleState()
    {
        state = !state;
    }
	public void ShowControlledObject()
	{if (myControlledObject==null) return;
		if (deactivationMethod==DeactivationMethods.setActive)
		myControlledObject.SetActive(true);
	}

	public void HideControlledObject()
	{
if (myControlledObject==null) return;
if (deactivationMethod==DeactivationMethods.setActive)
		myControlledObject.SetActive(false);

	}
    void Start()
    {	afix();
        if (b != null)
        {
            b.onClick.AddListener(ToggleState);
        }
		onToggleOn.AddListener(ShowControlledObject);
		onToggleOff.AddListener(HideControlledObject);
    }



}
}