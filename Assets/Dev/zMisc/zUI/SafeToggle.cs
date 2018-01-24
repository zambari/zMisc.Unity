using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
//using UnityEditor.Events;
#endif
namespace Z
{
    [RequireComponent(typeof(Toggle))]
    public class SafeToggle : SafeUIBase
    {
        Toggle _Toggle;
        public bool triggerToggleOnStart = true;

        [SerializeField]
        bool _isON;
        public bool testToggle;
        public bool isOn
        {
            get { return _isON; }
            set
            {if (ignoreToggle) 
            { if (_isON != value) Debug.Log(" ignore at input ");
               return;
            }
                _isON = value;
                //	testToggle=_isON;
                ignoreToggle = true;
                toggle.isOn = _isON;
               
                ToggleValueChangedHandler(_isON);
                ToggleEventSafe.Invoke(_isON);
                ToggleEventInverted.Invoke(!_isON);
                if (_isON)
                whenOn.Invoke() ;
                else
                whenOff.Invoke();
                 ignoreToggle = false;
            }
        }
        public void toggleS(string s)
        {
            isOn = !_isON;
        }
        protected Toggle toggle
        {
            get
            {

                if (_Toggle == null) _Toggle = GetComponentInChildren<Toggle>();
                return _Toggle;
            }
        }
        public BoolEvent ToggleEventSafe;
        public BoolEvent ToggleEventInverted;
        public UnityEvent whenOn;
        public UnityEvent whenOff;
        bool ignoreToggle = true;

public void ToggleOn()
{
    isOn=true;
}

public void ToggleOff()
{
    isOn=true;
}
        void OnValidate()
        {
           
            isOn = _isON;
            checkIfPersistentToggle(toggle.onValueChanged, ToggleValueChangedHandler);
          //  if (ToggleEventSafe.GetPersistentEventCount() > 0)
          //  {
            //    ToggleEventSafe.SetPersistentListenerState(0, UnityEventCallState.EditorAndRuntime);
          //  }
        }

        void Start()
        {
            ignoreToggle = false;
            //   if (toggle != null)
            //      checkIfPersistent();
            if (triggerToggleOnStart)
                ToggleValueChangedHandler(toggle.isOn);
isOn=_isON;
            ///        if (triggerToggleOnStart) ToggleValueChangedHandler(toggle.isOn);

        }
        void ToggleValueChangedHandler(bool f)
        {

            /*      if (!Application.isPlaying) {
                      toggle.enabled=false;
                      toggle.enabled=true;
                  }*/
            if (ignoreToggle)
            {

                // Debug.Log("dden",gameObject);
                return;
            } //else Debug.Log("en",gameObject);
             isOn=f;
        }
    }
    }