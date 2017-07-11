using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace Z
{
    public class SafeUIBase : MonoBehaviour
    {

        protected bool checkIfPersistentToggle(Toggle.ToggleEvent target, UnityAction<bool> action)
        {
          #if UNITY_EDITOR

          //  target.RemoveAllListeners();
            int count = target.GetPersistentEventCount();
			if (count==0)
			{
UnityEventTools.AddPersistentListener(target, action);
 target.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
			}
       /*     bool foundUs = false;
            for (int i = count-1; i > 0; i--)
            {
                Object o = target.GetPersistentTarget(i);
                if (o == this)
                {
               
                    if (!foundUs)
                    {
                        target.SetPersistentListenerState(i, UnityEventCallState.EditorAndRuntime);
                        foundUs = true;
                    }
                    else
                        UnityEventTools.RemovePersistentListener(target, i);
                    // return true;
                }
                //		 string s=toggle.onValuChanged.GetPersistentTarget(i);
            }

            if (!foundUs)
            { UnityEventTools.AddPersistentListener(target, action);
              //  target.AddListener(action);

            }*/

            #endif
            return true;


        }





        protected bool checkIfPersistent(Slider.SliderEvent target, UnityAction <float>action)
        {
#if UNITY_EDITOR
          int count = target.GetPersistentEventCount();
          if (count!=0) return false;
           
            target.RemoveAllListeners();
            UnityEventTools.AddPersistentListener(target,action);
            target.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
            #endif
            return true;
       //     bool foundUs = false;
       /*     for (int i = 0; i < count; i++)
            {
                Object o = target.GetPersistentTarget(i);
                if (o == this)
                {
                    foundUs = true;
                    target.SetPersistentListenerState(i, UnityEventCallState.EditorAndRuntime);
                    return true;
                }
                //		 string s=toggle.onValuChanged.GetPersistentTarget(i);
            }

            if (!foundUs)
            {
                target.AddListener(action);

            }
           */



        }



    }
}