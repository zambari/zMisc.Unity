using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/*

Title : ScreenNavigation
Part of  Toucan Universal Libraries
version 0.22    (2017.07)
0.21->0.22 timeout reset

Author by : zambari aka Filip Tomaszewski at http://toucan-systems.pl

Function overview:
A Set of three scrips designed for rapid prototyping of mulit sreen applications.
Features: works in the editor by ensuring only one screen is active at the time for editing. Marks active screen with an [X] in the hierarchy window.
Handles screne transitions (none / fade to color / sweep). Handles history so no need to manuall link your 'back' buttons. Optionally, escape key performs a Back() call. Sweep dynamics controlled via AnimationCurve
Does not interfere with standard Unity UI in any way - buttons can still be assgined the traditional way, nothing is lost.

ScreenButton - Added to a button, it requests a ScreenButton from its parent ScreenHub 
SubsScreen - marks gameobject that is used as a screen object - handles changing the name nad notifies screenhub when it becomes active
ScreenHub - containted in this file:
Component managing transitions, should be added to the parent component (i.e. canvas), holds the list of children screens and makes sure
only one is enabled.

To change the name of the screen change it within SubScreen inspector - it will take gameObejct name when first added, but will overwrite it later, 

Screen marked as 'default', if there is only one, will be set as active on Play enter.


Instrutions:
Add a Canvas and add ScreenHub to it. 
Add an empty panel, and add a SubScreen component to it. Open its inspector and type in main as your screen name.
Add an UI Button as its child, add a ScreenButton component to your button and place it somewhere in the corner, this will be your BACK button.
Duplicate your screen object a few times.
On your first screen, add a few more buttons with ScreenButton components on them. Drag and drop your other screens onto the respective buttons
Mark your main screen as 'default screen'
Your basic screen setup is ready.


To use multiple screen setups make sure no ScreenHub is a child of another ScreenHub (to be added later)
Scripting API:
Two useful methods used to trigger transitions:
           screenHub.ChangeToScreen( SubScreen targetScreen);
           screenHub.Back();

*/


[ExecuteInEditMode]
public class ScreenHub : MonoBehaviour
{

    [Range(0, 1.5f)]
    public float transitionDuration = 0.4f;
    public bool transitionSweep = true;
    public bool transitionFade = true;
    public Color transitionFadeColor = Color.white;
    static ScreenHub instance;
    private SubScreen defaultScreen;
    private SubScreen nextScreen;
    private SubScreen currentScreen;
    List<SubScreen> allScreens;
    List<SubScreen> screenHistory;
    const string activeMarker = "[X] ";
    const string inactiveMarker = "[ ] ";
    bool isInTransition;
    float fadeStartTime;
    public AnimationCurve transitionCurve;
    private AnimationCurve transitionUnavailableCurve;
    public bool escapeKeyGoesBack = true;
    public float sweepAmt=0.5f;
    bool backTransition;


    public float resetTimeOut = 3;
    float nextReset;

    /// <summary>
    /// Request a screen change. If back is set to true, the transition will go in reverse
    /// </summary>
    /// <param name="targetScreen"></param>
    /// <param name="back">is transition reversed</param>

    
    public void ChangeToScreen(SubScreen targetScreen, bool back = false)
    {

        if (isInTransition)
        {
            Debug.Log("screen change request while in transition");
            return;
        }
        if (targetScreen == null) { Back(); return; }
        if (currentScreen==null) { currentScreen=targetScreen; return; }
        checkLists();
        if (screenHistory.Count > 0 && screenHistory[screenHistory.Count - 1] == targetScreen)
        {
            // do nothing, this is our last active screen anyways
        }
        else
        {
            screenHistory.Add(targetScreen);
            nextScreen = targetScreen;
            if (isTransitionRequested())
            {

            //    if (transitionSweep)
                    StartCoroutine(SweepAndSwitch());
             //   else
             //       StartCoroutine(FadeAndSwitch());
            }
            else
            {
                SwitchActiveScreenToQueued();
                isInTransition = false;
            }
        }
    }

