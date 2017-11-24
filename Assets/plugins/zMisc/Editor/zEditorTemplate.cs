using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using UnityEngine.EventSystems;
using System.Linq;

public class zEditorTemplate : zEditorBase
{ 
// [MenuItem("Tools/Open zEditorHelper")]
//  static void Init()
//    {
//      BaseInit(typeof(zEditorTemplate));
//    }

    void DisplayMyTab()
    {
        Label("test");
    }

    protected override void DisplayTab(string tabName)
    {
        if (tabName=="myTab") DisplayMyTab();
    }

    protected override bool ShouldTabBeVisible(string s) // override to enable context sensitive tab switching
    {
       // if (s=="myTab") return SelectionHasComponentsAll<Image>();
        return true;
    }

    protected override void AddTabs()  
    {
            AddTab("myTab");
            AddTab("myTab2");
    }

   protected override string baseName { get { return "Template"; } set {  } }  // override
   protected override bool showConfig { get { return true; } set {} }
  protected override void AddOptions()
  {
    //   AddOption("my Option",true);
       //we later get them via GetOption("my Option") which is implemented via dictionary search
  }
  

  protected override void OnSelectionChange()
    {
        GetAvailableTools();
        Repaint();
    }

    protected override void OnToolChange()
    {

    }
/*
 Some usefulMethods:
   IfButton("", ()=> {    } );


protected void BH() {     GUILayout.BeginHorizontal(); }
 protected void EH() {     GUILayout.EndHorizontal(); }
 protected void BV() {     GUILayout.BeginVertical(); }
 protected void EV() {     GUILayout.EndVertical(); }

 protected void PerformOnSelection(Action<GameObject> actionToPerform)
 protected bool objectIsSelected; {get;set;}
 protected bool nothingSelected { get;set;}
 protected GameObject selObj
 protected bool IfButton(string buttonLabel, Action actionToPerform, bool children = false) where T : Component
 protected bool IfButtonDoOnComponents<T>(string buttonLabel, Action<T> actionToPerform, bool children = false) where T : Component
 bool SelectionHasComponentsSome<T>(bool inChildren=false) where T: Component
 bool SelectionHasComponentsAll<T>(bool inChildren=false) where T: Component
 bool IfHasComponentAndClicked<T>(string buttonLabel, Action<T> actionToPerform, bool children = false) where T : Component
 bool GetOption(string s)
 void StoreOption(string opName, bool newVal)
 void Space()
 void Label(string s);
 void DisplayColumns(Action col1, Action col2)
 void PerformOnComponents<T>(Action<T> actionToPerform, bool children = false) where T : Component
    
 */


}
