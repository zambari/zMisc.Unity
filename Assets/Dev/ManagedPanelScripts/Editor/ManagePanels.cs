using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
#if  EditorCoroutine  
//using Swing.Editor;
#endif
namespace zUI
{

    public class ManagePanels : EditorWindow
    {
        [SerializeField]
        List<Image> createdImages;
        const float panelAlpha = 0.3f;
        const float childSmallerByFactor = 0.8f;

        [MenuItem("Tools/Open Panel Manager window")]
        static void Init()
        {

#pragma warning disable 219
            ManagePanels window =
                (ManagePanels)EditorWindow.GetWindow(typeof(ManagePanels));
            if (window.createdImages == null) window.createdImages = new List<Image>();
            GameObject tempObject = GameObject.Find("__temp");
            if (tempObject != null)
                DestroyImmediate(tempObject);
        }
        [SerializeField]
        List<GameObject> objects;

        void OnHierarchyChange()
        {
            Repaint();
        }
        LayoutHelper lastSelected;


        [SerializeField]
        GameObject tempObject;
        Image flashImage;
        Color flashColor = Color.red * 0.3f;
        float lastSpacing;
//        bool coroutineRunning;
        bool restartCoroutine;
        bool stopCoroutine;
               GameObject createTempObject()
        {
            if (tempObject == null)
                tempObject = GameObject.Find("__temp");
            if (tempObject != null)
                DestroyImmediate(tempObject);

            tempObject = new GameObject("__temp");
            
            tempObject.hideFlags=HideFlags.HideInHierarchy;
            //  tempObject.hideFlags=HideFlags.HideInHierarchy;
            RectTransform r = tempObject.AddComponent<RectTransform>();
            //r.StretchToParent();
            
            LayoutElement el=tempObject.AddComponent<LayoutElement>();
            el.ignoreLayout=true;
            flashImage = tempObject.AddComponent<Image>();
            flashImage.color = flashColor;
            flashImage.raycastTarget = false;
            return tempObject;
        }
        
        IEnumerator selectionFlashRoutine(GameObject target)
        {
          //  coroutineRunning=true;
            stopCoroutine=false;
            if (flashImage == null) createTempObject();
            int numSteps = 20;
            if (flashImage == null) createTempObject();
         tempObject.SetActive(true);
             flashImage.enabled = true;
             flashImage.color=flashColor;
            if (target.transform.parent!=null)
            {            tempObject.transform.SetParent(target.transform.parent);
                           tempObject.StretchToParent();
                     tempObject.transform.SetAsLastSibling();
                for (int i=0;i<4;i++)
                {
tempObject.SetActive(true);
yield return null;
tempObject.SetActive(false);
yield return null;
                }
            }
        tempObject.SetActive(true);



            tempObject.transform.SetParent(target.transform);

            tempObject.transform.SetAsLastSibling();
   
            tempObject.StretchToParent();
            flashImage.enabled = true;

            float step = 1f / numSteps;
            float a = 1;
            GameObject t2=new GameObject();
            t2.hideFlags=HideFlags.HideInHierarchy;
            for (int j = 0; j < numSteps; j++)
            {
                a -= step;
                //flashImage.enabled=true;
                flashImage.color = flashColor.alpha(a);
                for (int i = 0; i < 10; i++)
                {
                
                    if (stopCoroutine)
                    {
                        yield break;
                    }
                    yield return null;

                }   
                t2.SetActive(true);
                t2.SetActive(false);
            
                //   for (int i = 0; i < 10; i++)
                //  {    yield return null;

                //  }
            }
        DestroyImmediate(t2);
            flashImage.enabled = false;

            tempObject.SetActive(false);
            
          //  coroutineRunning=false;
            yield return null;

        }

        void OnSelectionChange()
        {
stopCoroutine=true;
            if (lastSelected != null)
            {
                lastSelected.IsEdited(false); lastSelected = null;
            };
            if (Selection.activeGameObject != null)
            {
                LayoutHelper layoutHelper = Selection.activeGameObject.GetComponent<LayoutHelper>();
                if (layoutHelper != null)
                {
#if  EditorCoroutine                    
 //                  EditorCoroutine.start(selectionFlashRoutine((Selection.activeGameObject)));
#endif                    

                    layoutHelper.IsEdited(true);
                    lastSelected = layoutHelper;
                }
                else
                if (tempObject!=null)
                    DestroyImmediate(tempObject);

            }
            Repaint();
        }
        //        RectTransform cachedTarget;
        Transform canvasTransform;

