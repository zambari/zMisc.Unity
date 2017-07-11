using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
[ExecuteInEditMode]
public class SafeSlider : SafeUIBase
#pragma warning disable 414
{
float checkRange(float value)
{       

                    if (value > allowedMax)
            /*        {
                        if (autoExpandSlider) maxValue = value;
                    }
                    if (value < slider.minValue)
                    {
                        if (autoExpandSlider) minValue = value;
                    }
                }*/
                if (value >=  MinMaxRange.allowedMin && value <= _allowedMax)
                {
                  return value;
                }
                else
                {
                    if (value > allowedMax)
                    {
                        float f = value - _allowedMax;

                        MinMaxRange.overFlow.Invoke(-f);
                        value = allowedMax;
                        Debug.Log("value ouside range (" + value + ") where max allowed is " + allowedMax + " slider max is " + slider.maxValue, gameObject);
                    }
                    if (value < allowedMin)
                    {
                        float f = _allowedMin = value; ;

                        MinMaxRange.overFlow.Invoke(-f);
                        value = allowedMin;
                        Debug.Log("value ouside range (" + value + ") where max allowed is " + allowedMin + " slider max is " + slider.minValue, gameObject);
                    }


                }
return value;
}
    public float value
    {
        get
        {
            return slider.value;

        }
        set
        {
            if (!gameObject.activeInHierarchy || !enabled) return;
            if (!takeEvents) return;
            ignoreSlider = true;
            {
                lastValueRecievedFromOutside=value;
                   if (MinMaxRange.useLimits)
                {
                value=    checkRange(value);
                }
              //   testValue = lastValueRecievedFromOutside/(slider.maxValue-slider.minValue)-slider.minValue;
                  testrecieveValue = lastValueRecievedFromOutside/(slider.maxValue-slider.minValue)-slider.minValue;
              //   ignoreSlider=true;
                    slider.value = value;
//Debug.Log("sl");
                    ignoreSlider = false;

             

       
            }

        }
    }

    /*  public void value(float f)
       {
           setSliderValue(f);
       }
   */

 float  testrecieveValue;
    float startValue;
[ReadOnly]
  //  [SerializeField]
   float lastValueRecievedFromSlider;
   [ReadOnly]
   // [SerializeField]
   float lastValueRecievedFromOutside;
    [Header("Safe Event")]
    public FloatEvent sliderEventSafe;
    public bool triggerOnStart = true;

    public Slider slider;

    [Range(0, 1)]
     float testValue;
    public bool sendEvents = true;
    public bool takeEvents = true;
    bool ignoreSlider;
    [Header("value")]


    public bool invertValue;
    public bool autoExpandSlider;
    public bool sliderMinMaxNormalized;

    [Header("labels")]
    public bool createText;
    public Text valueText;
    public Text labelText;
    public MinimumMaximumRange MinMaxRange;
    public void setToStartVal()
    {

       // slider.value = startValue;

    }
public float setSlidexMax=1;
public float setSlidexMin;
    void OnValidate()
    {  
        if (MinMaxRange.reset)
        {
            MinMaxRange.reset = false;
            allowedMin = 0;
            allowedMax = 1;
        }

        if (!gameObject.activeInHierarchy || !enabled) return;
        if (MinMaxRange.allowedMax == 0) MinMaxRange.allowedMax = 1;
        checkObjects();
        { checkIfPersistent(slider.onValueChanged, recieveValueFromSlider); }


        if (createText)
        {
            createTexts();

        }
        setTextLabels();
        if (valueText == null)
        {

        }
        if (slider.minValue == 0 && slider.maxValue == 1)
        {
            // sliderMinMaxNormalized = true;
        }
        else
        {
            if (sliderMinMaxNormalized)
            {
                slider.minValue = 0;
                slider.maxValue = 1;

            }
        }
        allowedMax = MinMaxRange.allowedMax;
        allowedMin = MinMaxRange.allowedMin;
        if (sendEvents && sliderEventSafe != null && slider != null &&!Application.isPlaying)
        {
          //  float
  //value =if () return;
   //  slider.value =slider.minValue +testValue * (slider.maxValue - slider.minValue);
            /*newSliderValue((testValue - slider.minValue) / );
            value = ((testValue - slider.minValue) / (slider.maxValue - slider.minValue));*/
        }
        else testValue = slider.value;
    }
    public void createTexts()
    {
        if (slider == null) return;
        createText = false;
        if (valueText == null)
        {
            GameObject go = new GameObject();
            RectTransform thisRect = go.getRect();
            thisRect.SetParent(slider.gameObject.getRect());
            thisRect.anchorMin = new Vector2(0, 0.5f);
            thisRect.anchorMax = new Vector2(0, 0.5f);
            thisRect.pivot = new Vector2(0, 0.5f);
            thisRect.sizeDelta = new Vector2(170, 80);
            thisRect.anchoredPosition = new Vector2(10, -15);
            Text t = thisRect.gameObject.AddComponent<Text>();
            t.alignment = TextAnchor.MiddleLeft;
            valueText = t;
            t.raycastTarget = false;

        }
        if (labelText == null)
        {
            GameObject go2 = new GameObject();
            RectTransform thisRect2 = go2.getRect();
            thisRect2.SetParent(slider.gameObject.getRect());
            thisRect2.anchorMin = new Vector2(1, 0.5f);
            thisRect2.anchorMax = new Vector2(1, 0.5f);
            thisRect2.pivot = new Vector2(1, 0.5f);
            thisRect2.sizeDelta = new Vector2(170, 80);
            thisRect2.anchoredPosition = new Vector2(-10, -15);
            Text t2 = thisRect2.gameObject.AddComponent<Text>();
            t2.alignment = TextAnchor.MiddleRight;
            t2.raycastTarget = false;
            labelText = t2;

        }
        setTextLabels();
    }

    void swapTextObjects()
    {
        Text a = labelText;
        labelText = valueText;
        valueText = a;
    }

    void setTextLabels()
    {
        if (slider == null) return;
        if (valueText != null)
        {
            valueText.name = "ValueDisplay" + slider.name;
            if (slider != null)
                valueText.text = slider.value.ToShortString();
            valueText.raycastTarget = false;
        }

        if (labelText != null)
        {
            labelText.text = name;
            labelText.name = "LabelText";
            labelText.raycastTarget = false;
        }

    }

    float _allowedMin;

    float _allowedMax = 1;
    // [SerializeField]
    //  float allowedMinSet;
    // [SerializeField]
    // float allowedMaxSet = 1;

    public float allowedMin
    {
        get { return MinMaxRange.allowedMin; }
        set
        {
            if (value < _allowedMax)
            {
                if (!MinMaxRange.useLimits) return;

                _allowedMin = value;
                MinMaxRange.allowedMin = value;

                if (_allowedMin < slider.minValue) slider.minValue = _allowedMin;
            }
            if (slider.value < allowedMin)
            {
                float f = allowedMin - slider.value;
                Debug.Log("while chanign allowed minium" + _allowedMin + ", we found value " + slider.value + " to be too low, corrected and sent overflow " + f, gameObject);
                slider.value = allowedMin;
                MinMaxRange.overFlow.Invoke(-f);

            }

            MinMaxRange.allowedMin = value;
        }
    }
    public float allowedMax
    {
        get { return MinMaxRange.allowedMax; }
        set
        {
            if (value > _allowedMin)
            {
                if (!MinMaxRange.useLimits) return;

                _allowedMax = value;
                MinMaxRange.allowedMax = value;
                if (_allowedMax > slider.maxValue) slider.maxValue = _allowedMax;

                if (slider.value > allowedMax)
                {
                    float f = slider.value - allowedMax;
                    Debug.Log("while chanign allowed max " + _allowedMax + ", we found value " + slider.value + " to be too high sent overflow " + f, gameObject);
                    slider.value = allowedMax;
                    MinMaxRange.overFlow.Invoke(-f);
                }
            }
            MinMaxRange.allowedMax = _allowedMax;
        }
    }

    void checkObjects()
    {

        if (slider == null)
            slider = GetComponent<Slider>();
        //   if (valueText == null)
        //        valueText = GetComponentInChildren<Text>();

    }

    void OnAwake()
    {
        setTextLabels();
    }
    void OnEnable()
    {
        setTextLabels();
    }


    float getInverted(float norm)
    {

        return (slider.maxValue - slider.minValue) - norm;
    }

    public void setSliderValue(float f)
    {
        value = f;
        Debug.Log("depreciated call", gameObject);

    }

    void Reset()
    {
        allowedMax = 1;
        OnValidate();
    }

    public float maxValue
    {
        get { return slider.maxValue; }
        set
        {
            if (MinMaxRange.adjustWithSliderRange)
                allowedMax = value;
            slider.maxValue = value;

        }
    }

    public float minValue
    {
        get { return slider.minValue; }
        set
        {

            if (MinMaxRange.adjustWithSliderRange)
                allowedMin = value;
            slider.minValue = value;

        }
    }

    void Start()
    {
        if (slider == null) return;
        checkObjects();

        startValue = slider.value;
        lastValueRecievedFromSlider = slider.value;

        if (triggerOnStart && sliderEventSafe != null)
            recieveValueFromSlider(slider.value);
    }
    void newSliderValue(float f)
    {
        recieveValueFromSlider(f);
    }
    public void offsetValue(float f)
    {
        if (slider.value + f <= allowedMax &&
             slider.value + f >= allowedMin)
            slider.value += f;
    }

    /// <summary>        sliderEventSafe.Invoke(f);
    ///  this should not be called manually
    /// </summary>
    /// <param name="f"></param> <summary>
    /// 
    /// </summary>
    /// <param name="f"></param>
    public void recieveValueFromSlider(float f)
    {
        if (ignoreSlider || !sendEvents || !gameObject.activeInHierarchy || !enabled)
        {
            return;
        }
    
        lastValueRecievedFromSlider = f;
         if (invertValue) f=getInverted(f);
            
             testValueIn=f/(slider.maxValue-slider.minValue)-slider.minValue;
 
            sliderEventSafe.Invoke(f);
      

        if (valueText != null) valueText.text = f.ToShortString();
    }
    [Range(0,1)]
      public float  testValueIn;//=f/(slider.maxValue-slider.minValue)-slider.minValue;
}
namespace Z
{
    [System.Serializable]
    public class MinimumMaximumRange
    {
        public bool useLimits;
        public bool reset;
        public float allowedMin;
        public float allowedMax = 1;
        public FloatEvent overFlow;
        public bool adjustWithSliderRange;

    }
}