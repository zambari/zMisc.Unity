using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragHelper : MonoRect, IBeginDragHandler, IDragHandler,IPointerEnterHandler, IPointerExitHandler {
public enum Direction {Horizontal, Vertical,HorizontalReversed, VerticalReversed}
public Direction direction;
Vector2 lastPostion;
public FloatEvent dragValue;

public Color normalColor=Color.red;
public Color hoverColor=Color.red*0.6f;
public float valueScaler=1;
public void OnPointerEnter(PointerEventData e)
{
	image.color=hoverColor;

}
public void OnPointerExit(PointerEventData e)
{
	image.color=normalColor;
}
public void OnBeginDrag(PointerEventData e)
{
	lastPostion=e.position;

}
public void OnDrag(PointerEventData e)
{
	float dragAmount;
	if (direction==Direction.Horizontal || direction==Direction.HorizontalReversed)
	{
		dragAmount=e.position.x-lastPostion.x;
	}else
	dragAmount=e.position.y-lastPostion.y;

	if (direction==Direction.HorizontalReversed || direction==Direction.VerticalReversed)
	dragAmount*=-1;

	dragValue.Invoke(dragAmount*valueScaler);
	lastPostion=e.position;
}
	// Use this for initialization
	 void OnValidate()
		{
			image.color=normalColor;
			
		} 
		
	
}
