using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
[ExecuteInEditMode]
public class zPanelAnchor : MonoBehaviour {
RectTransform canvasRectTransform;
      public Transform followTransform;
	// Use this for initialization
	public Vector3 relativeAttachmentPoint;
	Vector3 _attachmentPoint;

	void OnRectTransformDimensionsChange()
	{
		Debug.Log("tt");
	}
	public void setLabel(string label)
	{

		Text text=GetComponentInChildren<Text>();
		if (text!=null) text.text=label;

	}	void Start () {
		canvasRectTransform=GetComponentInParent<Canvas>().GetComponent<RectTransform>();
//		if (followTransform!=null) hoverable=followTransform.GetComponentInChildren<IHoverable>();
	if (followTransform==null) enabled=false;
	}
    protected virtual void LateUpdate()
    {  
		
		 _attachmentPoint=followTransform.position+relativeAttachmentPoint;
		
		 Vector3 screenPosHud = Camera.main.WorldToViewportPoint(_attachmentPoint);
        screenPosHud.x *= canvasRectTransform.rect.width;
        screenPosHud.y *= canvasRectTransform.rect.height;
        transform.position = screenPosHud;// + offset*currentScale;
    }

    public virtual void OnPointerClick(BaseEventData e)
	{

	//	Debug.Log("open stats pane",gameObject);
	}

	public virtual void OnHoverEnter(BaseEventData e)
	{
//if (hoverable!=null)
	//	hoverable.OnHover();
	}
public virtual void OnHoverExit(BaseEventData e)
	{
//		if (hoverable!=null)
//		hoverable.OnHoverExit();
	}
}
