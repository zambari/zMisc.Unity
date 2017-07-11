
	//z2k17

using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.IO;


public class FileNode : zNode
{
    public Text label;
    public RectTransform mainButton;
    //public zFileBrowser1 zFileBrowser1;
    public int indentationConstant = 12;
    public int depth;
    public string fileName;
  public HoverableColors colors;

   // public override void setColor()

    public override void setColor()
    {
     //   if (customColor) return;
        if (controller == null) controller = GetComponentInParent<zNodeController>();
            
        if (image != null && controller != null)
        {
            if (isActive)
                image.color = colors.activeColor;
            else
           if (isHovered) image.color = colors.hoveredColor;
            else image.color = colors.normalColor;
        }
    }
    
      // List<FileNode> childItems;
   // public enum NodeTypes {file, directory, parent }
   // public NodeTypes type;
   
/* *
    public void fold()
    {
        for (int i = 0; i < childItems.Count; i++)
      //      childItems[i].gameObject.SetActive(false);
        isExpanded = false;
        setLabel();
        // zFileBrowser1.childTransformChanged(-childItems.Count);
    }
    public void unfold()
    {
        for (int i = 0; i < childItems.Count; i++)
            childItems[i].gameObject.SetActive(true);
        isExpanded = true;
        setLabel();
      
    }
   */
  public  void setLabel()
    {
       /* if (type==NodeTypes.directory)
         label.text = "     " + nodeName;
         else*/
         label.text =  nodeName;
   
    } 
  protected   void OnValidate()
    {
        colors.OnValidate(this);
        image.color = colors.normalColor;

    }  
    
      public void setDepth(int i)
    {
         depth = i; 
         mainButton.sizeDelta =   mainButton.sizeDelta+ new Vector2(-depth * indentationConstant, 0);
         mainButton.anchoredPosition =          mainButton.anchoredPosition +new Vector2(depth * indentationConstant, 0);
    }
/* 
    public void link(NodeTypes t, string path, int i)
    {
        
      //  type=t;
        file = path;
      //  if (type==NodeTypes.parent) 
        nodeName="..";
        else
        {
        string[] pathsplit = path.Split('/');
        nodeName=pathsplit[pathsplit.Length-1];
        }
        name = "["  + nodeName +"]";
        setLabel();
        
    }
*/
    
       
}


/*public class FileNode : MonoBehaviour,IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Text label;
    public RectTransform myRect;
    public RectTransform mainButton;

    public zFileBrowser1 zFileBrowser1;
    public int indentationConstant = 12;
    [Header("Filled at runtime")]
    public string nodeName;

    public int depth;
  
    List<FileNode> childItems;
    public enum NodeTypes {file, directory, parent }
    public NodeTypes type;
    Image image;
    bool isExpanded;
	public Color nonHoveredColor= new Color(1,1,1,0.1f);
	public Color hoveredColor= new Color(1,1,1,0.3f);

public string file;
   void OnValidate()
    {
        image = GetComponent<Image>();
        image.color = nonHoveredColor;
    }
    void Start()
    {
        image=GetComponent<Image>();
        myRect=GetComponent<RectTransform>();

    }
    public void fold()
    {
        for (int i = 0; i < childItems.Count; i++)
            childItems[i].gameObject.SetActive(false);
        isExpanded = false;
        setLabel();
         zFileBrowser1.childTransformChanged(-childItems.Count);
    }
    public void unfold()
    {
        for (int i = 0; i < childItems.Count; i++)
            childItems[i].gameObject.SetActive(true);
        isExpanded = true;
        setLabel();
      
    }
    // public void  OnPointerEnter(PointerEventData eventData) 
    public void  OnPointerClick(PointerEventData eventData) 
    {
if (eventData.button!=0) return;
     
     if (type==NodeTypes.file)
     {  IOpenFiles opener=GetComponentInParent<IOpenFiles>();
          if (opener!=null) opener.openFile(file); else Debug.Log("add opener to file browser");
     }
      if (type==NodeTypes.directory)
               zFileBrowser1.scanDir(file);
       if (type==NodeTypes.parent)
       {
             zFileBrowser1.scanDir( Directory.GetParent(file).FullName);
       }
    }

    public void  OnPointerEnter(PointerEventData eventData) 
    {

       image.color=hoveredColor;
	   eventData.Use();
    }
    public void  OnPointerExit(PointerEventData eventData) 
    {
		image.color=nonHoveredColor;
        eventData.Use();
    }
    void setLabel()
    {
  
         label.text =  nodeName;
   
    }
    public void setDepth(int i)
    {
         depth = i; 
         mainButton.sizeDelta =   mainButton.sizeDelta+ new Vector2(-depth * indentationConstant, 0);
         mainButton.anchoredPosition =          mainButton.anchoredPosition +new Vector2(depth * indentationConstant, 0);
    }

    public void link(NodeTypes t, string path, int i)
    {
        
        type=t;
        file = path;
        if (type==NodeTypes.parent) 
        nodeName="..";
        else
        {
        string[] pathsplit = path.Split('/');
        nodeName=pathsplit[pathsplit.Length-1];
        }
        name = "["  + nodeName +"]";
        setLabel();
        
    }

    
       
}
*/