        bool searchInSelectionAndParents()
        {
            if (Selection.activeGameObject == null) return false;
            Canvas canvasInParent;
            canvasInParent = Selection.activeGameObject.GetComponentInParent<Canvas>();
            if (canvasInParent != null)
            {
                canvasTransform = canvasInParent.transform;
                return true;
            }
            return false;

        }

        bool searchByName()
        {
            GameObject canvasObject = GameObject.Find("Canvas");
            if (canvasObject == null) return false;
            canvasTransform = canvasObject.transform;
            return true;
        }
        GameObject createCanvas()
        {
            if (searchByName())
                return canvasTransform.gameObject;

            GameObject canvas = new GameObject("Canvas");
            RectTransform rect = canvas.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector2(1920, 1080);
            Canvas c = canvas.AddComponent<Canvas>();
            c.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<CanvasScaler>();
            canvas.AddComponent<GraphicRaycaster>();

            canvasTransform = canvas.transform;
            return canvas;
        }
        void getCanvas()
        {
            if (!searchInSelectionAndParents())
            {
                Debug.Log("not found in parent");

                if (!searchByName())
                {
                    Debug.Log("not found via search");
                    createCanvas();
                }

            }
        }

        void setLayoutState(LayoutElement layout, bool setState = true)
        {
            LayoutElementState currentState = LayoutElementState.noLayoutElement;
            if (layout != null) currentState = layout.getState();
            LayoutElementState nextState = (LayoutElementState)EditorGUILayout.EnumPopup("LayoutElement State", currentState);
            if (nextState != currentState)
            {
                if (layout == null && nextState != LayoutElementState.noLayoutElement) layout = Selection.activeGameObject.AddComponent<LayoutHelper>();
                if (setState)
                    layout.setState(nextState);
            }
        }

        void setLayoutOrientation(LayoutHelper layout)
        {
            if (layout == null) return;
            Orientation currentOrientation = layout.orientation;
            Orientation nextOrientation = (Orientation)EditorGUILayout.EnumPopup("Orientation ", currentOrientation);
            if (nextOrientation != currentOrientation)
            {
                layout.orientation = nextOrientation;
            }
        }
        void displayDiagInfo()
        {
            if (Selection.activeGameObject == null) return;

            LayoutElement layout = Selection.activeGameObject.GetComponent<LayoutElement>();
            LayoutHelper layoutHelper = Selection.activeGameObject.GetComponent<LayoutHelper>();

            if (layout == null)
            {
                GUILayout.Label("No Auto Layout ");
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("AddLayoutHelper"))
                {
                    Selection.activeGameObject.AddComponent<LayoutHelper>();

                }
                if (GUILayout.Button("Add basic LayoutElement"))
                {
                    Selection.activeGameObject.AddComponent<LayoutElement>();

                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (layoutHelper == null)
                {
                    GUILayout.Label("basic Auto Layout ");
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("upgrade to LayoutHelper"))
                    {
                        layout.DestroySmart();
                        EditorApplication.delayCall += () => Selection.activeGameObject.AddComponent<LayoutHelper>();
                        return;
                    }
                    if (GUILayout.Button("downgrade to no auto layout"))

                    {
                        layout.removeAutoLayout();
                        return;
                    }


                    EditorGUILayout.EndHorizontal();
                }
                else
                {

                    GUILayout.Label("Great, you are  using a layouthelper here ");

                    if (GUILayout.Button("downgrade to standard element"))
                    {
                        layout.DestroySmart();
                        EditorApplication.delayCall += () => Selection.activeGameObject.AddComponent<LayoutElement>();
                        return;
                    }
                }

            }
            setLayoutState(layout);
            setLayoutOrientation(layoutHelper);
            /*   LayoutHelper layoutHelper = GetLayoutHelper();
               if (layoutHelper==null)
               { if (GUILayout.Button("Add LayoutHelper"))
               {
                Selection.activeGameObject.AddComponent<LayoutHelper>();

               }}
               else {
                           GUILayout.Space(10);
                           GUILayout.Label("Layout Helper Info Layout element :");

                           GUILayout.Label("Current level= :" + layoutHelper.level);
                           if (GUILayout.Button("RemoveHelper")){
                               DestroyImmediate(layoutHelper);

                           }
                           GUILayout.Space(10);

               } else 



                           bool hasLayout=layout!=null;
                           bool shouldHaveLayout=GUILayout.Toggle(hasLayout,"Has Layout Element");
                           if (hasLayout!=shouldHaveLayout)
                           {
                                   if (shouldHaveLayout)
                                   {
                                       Selection.activeGameObject.AddComponent<LayoutElement>();
                                   } else
                                   DestroyImmediate(layout);

                           }
                                           GUILayout.Label("------------");
                           if (layout!=null)
                           {

                           if (layout.ignoreLayout)
                           GUILayout.Label("Current level= :" + layoutHelper.level);
                           else 
                           GUILayout.Label("flex x="+(layout.flexibleWidth==1?"YES":layout.flexibleWidth.ToStringShort())+" flex y="+(layout.flexibleHeight==1?"YES":layout.flexibleWidth.ToShortString()));
                           if (layout.preferredHeight!=0 || layout.preferredWidth!=0)
                           GUILayout.Label("pref x="+layout.preferredWidth+" pref y="+layout.preferredHeight);
                           }
                           else 
                           {

                           }*/

        }

