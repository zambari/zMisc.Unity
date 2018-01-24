using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Z;
//namespace Z{
/// <summary>
/// This class is meant to provide a simple way to set and access parameters of generic type (stored as string). To provide access from UnityEngine.UI level (UI elements can provide parameters, but only methods with one parameter are visible)
/// write access has been split into two calls - SetNextValueName and SetValue
/// </summary>


[ExecuteInEditMode]
public class Settings : MonoBehaviour
{
    public static Action settingsUpdated;
   public static Settings instance;
    string _nextName;
    public static Dictionary<string, string> fields;

    [Header("preview only, do not change here")]
    public List<string> optionNames;
    public List<string> optionValues;



    void OnEnable()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError(" two generic option providers!", gameObject);
            Debug.LogError(" two generic option providers!", instance.gameObject);
        }
        instance = this;
        if (fields == null) fields = new Dictionary<string, string>();
    }

    [ReadOnly]
    [SerializeField]
    int recordCount;
    [ReadOnly]
    [SerializeField]
    int nameCallCount;
    [ReadOnly]
    [SerializeField]
    int valueCallCount;
    float timeOfNameSet;


    //BEGIN * UI ! those two methods are meant to be called in sequence from UnityEvent callers *BEGIN
    public void SetThisValueNameUI(string nextName)
    {
        nameCallCount++;
        timeOfNameSet = Time.time;
        _nextName = nextName;
    }
    public void SetThisValueUI(string valueString)
    {
        if (timeOfNameSet != Time.time) Debug.Log("settings warning : name field " + _nextName + " was set at a different frame than current");
        valueCallCount++;
        if (valueCallCount != nameCallCount) Debug.Log(" warning, vallue call count " + valueCallCount + " is no equal to name call count " + nameCallCount);
        SetValue(_nextName, valueString);
    }
    // END * UI ! those two methods are meant to be called in sequence from UnityEvent callers *END


    // from code just use static methods
    public static void SetValue(string nameString, float val, GameObject source =null)
    {
        SetValue(nameString, val.ToString(),source);
    }
    public static void SetValue(string nameString, int val, GameObject source =null)
    {
        SetValue(nameString, val.ToString(),source);
    }
    public static void SetValue(string nameString, string valueString, GameObject source =null)
    {
       
        string val = "";
        if (fields.TryGetValue(nameString, out val))
        {
        //    Debug.Log("settings field '" + nameString + "' was found having value of '" + fields[nameString] + "' new value has been set '" + valueString+"'",source);
            fields[nameString] = valueString;
        }
        else
        {
        //    Debug.Log("settings field '" + nameString + "' was added with value of '" + valueString+"'",source);
            fields.Add(nameString, valueString);
            instance.optionNames.Add(nameString);
			if (!instance.optionValues.Contains(valueString))
            	instance.optionValues.Add(valueString);
        }
        if (settingsUpdated!=null) settingsUpdated.Invoke();
        instance.recordCount = fields.Count;
    }


    /// <summary>
    /// Gets the stored value, or 'not set'
    /// </summary>
    public static string GetValue(string nameString)
    {
        string val = "not set";

		if (string.IsNullOrEmpty(nameString)) { Debug.Log(" null string requested "); return val; }
        if (fields==null) return null;
        if (fields.TryGetValue(nameString, out val))
        {
            return val;
        }
        else
        {
            Debug.Log(" value '"+nameString+"' has not been set");
        }
        return val;
    }
    /// <summary>
    /// Gets the stored value parsed to int, or -1
    /// </summary>
    public static int GetIntValue(string nameString)
    {
		if (string.IsNullOrEmpty(nameString)) { Debug.Log(" null name string requested "); return -1;}
        string val = GetValue(nameString);
        int result = -1;
        if (string.IsNullOrEmpty(val)) return result;
        if (!Int32.TryParse(val, out result))
        {
            Debug.LogError(" parsing failed of string " + val);
        }
        return result;
    }
    /// <summary>
    /// Gets the stored value parsed to float, or -1
    /// </summary>
    /// 
    /// 
    public static float GetFloatValue(string nameString)
    {   if (string.IsNullOrEmpty(nameString)) { Debug.Log(" null name string requested "); return -1;}
        string val = GetValue(nameString);
        float result = -1;
		if (string.IsNullOrEmpty(val)) return result;
        if (string.IsNullOrEmpty(nameString)) return result;
        if (!Single.TryParse(val, out result))
        {
            Debug.LogError(" parsing failed of string " + val);
        }
        return result;
    }
public void ResetPreferences()
    {
        PlayerPrefs.DeleteAll();
        
    }

}
