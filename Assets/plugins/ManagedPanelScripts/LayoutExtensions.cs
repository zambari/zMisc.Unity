using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zUI;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace zUI
{
    public enum LayoutElementState { noLayoutElement, layoutPresentIgnoring, layoutPresentStretch, layoutStrechHorizontal, lyoutStretchVertical, layoutNotFlexible, layoutDenenerate }



    public static class MoreLayoutExt
    {

        public static LayoutElementState getState(this LayoutElement layoutElement)
        {
            if (layoutElement == null) return LayoutElementState.noLayoutElement;
            if (layoutElement.ignoreLayout) return LayoutElementState.layoutPresentIgnoring;
            if (layoutElement.flexibleHeight > 0 && layoutElement.flexibleHeight > 0) return LayoutElementState.layoutPresentStretch;

            if ((layoutElement.preferredHeight > 0 || layoutElement.minHeight > 0) &&
            (layoutElement.preferredWidth > 0 || layoutElement.minWidth > 0)) return LayoutElementState.layoutNotFlexible;
            return LayoutElementState.layoutDenenerate;

        }

        public static void setState(this LayoutElement layoutElement, LayoutElementState state)
        {
            if (layoutElement == null) return;
            switch (state)
            {
                case LayoutElementState.noLayoutElement:
                    layoutElement.DestroySmart();
                    break;
                case LayoutElementState.layoutPresentIgnoring:
                    layoutElement.ignoreLayout = true;
                    break;
                case LayoutElementState.layoutPresentStretch:
                    layoutElement.ignoreLayout = false;
                    layoutElement.flexibleHeight = 1;
                    layoutElement.flexibleWidth = 1;

                    break;

                case LayoutElementState.layoutStrechHorizontal:
                    if (layoutElement.ignoreLayout)
                    {
                        layoutElement.ignoreLayout = false;
                       
                    }
                     layoutElement.fillPreferredHeight();
                    layoutElement.flexibleHeight = -1;
                    layoutElement.flexibleWidth = 1;
                    break;
                case LayoutElementState.lyoutStretchVertical:
                    if (layoutElement.ignoreLayout)
                    {
                        layoutElement.ignoreLayout = false;
                      
                    }
                      layoutElement.fillPreferredWidth();
                    layoutElement.flexibleHeight = 1;
                    layoutElement.flexibleWidth = -1;
                    break;
                case LayoutElementState.layoutNotFlexible:
                    layoutElement.ignoreLayout = false;
                    layoutElement.fillPreferred();
                    layoutElement.flexibleHeight = -1;
                    layoutElement.flexibleWidth = -1;

                    break;
            }


        }

    } // more layout class
    public static class LayoutExtensions
    {

const float defaultSize=100;

        public static void fillPreferredHeight(this LayoutElement l,bool resetIfDegenerate=true)
        {
            if (l == null) return;
            float currentHeight = l.transform.getHeight();
            if (currentHeight <= 0) currentHeight = defaultSize;
            l.preferredHeight = currentHeight;


        }
        public static void fillPreferredWidth(this LayoutElement l,bool resetIfDegenerate=true)
        {
            if (l == null) return;
            float currentWidth = l.transform.getWidth();
            if (currentWidth <= 0) currentWidth = defaultSize;
            l.preferredWidth = currentWidth;


        }

        public static void fillPreferred(this LayoutElement l,bool resetIfDegenerate=true)
        {

            l.fillPreferredHeight(resetIfDegenerate);
            l.fillPreferredWidth(resetIfDegenerate);


        }
        public static int getActiveElementCount(this VerticalLayoutGroup layout)
        {
            int count = 0;
            if (layout == null) return count;
            for (int i = 0; i < layout.transform.childCount; i++)
            {
                GameObject thisChild = layout.transform.GetChild(i).gameObject;
                if (thisChild != null)
                {
                    LayoutElement le = thisChild.GetComponent<LayoutElement>();
                    if (le != null)
                    {
                        if (!le.ignoreLayout) count++;
                    }
                }

            }

            return count;
        }


        public static LayoutElement getLayout(RectTransform rect)
        {
            LayoutElement le = null;
            LayoutGroup lg = rect.GetComponent<LayoutGroup>();
            if (lg != null)
            {
                le = rect.gameObject.AddComponent<LayoutElement>();

                le.flexibleHeight = 1;
                le.flexibleWidth = 1;
            }
            return le;
        }

        public static void removeAutoLayout(this Component c)
        {
            ContentSizeFitter contentSizeFitter = c.GetComponent<ContentSizeFitter>();
            if (contentSizeFitter != null) contentSizeFitter.DestroySmart();
            LayoutElement le = c.GetComponent<LayoutElement>();
            if (le != null) le.DestroySmart();
            LayoutHelper lh = c.GetComponent<LayoutHelper>();
            if (lh != null) lh.DestroySmart();
            VerticalLayoutGroup v = c.GetComponent<VerticalLayoutGroup>();
            if (v != null) v.DestroySmart();
            HorizontalLayoutGroup h = c.GetComponent<HorizontalLayoutGroup>();
            if (h != null) h.DestroySmart();


        }

        public static float getWidth(this RectTransform r)
        {
            return r.rect.width;
        }
        public static float getHeight(this RectTransform r)
        {
            return r.rect.height;
        }
        public static float getWidth(this Transform t)
        {
            RectTransform r = t.GetComponent<RectTransform>();
            if (r != null)
                return r.rect.width;
            else return -1;
        }
        public static float getHeight(this Transform t)
        {
            RectTransform r = t.GetComponent<RectTransform>();
            if (r != null)
                return r.rect.height;
            else return -1;
        }
        public static float getWidth(this GameObject t)
        {
            RectTransform r = t.GetComponent<RectTransform>();
            if (r != null)
                return r.rect.width;
            else return -1;
        }
        public static float getHeight(this GameObject t)
        {
            RectTransform r = t.GetComponent<RectTransform>();
            if (r != null)
                return r.rect.height;
            else return -1;
        }
        public static void setChildControl(this HorizontalLayoutGroup layout, float spacing = 0)

        {
            if (layout == null) return;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.spacing = spacing;
        }

        public static void setChildControl(this VerticalLayoutGroup layout, float spacing = 0)

        {
            if (layout == null) return;
            layout.childForceExpandHeight = false;
            layout.childForceExpandWidth = false;
            layout.childControlHeight = true;
            layout.childControlWidth = true;
            layout.spacing = spacing;
        }
        public static LayoutElement[] getActiveElements(this VerticalLayoutGroup layout)
        {
            List<LayoutElement> elements = new List<LayoutElement>();
            if (layout == null) return elements.ToArray();
            for (int i = 0; i < layout.transform.childCount; i++)
            {
                GameObject thisChild = layout.transform.GetChild(i).gameObject;
                LayoutElement le = thisChild.GetComponent<LayoutElement>();
                if (le != null && !le.ignoreLayout) elements.Add(le);
            }
            return elements.ToArray();
        }


        public static LayoutElement[] getActiveElements(this GameObject g)
        {
            List<LayoutElement> elements = new List<LayoutElement>();
            Debug.Log("seacrihg " + g.transform.childCount);
            for (int i = 0; i < g.transform.childCount; i++)
            {
                GameObject thisChild = g.transform.GetChild(i).gameObject;
                LayoutElement le = thisChild.GetComponent<LayoutElement>();
                if (le == null) Debug.Log(" NO LAYUT ELEMENT ON GAMEOBJECT " + thisChild.name, thisChild);
                else
                {
                    LayoutHelper lh = thisChild.GetComponent<LayoutHelper>();
                    if (lh != null) Debug.Log("lh present");
                    else
                if (!le.ignoreLayout)
                        elements.Add(le);
                }

            }


            return elements.ToArray();
        }
        public static Image AddImageChild(this GameObject g, float opacity = 0.3f)
        {
            Image image = g.AddComponent<Image>();
            image.color = new Color(Random.value * 0.3f + 0.7f,
                 Random.value * 0.3f + 0.7f,
             Random.value * 0.2f, opacity);


            image.name = "Image";
            Debug.Log("added image to " + g.name, g);
            return image;
        }


        public static Image AddImageChild(this RectTransform rect, float opacity = 0.3f)
        {
            return rect.gameObject.AddImageChild(opacity);
        }

        public static void AddLayoutElement(this GameObject go, bool ignore = true)
        {
            LayoutElement layoutElement = go.GetComponent<LayoutElement>();
            if (layoutElement == null) layoutElement = go.AddComponent<LayoutElement>();
            layoutElement.ignoreLayout = ignore;
            layoutElement.CollapseComponent();


        }
        public static void SetAsSiblingTo(this GameObject thisGameObject, GameObject target)
        {
            thisGameObject.transform.SetAsSiblingTo(target.transform);


        }
        public static void SetAsSiblingTo(this Transform thisTransform, GameObject target)
        {
            thisTransform.SetAsSiblingTo(target.transform);
        }
        public static void SetAsSiblingTo(this Transform thisTransform, Transform target)
        {
            int targetIndex = target.GetSiblingIndex();
            thisTransform.SetParent(target.parent);
            thisTransform.SetSiblingIndex(targetIndex + 1);


        }

        public static void StretchToParent(this GameObject go, float margin = 0)
        {
            RectTransform rect = go.GetComponent<RectTransform>();
            if (rect != null) rect.StretchToParent(margin);

        }
        public static void StretchToParent(this RectTransform rect, float margin = 0)
        {
            if (rect==null) {
                Debug.Log("no rect");
                return;
            }
            if (rect.parent==null)
            {
                Debug.Log("no parent");
                return;
            }
            RectTransform parentRect = rect.parent.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(margin, margin);
            rect.anchorMax = new Vector2(1 - margin, 1 - margin);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;


        }
        public static bool isCanvas(this GameObject g)
        {
            return g.GetComponent<Canvas>() != null;
        }
        public static bool hasCanvasParent(this GameObject g)
        {
            if (g.GetComponent<Canvas>() != null) return false;
            return (g.GetComponentInParent<Canvas>() != null);
        }

        public static bool hasVerticalLayout(this GameObject g)
        {
            return g.GetComponent<VerticalLayoutGroup>() != null;
        }

        public static bool hasHorizontalLayout(this GameObject g)
        {
            return g.GetComponent<HorizontalLayoutGroup>() != null;
        }


    }

}