    /// <summary>
    /// Performs a transition to the screen open before this one
    /// </summary>
    public void Back()
    {
        if (screenHistory.Count > 1)
        {
            screenHistory.RemoveAt(screenHistory.Count - 1);
            SubScreen lastScreen = screenHistory[screenHistory.Count - 1];
            screenHistory.RemoveAt(screenHistory.Count - 1);
            backTransition = true;
            ChangeToScreen(lastScreen, true);
        }
        else
        {
            Debug.Log("ScreenNav: This is the first screen we shown, no more history");
            if (transitionSweep)
                StartCoroutine(SweepHint());
        }
    }
   /* IEnumerator FadeAndSwitch()
    {
        if (blockerImage != null && transitionDuration > 0 && instance.nextScreen != null)
        {
            blockerImage.enabled = true;
            fadeStartTime = Time.time;
            Color blockColor = new Color(blockerImage.color.r, blockerImage.color.g, blockerImage.color.b, 1);
            while (Time.time - fadeStartTime <= (transitionDuration / 2))
            {
                float fadeAmt = (Time.time - fadeStartTime) / (transitionDuration / 2);
                blockerImage.color = new Color(blockColor.r, blockColor.g, blockColor.b, (fadeAmt));
                yield return null;
            }
            SwitchActiveScreenToQueued();
            fadeStartTime = Time.time;
            while (Time.time - fadeStartTime <= (transitionDuration / 2))
            {
                float fadeAmt = (Time.time - fadeStartTime) / (transitionDuration / 2);
                blockerImage.color = new Color(blockColor.r, blockColor.g, blockColor.b, (1 - fadeAmt));
                yield return null;
            }
            blockerImage.enabled = false;
        }
        isInTransition = false;
        yield return null;
    }*/

    IEnumerator SweepAndSwitch()
    {
        if (transitionDuration > 0 && instance.nextScreen != null)
        {
            Transform tempObject = (new GameObject("sweeper")).transform;
            tempObject.SetParent(transform);
            tempObject.localPosition = new Vector3(0, 0, 0);
            // will need to restore that in a second
            Vector3 nextPosition = nextScreen.transform.position;
            Transform nextScreenParent = nextScreen.transform.parent;
            int currentSiblingIndex = 0;
            Vector3 lastPosition = Vector3.zero;
            Transform currentScreenParent = null;
            int nextSiblingIndex = nextScreen.transform.GetSiblingIndex();
            if (currentScreen != null)
            {
                currentSiblingIndex = currentScreen.transform.GetSiblingIndex();
                lastPosition = currentScreen.transform.position;
                currentScreenParent = currentScreen.transform.parent;
            }
            nextScreen.transform.SetParent(tempObject);
            tempObject.localPosition = new Vector3(Screen.width * sweepAmt * (backTransition ? -1 : 1), 0, 0);
            if (currentScreen != null) currentScreen.transform.SetParent(tempObject);
            nextScreen.gameObject.SetActive(true);
            fadeStartTime = Time.time;

            CanvasGroup currentCanvasGroup = null;
            if (currentScreen != null) currentCanvasGroup = currentScreen.GetComponent<CanvasGroup>();
            CanvasGroup nextCanvasGroup = nextScreen.GetComponent<CanvasGroup>();


            while (Time.time - fadeStartTime <= transitionDuration)
            {
                float rawAmt = (Time.time - fadeStartTime) / (transitionDuration);
                float amt = 1 - transitionCurve.Evaluate(rawAmt);
                tempObject.localPosition = sweepPos(amt);
                if (transitionFade)
                {
                    if (currentCanvasGroup != null)
                        currentCanvasGroup.alpha =amt;
                    if (nextCanvasGroup != null)
                        nextCanvasGroup.alpha = 1-amt;
                }
                //    Debug.Log(tempObject.localPosition);
                yield return null;
            }
            nextScreen.transform.SetParent(nextScreenParent);
            if (currentScreen != null)
            {
                currentScreen.transform.SetParent(currentScreenParent);
                currentScreen.transform.position = lastPosition;
                currentScreen.transform.SetSiblingIndex(currentSiblingIndex);
            }
            nextScreen.transform.position = nextPosition;
            Destroy(tempObject.gameObject);
            nextScreen.transform.SetSiblingIndex(nextSiblingIndex);
            SwitchActiveScreenToQueued();
        }
        backTransition = false;
        isInTransition = false;
        yield return null;
    }

    Vector3 sweepPos(float amt)
    {
        return new Vector3((amt) * sweepAmt * Screen.width * (backTransition ? -1 : 1), 0, 0);
    }

