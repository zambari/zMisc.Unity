//z2k17

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//using System.Collections;
//using System.Collections.Generic;

public class zNode : MonoRect, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected zNodeController controller;
    public UnityEvent onClickAction;
    public string nodeName;
    protected LayoutElement layoutElement;
    protected bool isActive;
    protected bool isHovered;
    public bool isDisabled;

    public bool activateOnHover=false;


    public string getTemplateName()
    {
    if (name.Contains("{") && name.Contains("}"))
        {
            string templateName=(name.Substring(1, name.Length - 2));
              char[] a = templateName.ToCharArray();
              a[0] = char.ToUpper(a[0]);
              templateName=new string(a);
              gameObject.name="{"+templateName+"}";
            return templateName;
        }

        return null;

    }


    public virtual void setColor()
    {
     //   if (customColor) return;
        if (controller == null) controller = GetComponentInParent<zNodeController>();
            
        if (image != null && controller != null)
        {
            if (isActive)
                image.color = controller.setup.activeColor;
            else
           if (isHovered) image.color = controller.setup.hoveredColor;
            else image.color = controller.setup.nonHoveredColor;
        }
    }
    public virtual void setLabel(string s)
    {
       // Debug.Log("setting labe;");
        if (text != null) text.text = s;
        nodeName = s;
        name="<"+nodeName+">";

    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {

        if (eventData.button != 0) return;
        OnClick();
    }


    public virtual void setAsActive(bool a)
    {
        isActive = a;
        setColor();
    }
    public virtual void OnClick()
    {
        if (controller!=null) controller.NodeClicked(this);
        if (onClickAction!=null)  onClickAction.Invoke();
    }
    public void setHeight(float f)
    {
        if (layoutElement == null) layoutElement = GetComponent<LayoutElement>();
        layoutElement.preferredHeight = f;

    }
    protected virtual void Start()
    {
        setColor();
    }


    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (isDisabled) return;
        isHovered = true;
        setColor();

        eventData.Use();
        if (controller!=null) controller.newNodeHovered(this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDisabled) return;
        isHovered = false;
        setColor();

        eventData.Use();
    }


    public virtual void setLabelToNodeName()
    {
        setLabel(nodeName);
        
    }
}
