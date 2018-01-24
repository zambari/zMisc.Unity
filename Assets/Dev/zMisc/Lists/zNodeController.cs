///zambari codes unity
// zNodeController 1.1

// this class is boilerplate code repository that evolved arout the concept of managing list of items (in the UI)
// 

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class zNodeController : MonoBehaviour

{
   public  NodeControllerSetup setup;
    [SerializeField]
    protected List<zNode> nodes;
 
    [SerializeField]
    [HideInInspector]
    protected zScrollRect scrollRect;
    protected Canvas canvas;

    protected RectTransform rect;
    protected int activeNodeIndex;

    [Header(" make protected ")]

    //    protected Vector2 startDrag;
    //  protected Vector2 startSize;

    //  protected bool scrollStateDirty;
    [SerializeField]
    Dictionary<string, zNode> templateDict;
    public Action OnNodeAdded;

    //  bool _scrolldir;
    /*   get { return scrollBar.direction == Scrollbar.Direction.BottomToTop; }
       set
       {
           if (value) scrollBar.direction = Scrollbar.Direction.BottomToTop;
           else scrollBar.direction = Scrollbar.Direction.TopToBottom;
       }*/

    [HideInInspector]
    public bool isHidden;
    void OnEnable()
    {
        OnValidate();
    }

    #region remoteNavigation


    public virtual void OnUp()
    {
        if (isHidden) return;
        if (activeNodeIndex < 0 || activeNodeIndex >= nodes.Count) activeNodeIndex = 0;
        nodes[activeNodeIndex].setAsActive(false);
        int startIndex = activeNodeIndex;

        do
        {
            activeNodeIndex--;
            if (activeNodeIndex < 0) activeNodeIndex = nodes.Count - 1;

        } while (activeNodeIndex != startIndex && !nodes[activeNodeIndex].gameObject.activeInHierarchy);

        nodes[activeNodeIndex].setAsActive(true);
        scrollToActive();


    }
    public virtual void OnDown()
    {
        if (isHidden) { Debug.Log("ishidden"); return; }
        if (activeNodeIndex < 0 || activeNodeIndex >= nodes.Count) activeNodeIndex = 0;
        nodes[activeNodeIndex].setAsActive(false);
        int startIndex = activeNodeIndex;
        do
        {
            activeNodeIndex++;
            if (activeNodeIndex >= nodes.Count) activeNodeIndex = 0;
        } while (activeNodeIndex != startIndex && !nodes[activeNodeIndex].gameObject.activeInHierarchy);
        nodes[activeNodeIndex].setAsActive(true);
        scrollToActive();


    }
    public virtual void GoParent()
    {
    }
    public virtual void OnLooseFocus()
    {
        canvas.enabled = false;
        isHidden = true;
    }
    public virtual void OnLeft()
    {
        if (isHidden) return;
        if (activeNodeIndex == 0)
            GoParent();
        else
        {
            nodes[activeNodeIndex].setAsActive(false);
            activeNodeIndex = 0;
            nodes[activeNodeIndex].setAsActive(true);
        }
        scrollToActive();
    }
    public virtual void OnRight()
    {
        if (isHidden) return;
        nodes[activeNodeIndex].setAsActive(false);
        activeNodeIndex = nodes.Count - 1;
        nodes[activeNodeIndex].setAsActive(true);
        scrollToActive();
    }
    public virtual void OnEnter()
    {
        if (isHidden) return;
        //        FileNode f = nodes[activeNodeIndex] as FileNode;
        //    bool isFile = f.type == FileNode.NodeTypes.file;
        //    nodes[activeNodeIndex].OnClick();
        //    if (isFile)
        //        OnLooseFocus();
    }

    public virtual void OnEscape()
    {
        canvas.enabled = !canvas.enabled;
        isHidden = !canvas.enabled;
    }

    public virtual void OnFocus()
    {
        canvas.enabled = true;
        isHidden = false;
    }
    #endregion

    public zNode AddNodeFromTemplate()
    {
        return AddNode("node", setup.nodeTemplatePool[0]);
    }
    public zNode AddNode(string nodeName, string templateName)
    {
        return AddNode(nodeName, getTemplate(templateName));
    }

    public zNode AddNode(string nodeName, zNode templateNode)
    {
        if (templateNode==null)
        {
            Debug.LogWarning("template does note exist !");
            return null;
        }
        zNode newNode = Instantiate(templateNode, setup.content);
        newNode.setLabel(nodeName);
        newNode.gameObject.SetActive(true);
        if (OnNodeAdded != null)
            OnNodeAdded();
        nodes.Add(newNode);
        return newNode;

    }
    public zNode AddNode(string nodeName)
    {
        return AddNode(nodeName, setup.nodeTemplatePool[0]);
    }

    /* 
        void _onNewNode()
        {
            scrollStateDirty = true;
        }*/

    public virtual void Clear()
    { 
        Debug.Log("removing "+ nodes.Count);
        for (int i = nodes.Count - 1; i >= 0; i--)
        {  
            zNode thisNode = nodes[i];
            nodes.Remove(thisNode);
            Debug.Log(" destroyin "+thisNode.gameObject.name,thisNode.gameObject);
            Destroy(thisNode.gameObject);
            Debug.Log(" post destroyin "+thisNode.gameObject.name,thisNode.gameObject);
        }

    }

    protected bool createTemplateDictionary()
    {
        if (setup.nodeTemplatePool == null) Debug.Log(gameObject.name + " has no templates, trying to Add", gameObject);
        else
        if (setup.nodeTemplatePool.Count == 0) Debug.Log(gameObject.name + " has  template Count==0, trying to Add", gameObject);
        else
        if (setup.nodeTemplatePool[0] == null) Debug.Log(gameObject.name + " templateePool[0]==null, trying to Add", gameObject);
        if (setup.nodeTemplatePool == null || (setup.nodeTemplatePool.Count == 0) || (setup.nodeTemplatePool[0] == null))
        {
            setup.nodeTemplatePool = new List<zNode>();
            zNode[] nodeComponents = GetComponentsInChildren<zNode>();
            Debug.Log(gameObject.name + "template add started " + nodeComponents.Length + " components found");
            foreach (zNode thisNode in nodeComponents)
                if (!String.IsNullOrEmpty(thisNode.getTemplateName()))
                    setup.nodeTemplatePool.Add(thisNode);
                else Debug.Log("non template node ? '" + thisNode.getTemplateName() + "'", thisNode.gameObject);
        }

        templateDict = new Dictionary<string, zNode>();
        for (int i = 0; i < setup.nodeTemplatePool.Count; i++)
            if (setup.nodeTemplatePool[i] != null &&
                setup.nodeTemplatePool[i].getTemplateName() != null)
                if (!templateDict.ContainsKey(setup.nodeTemplatePool[i].getTemplateName()))
                    templateDict.Add(setup.nodeTemplatePool[i].getTemplateName(), setup.nodeTemplatePool[i]);

        if (setup.nodeTemplatePool == null || setup.nodeTemplatePool.Count == 0 || setup.nodeTemplatePool[0] == null)
        {
            Debug.Log(gameObject.name + " has templatebool or object 0 is null", gameObject);
            return false;

        }
        zNode t = setup.nodeTemplatePool[setup.nodeTemplatePool.Count - 1];
        if (t.transform.parent == null) Debug.Log("no parent? wtf", gameObject);
        setup.templatePoolGO = t.transform.parent.gameObject;
        if (setup.templatePoolGO == null) Debug.Log(gameObject.name + " has no template pool", gameObject);
        else
            return true;
        return false;
    }

    public zNode getTemplate(string n)
    {
        if (templateDict == null)
            createTemplateDictionary();
        zNode t;
        if (templateDict.TryGetValue(n, out t))
            return t;
        else
        {
            Debug.Log(gameObject.name + " unknown template " + n + " dictionary has " + templateDict.Count + " entries", gameObject);
            foreach (string s in templateDict.Keys) Debug.Log(s);
        }
        return t;
    }
    protected virtual void scrollToActive()
    {
        /*  if (scrollAmount != 0 && activeNodeIndex > 0 && activeNodeIndex < nodes.Count)
          {
              if (nodes.Count > 1)
              {
                  float newScroll = activeNodeIndex * 1f / (nodes.Count - 1);
                  scrollBar.value = newScroll;
                  scrollContentSlider(newScroll);

              }

          }*/
    }
    public virtual void newNodeHovered(zNode hoveredNode)
    {
        /* 
         int nodeIndex = -1;
         for (int i = 0; i < nodes.Count; i++)
             if (nodes[i] == hoveredNode) nodeIndex = i;
         if (nodeIndex != -1)
         {
             if (activeNodeIndex >= 0 && activeNodeIndex < nodes.Count)
                 nodes[activeNodeIndex].setAsActive(false);
             activeNodeIndex = nodeIndex;
             nodes[activeNodeIndex].setAsActive(true);
         }*/
    }

    public virtual void activeNodeClicked()
    {
        if (activeNodeIndex >= 0 && activeNodeIndex < nodes.Count)
            nodes[activeNodeIndex].OnClick();
    }
    public virtual void nodeValueChanged()
    {

    }
    public bool activeNodePresent()
    {
        if (activeNodeIndex >= 0 && activeNodeIndex < nodes.Count && nodes[activeNodeIndex].gameObject.activeInHierarchy) return true;
        return false;
    }
    public virtual void setHeight(float f) // size
    {
        if (nodes == null) return;
        for (int i = 0; i < nodes.Count; i++)
            nodes[i].setHeight(f);

    }

    public virtual void NodeClicked(zNode node)
    {

    }


    Color savedColor = new Color(0, 0, 0, 0);
    bool disableimage;
    public virtual void highlightPanel(BaseEventData eventData)
    {
        if (setup.image != null)
        {
            if (setup.image.enabled == false) disableimage = true; ;
            setup.image.enabled = true;
            savedColor = setup.image.color;
            setup.image.color = setup.panelEditColor;
        }
    }
    public virtual void restoreColor(BaseEventData eventData)
    {
        if (setup.image != null)
        {
            if (disableimage) setup.image.enabled = false;
            else
                setup.image.color = savedColor;
        }
    }

    protected virtual void OnValidate()
    {
        
        if (setup.rescanTemplates)
        {
            setup=new NodeControllerSetup();
        }
        if (setup.text == null) setup.text = GetComponentInChildren<Text>();

        if (enabled && gameObject.activeInHierarchy)
           createTemplateDictionary();


            FindSetupObjects();
    /*if (gameObject.activeInHierarchy) return;            
        if (scrollRect == null)
            scrollRect = GetComponent<zScrollRect>();
        if (scrollRect == null)
        {
            gameObject.AddComponent<zScrollRect>();
        }
        if (scrollRect != null)
        {
            scrollRect.verticalScrollbar.direction = Scrollbar.Direction.BottomToTop;
            scrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
        }*/
    }
protected virtual void FindSetupObjects()
{
        nodes = new List<zNode>();
        setup.image = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();

        rect = GetComponent<RectTransform>();
        Mask m=GetComponentInChildren<Mask>();
        if (scrollRect==null) scrollRect=GetComponentInChildren<zScrollRect>();
        setup.contentMaskRect=m.GetComponent<RectTransform>();
              //  createTemplateDictionary();
        
}
    


    protected virtual void Awake()
    {   FindSetupObjects();
        if (setup.templatePoolGO != null)
        {
            GameObject contentGO = Instantiate(setup.templatePoolGO, setup.templatePoolGO.transform.parent);
            setup.content = contentGO.GetComponent<RectTransform>();
         
           if (scrollRect!=null) scrollRect.content = setup.content;

            for (int i = setup.content.transform.childCount - 1; i >= 0; i--)
                if (Application.isPlaying) Destroy(setup.content.transform.GetChild(i).gameObject);
                else DestroyImmediate(setup.content.transform.GetChild(i).gameObject);
            setup.content.name = "CONTENT";

            Mask m = setup.content.GetComponentInParent<Mask>();
            if (m == null) Debug.Log("no mask");
            else
                setup.contentMaskRect = m.GetComponent<RectTransform>();
            setup.templatePoolGO.SetActive(false);
        }
        else Debug.Log("no template pool");

    

    }

    protected virtual void Start()
    {

    }


}