    /// <summary>
    /// This function moves current screen back and forth to flag its unsuitabiltiy to move
    /// </summary>
    /// <returns></returns>
    IEnumerator SweepHint()
    {
        if (currentScreen != null)
        {
            Transform currentScreenparent = currentScreen.transform.parent;
            Transform tempObject = (new GameObject("sweeper")).transform;
            tempObject.SetParent(transform);
            tempObject.localPosition = new Vector3(0, 0, 0);
            currentScreen.transform.SetParent(tempObject);
            fadeStartTime = Time.time;
            while (Time.time - fadeStartTime <= transitionDuration)
            {
                float amt = transitionUnavailableCurve.Evaluate((Time.time - fadeStartTime) / (transitionDuration));
                tempObject.localPosition = sweepPos(amt);
                yield return null;
            }
            tempObject.localPosition = new Vector3(0, 0, 0);
            currentScreen.transform.SetParent(currentScreenparent);
            Destroy(tempObject.gameObject);
            backTransition = false;
            isInTransition = false;
        }
        yield return null;
    }

    /// <summary>
    /// Called by subscreen, requesting to be added to the list of screens in this hub
    /// </summary>
    /// <param name="thisScreen"></param>
    public void RegisterScreen(SubScreen thisScreen)
    {
        if (instance == null) return;
        instance.checkLists();

        if (!instance.allScreens.Contains(thisScreen)) instance.allScreens.Add(thisScreen);
        if (thisScreen.isDefaultScreen)
            if (instance.defaultScreen == null) instance.defaultScreen = thisScreen;
            else
             if (instance.defaultScreen != thisScreen)
                Debug.LogWarning("multiple screens set as default - " + instance.defaultScreen.name + " and " + thisScreen.name, thisScreen);

    }

    /// <summary>
    /// Check conditions for doing the fade (i.e. don't do it if we're int he editor)
    /// </summary>

    bool isTransitionRequested()
    {
        // return true;
        if (transitionDuration != 0 &&
        Application.isPlaying
         && (transitionFade || transitionSweep) &&
          //&& screenHistory.Count >= 1 &&
          !isInTransition && Time.time > 1)
            return true;
        else return false;
    }

    /// <summary>
    /// Does the actual switch
    /// </summary>
    void SwitchActiveScreenToQueued()
    {
        if (instance == null) return;
        checkLists();

        for (int i = 0; i < instance.allScreens.Count; i++)
        {
            if (instance.allScreens[i] != null && instance.allScreens[i] != instance.nextScreen)
            {
                instance.allScreens[i].gameObject.SetActive(false);
            }
        }
        currentScreen = nextScreen;
        if (nextScreen != null)
            nextScreen.gameObject.SetActive(true);
    }


    /// <summary>
    /// UtilityMethod used for collapsing components in Editor
    /// </summary>

    public static void CollapseComponent(Component c)
    {
#if UNITY_EDITOR
        if (c != null)
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(c, false);
#endif
    }

    /// <summary>
    /// Force a selection refresh so that the editor will refresh the folds (use after CollapseComponent)
    /// </summary>

    public static void RefreshSelection(Component c)
    {
#if UNITY_EDITOR
        if (c != null)
        {
            if (Selection.activeGameObject == c.gameObject)
            {
                Selection.activeGameObject = null;
                Selection.activeGameObject = c.gameObject;
            }
        }
#endif
    }
    /* void CreateBlockerImage()
     {
         if (blockerImage == null)
             if (transitionFade && Application.isPlaying)
             {
                 GameObject fader = new GameObject("FadeImage");
                 RectTransform fadeRect = fader.AddComponent<RectTransform>();
                 fader.transform.SetParent(transform);
                 fadeRect.anchorMin = Vector2.zero;
                 fadeRect.anchorMax = Vector2.one;
                 fadeRect.anchoredPosition = Vector2.zero;
                 fadeRect.sizeDelta = Vector2.zero;
                 //  fadeRect.offsetMin=Vector2.zero;
                 //  fadeRect.offsetMax=Vector2.one;

                 blockerImage = fader.AddComponent<Image>();
                 blockerImage.color = transitionFadeColor;
                 blockerImage.enabled = false;
             }

     }*/


    void ActivateDefaultScreen()
    {
        for (int i = 0; i < allScreens.Count; i++)
            if (allScreens[i].isDefaultScreen)
            {
                ChangeToScreen(allScreens[i]);
                return;
            }
    }