        void CreateLayout(Orientation orientation)
        {


            /* if (Selection.activeObject != null)
                 {
                     if (!objects.Contains(Selection.activeGameObject))
                         objects.Add(Selection.activeGameObject);
                     else
                     {
                         objects.Remove(Selection.activeGameObject);
                         objects.Insert(0, Selection.activeGameObject);
                     }
                 }*/

        }
        [SerializeField]
        int nextPanelIndex;
        bool showingImages = true;
        bool lastShowingImages = true;
        bool selectionIsCanvas()

        {
            if (Selection.activeGameObject == null) return false;
            return (Selection.activeGameObject.GetComponent<Canvas>() != null);
        }

        void setFlexible(LayoutElement l)
        {
            l.ignoreLayout = false;
            l.flexibleHeight = 1;
            l.flexibleWidth = 1;

        }
        void setIgnore(LayoutElement l)
        {
            l.ignoreLayout = true;
        }
        void CreatePanel(GameObject target, bool asSsubPanel)
        {
            if (canvasTransform == null) createCanvas();
            if (target == null) return;
            int targetIndex = target.transform.GetSiblingIndex();

            GameObject go = new GameObject("Panel_" + nextPanelIndex);
             Undo.RegisterCreatedObjectUndo(go,"panel");
            nextPanelIndex++;
            RectTransform rectTransform = go.AddComponent<RectTransform>();
            rectTransform.sizeDelta = Vector2.one * 300 + Vector2.zero.Randomize(200);

            LayoutHelper thisLayout = go.AddComponent<LayoutHelper>();
            if (asSsubPanel) setFlexible(thisLayout);
            else
                setIgnore(thisLayout);

            Image image = go.AddComponent<Image>();
            image.color = Random.ColorHSV(0, 1);
            image.color = image.color.alpha(panelAlpha);
            createdImages.Add(image);
            if (asSsubPanel || selectionIsCanvas())
            {
                go.transform.SetParent(target.transform);
                go.transform.localPosition = Vector2.zero;
                if (stretch) go.StretchToParent();
                if (asSsubPanel)
                {
                    Image parentImage = target.GetComponent<Image>();
                    if (parentImage != null)
                        image.color = parentImage.color.randomize();
                    rectTransform.sizeDelta = rectTransform.parent.gameObject.GetComponent<RectTransform>().sizeDelta * childSmallerByFactor;
                }
            }
            else
            {
                if (Selection.activeGameObject != null)
                {
                    if (Selection.activeGameObject.hasCanvasParent())
                    {

                        go.transform.SetAsSiblingTo(Selection.activeGameObject);
                        go.transform.position = Selection.activeGameObject.transform.position;
                    }
                    else Debug.Log("seletion has no canvas parent");

                }
                else
                {
                    if (canvasTransform != null)
                    {
                        go.transform.SetParent(canvasTransform);
                        go.transform.position = go.transform.position.RandomizeXY(300);

                    }
                    else Debug.Log("layout oops");
                }
            }
            Selection.activeGameObject = go;
        }


