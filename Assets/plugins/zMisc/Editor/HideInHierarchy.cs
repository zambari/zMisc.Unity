using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class HideInHierarchy : MonoBehaviour {
    public string orgObjName;
    // Use this for initialization
    [ReadOnly]
    public int numberOfChildren;
    [ReadOnly]
    public bool globalUnhideOn;
  
    public bool hideChildren=true;
    public bool hidingDisabled;

    public static bool _globalUnhide=true;
   // public static UnityEvent toggleUnityAction;
   /* public void globalToggled()
    {

        globalUnhideOn = _globalUnhide;
        HideUnhide();
     
    }*/
    private void Update()
    {
        if (Input.GetKeyDown("h"))
        { hideChildren = true; }
        if (Input.GetKeyDown("g"))
        { hideChildren = false; }
        if (globalUnhideOn != _globalUnhide)
        {
            globalUnhideOn = _globalUnhide;
            HideUnhide();

        }
         
    }

    private void OnDestroy()
    {
        if (gameObject.name != orgObjName) gameObject.name = orgObjName;
    }
    void HideUnhide()
    {
        
       
        if (hideChildren && !globalUnhideOn)
        {
            for (int i = 0; i < transform.childCount; i++)

                transform.GetChild(i).gameObject.hideFlags = HideFlags.HideInHierarchy;
                   gameObject.name = ">"+orgObjName + " [" + numberOfChildren + "]";
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)

                transform.GetChild(i).gameObject.hideFlags = HideFlags.None;
            if (orgObjName.Length > 1) gameObject.name = orgObjName;
        }
        numberOfChildren = transform.childCount;
    
    }
 
    private void OnValidate()
    {
        if (hidingDisabled)
        {
            hidingDisabled = false;
            _globalUnhide = !_globalUnhide;
       //     prepareEvent();
         //  HideInHierarchy.toggleUnityAction.Invoke();
         }
        
        HideUnhide();
     

    }
    private void OnEnable()
    {
        HideUnhide();


    }
  /*  void prepareEvent()
    {
        if (HideInHierarchy.toggleUnityAction == null) HideInHierarchy.toggleUnityAction = new UnityEvent();
        HideInHierarchy.toggleUnityAction.RemoveListener(globalToggled);
        HideInHierarchy.toggleUnityAction.AddListener(globalToggled);
    }*/
    private void Awake()
    {
        if (orgObjName==null)

            orgObjName = gameObject.name;
      //  prepareEvent();
        OnValidate();
        
    }


}