    private void checkLists()
    {
        if (allScreens == null)
        {

            allScreens = new List<SubScreen>();
            SubScreen[] subscreens = GetComponentsInChildren<SubScreen>(true);
            for (int i = 0; i < subscreens.Length; i++) RegisterScreen(subscreens[i]);
        }
        if (screenHistory == null) screenHistory = new List<SubScreen>();
    }
    #region stringHelpers

    /// <summary>
    /// Checks if string begins with a marker symbol defined in ScrenHub
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static bool hasMarker(string s)
    {
        if (s.StartsWith(activeMarker) || s.StartsWith(inactiveMarker)) return true;
        //if (s.Length < 3) return false;
        //if (s[0] == '[' && s[2] == ']') return true;
        return false;
    }


    /// <summary>
    /// Gives back the name without the starting [X] or [ ], actually just crops first 3 characters
    /// </summary>
    /// 
    public static string RemoveMarker(string s)
    {
        if (hasMarker(s))
            s = s.Substring(4);
        return s;
    }

    /// <summary>
    ///  String helper function, adds [X] or [ ] to the name 
    /// </summary>
    /// 
    public static string ActiveName(string baseName, bool markActive)
    {
        baseName = RemoveMarker(baseName);
        if (markActive) return activeMarker + baseName;
        else return inactiveMarker + baseName;
    }
    #endregion
    #region mono

    void OnEnable()
    {
        if (instance == null) instance = this;
        if (transitionCurve == null) transitionCurve = new AnimationCurve();
        if (transitionCurve.keys.Length < 1)
        {
            transitionCurve.AddKey(new Keyframe(0f, 0f, 0f, 0f));
            transitionCurve.AddKey(new Keyframe(0.1510584f, -0.01459254f, 0.2446519f, 0.2446519f));  // nice curve with a bit overshoot
            transitionCurve.AddKey(new Keyframe(0.6344747f, 0.955584f, 0.7718801f, 0.7718801f));
            transitionCurve.AddKey(new Keyframe(1f, 1f, 0f, 0f));
        }
        transitionUnavailableCurve = new AnimationCurve();
        transitionUnavailableCurve.AddKey(new Keyframe(0f, 0f, -0.04362153f, -0.04362153f));
        transitionUnavailableCurve.AddKey(new Keyframe(0.1264497f, -0.005515931f, -0.007406997f, -0.007406997f)); // na-nah curve
        transitionUnavailableCurve.AddKey(new Keyframe(0.4200125f, 0.002940889f, 0.003742002f, 0.003742002f));
        transitionUnavailableCurve.AddKey(new Keyframe(0.6871244f, -0.002754879f, -0.00625925f, -0.00625925f));
        transitionUnavailableCurve.AddKey(new Keyframe(1f, 0f, 0.00880503f, 0.00880503f));
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
        checkLists();
    }

    void OnValidate()
    {
        //    if (transitionSweep) transitionFade = false;
        //   if (transitionFade) transitionSweep = false;
        //   if (transitionFade) CreateBlockerImage();
    }
    void Reset()
    {
#if UNITY_EDITOR

        CollapseComponent(GetComponent<CanvasScaler>());
        CollapseComponent(GetComponent<EventSystem>());
        CollapseComponent(GetComponent<StandaloneInputModule>());
        CollapseComponent(GetComponent<GraphicRaycaster>());
        CollapseComponent(GetComponent<Canvas>());
        CollapseComponent(GetComponent<RectTransform>());
        CollapseComponent(GetComponent<Camera>());
        Selection.activeGameObject = this.gameObject;
#endif
    }

    void Update()
    {
        if (escapeKeyGoesBack && Input.GetKeyUp(KeyCode.Escape))
            Back();
     if (Input.anyKey || Input.GetMouseButton(0))
     {
         nextReset=Time.time+resetTimeOut;
     }
     if (Time.time>nextReset)
     {
         ActivateDefaultScreen();
         nextReset=Time.time+resetTimeOut;
     }
    }

    void Start()
    {
        screenHistory = new List<SubScreen>(); ;
        nextReset=Time.time+resetTimeOut;
        checkLists();
        //  CreateBlockerImage();
        ActivateDefaultScreen();

    }
    #endregion
}