        bool isSomethingValidSelected()
        {
            if (Selection.activeGameObject == null) return false;
            if (Selection.activeGameObject.GetComponentInParent<Canvas>() == null) return false; else return true;

        }
        bool stretch;
        bool selectionHasLayoutHelper()
        {
            if (Selection.activeGameObject == null) return false;

            return Selection.activeGameObject.GetComponent<LayoutHelper>() != null;


        }

        LayoutHelper GetLayoutHelper()
        {
            return Selection.activeGameObject.GetComponent<LayoutHelper>();

        }
        void OnGUI()
        {
            GUILayout.Space(10);
            GUILayout.Label(" Managed Panel Toolkit alpha");
            GUILayout.Space(10);
            if (Selection.objects.Length > 1)
            {
                GUILayout.Label(" mutliple objects selected ");
                GUILayout.Label(" Not supported yet");
                return;
            }
            if (!isSomethingValidSelected())
            {
                if (canvasTransform == null)
                {
                    if (GUILayout.Button("Create Canvas")) //, GUIStyle.none
                        Selection.activeGameObject = createCanvas();
                }
                else
                if (GUILayout.Button("Select Canvas")) //, GUIStyle.none
                    Selection.activeGameObject = canvasTransform.gameObject;
                return;

            }
            GameObject selectedObject = Selection.activeGameObject;
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Create New Panel")) //, GUIStyle.none
            {
                CreatePanel(selectedObject, false);
                return;
            }

            if (GUILayout.Button("Create Sub Panel")) //, GUIStyle.none
            {
                CreatePanel(selectedObject, true);
                return;
            }
            if (GUILayout.Button("5x Sub Panel")) //, GUIStyle.none
            {
                    
                
                for (int i = 0; i < 5; i++)
                {
                    CreatePanel(selectedObject, true);
                }
                return;
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);


            EditorGUILayout.BeginHorizontal();

            stretch = (GUILayout.Toggle(stretch, "Stretch new to fit"));
            if (Selection.activeGameObject != null && Selection.activeGameObject.hasCanvasParent())
            {
                if (GUILayout.Button("Stretch current")) //, GUIStyle.none

                    selectedObject.StretchToParent();
                if (GUILayout.Button("Almost Stretch")) //, GUIStyle.none

                    selectedObject.StretchToParent(0.05f);
            }
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(10);
            showingImages = (GUILayout.Toggle(showingImages, "Enable Recent Panel Images"));
            if (showingImages != lastShowingImages)
            {
                lastShowingImages = showingImages;
                for (int i = 0; i < createdImages.Count; i++)
                {
                    if (createdImages[i] != null)
                        createdImages[i].enabled = showingImages;
                }
                Debug.Log("toggled " + createdImages.Count + " images ");
            }
            if (!selectionHasLayoutHelper())
            {

                EditorGUILayout.BeginHorizontal();

                if (GUILayout.Button("Create Horizontal Layout")) //, GUIStyle.none
                {
                    CreateLayout(Orientation.Horizontal);
                    return;
                }

                if (GUILayout.Button("Create Vertical Layout")) //, GUIStyle.none
                {
                    CreateLayout(Orientation.Vertical);

                }
                //  GUILayout.FlexibleSpace();


                EditorGUILayout.EndHorizontal();
            }
            else  // HAS laoutHelper, we know its there
            {



            }
            GUILayout.Space(100);
            displayDiagInfo();

            GUILayout.Space(10);
            lastSpacing=EditorGUILayout.Slider("Spacing",lastSpacing,0,20);
            if (lastSpacing!=LayoutHelper.spacing)
            {
               LayoutHelper.spacing= lastSpacing;


                if (LayoutHelper.distanceChange!=null) LayoutHelper.distanceChange.Invoke(); else Debug.Log("noe");
                if (tempObject==null) createTempObject();
                tempObject.SetActive(false);
                tempObject.SetActive(true);
            }

        }

    }

}