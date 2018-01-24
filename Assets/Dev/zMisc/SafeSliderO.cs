using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
[ExecuteInEditMode]
public class SafeSliderO : SafeUIBase
{
    [Header("Safe Event")]
    public FloatEvent sliderEventSafe;
    public bool triggerOnStart = true;

    public Slider slider;
    bool ignoreSlider;
    [Header("labels")]
    public bool createText;
    public Text valueText;
    public Text labelText;
    public bool autoExpandSlider;
    public bool sliderMinMaxNormalized;
    public void setToStartVal()
    {
        slider.value=startValue;
    }
    public bool addlistener;
public bool invertValue;
    float startValue;
 // [SerializeField]
 // [ReadOnly]
//    float _maxAllowed=1;
    public void setMaxAllowed(float f)
    {
//_maxAllowed=f;
    }
    void OnValidate()
    {
        if (!gameObject.activeInHierarchy) return;
        if (swapLabels)
        {
            swapLabels = false;
            swapTextObjects();
        }
        if (addlistener) {  checkIfPersistent( slider.onValueChanged,recieveValueFromSlider); }

        checkObjects();
        if (createText)
        {
            createTexts();

        }
        setTextLabels();
        if (valueText == null)
        {

        }
        if (slider.minValue == 0 && slider.maxValue == 1) sliderMinMaxNormalized = true;
        else
        {
            if (sliderMinMaxNormalized)
            {
                slider.minValue = 0;
                slider.maxValue = 1;

            }
        }
        recieveValueFromSlider(send);
    }
    public void createTexts()
    {   if (slider==null) return;
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
    public bool swapLabels;

    void swapTextObjects()
    {
        Text a = labelText;
        labelText = valueText;
        valueText = a;
    }

    void setTextLabels()
    { if (slider==null) return;
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

    void checkObjects()
    {

        if (slider == null)
            slider = GetComponent<Slider>();
        if (valueText == null)
            valueText = GetComponentInChildren<Text>();

    }

    void OnAwake()
    {
        setTextLabels();
    }
    void OnEnable()
    {
        setTextLabels();
    }
  public void value(float f)
  {
      setSliderValue(f);
  }
  public bool sendEvents=true;
  public bool takeEvents=true;
  [Range(0,1)]
  public float a;
  [Range(0,1)]
  public float b;
    [Range(0,1)]
  public float send;
  float getInverted(float norm)
  {
        a=(slider.maxValue-slider.minValue)-norm;
        return a;
  }

    float setInverted(float norm)
  {
        b=(slider.maxValue-slider.minValue)-norm;
        return b;
  }
    public void setSliderValue(float f)
    {
        if (!gameObject.activeInHierarchy||!enabled) return;
        if (!takeEvents) return;
        ignoreSlider = true;
        {
            if (f > slider.maxValue)
            {
                if (autoExpandSlider) slider.maxValue = f;
            }
            if (f < slider.minValue)
            {
                if (autoExpandSlider) slider.minValue = f;
            }
        }
        b=getInverted(f);
        slider.value = f;
        send=f;
        ignoreSlider = false;

    }
    void Reset()
    {
        OnValidate();
    }

    public void setSliderMaxValue(float f)
    {

        slider.maxValue = f;

    }
    public void setSliderMinValue(float f)
    {

        slider.minValue = f;

    }
    void Start()
    {

        checkObjects();
    startValue=slider.value;
        if (slider != null)
        {
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(recieveValueFromSlider);
        }
        if (triggerOnStart)
            recieveValueFromSlider(slider.value);
    }
 

      public void recieveValueFromSlider(float f)
    {
        if (ignoreSlider||!sendEvents) 
        {
         //   Debug.Log("inored");
             return;
        }
         if (!gameObject.activeInHierarchy||!enabled) return;
    // Debug.Log("sending "+f);
    getInverted(f);
    if (sliderEventSafe!=null)
         sliderEventSafe.Invoke(f);
        if (valueText != null) valueText.text = f.ToShortString();
    }
}
