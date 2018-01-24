using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Z;
namespace Z
{
    public class LogMessage
    {
        public LogMessage(string t)
        { messageText = t; }
        public enum TypeOfMessage { info, hp, ap, turn }
        public string messageText;
        public TypeOfMessage messageType;

    }


    public class zLogRow : zNode
    {
        string valueCache;
        public bool traceStack;
        public string stackTrace;
        public float initialFlashTime = .5f;
        public float fadeAfter = 2;
        public float destroyAfter = 5;
        public Color textColor = Color.gray;
        public Color textColorFlash = Color.red;
        public Color rowColor = Color.gray;
        // public Color rowColorFlash = Color.red;
        public zLog log;
        //    Coroutine fade;
        float startTime;
        public bool keepInPool = true;
        protected  void OnValidate()
        {
            if (!enabled || !gameObject.activeInHierarchy) return;
                text.color = textColor;
            if (image != null) image.color = rowColor;
            adjustSizeToFitText();
        }

        public void setAlternativeMessage(string message)

        {
            stackTrace = message;

        }

        public void printMessage(LogMessage message)
        {
            isDisabled = false;
            gameObject.SetActive(true);
            //      if (fade != null)
            //       StopCoroutine(fade);
            text.color = textColor;
            startTime = Time.time;
            text.text = message.messageText;
            transform.SetAsLastSibling();
            //fade = StartCoroutine(fadeRow());
            adjustSizeToFitText();

            //      if (controller!=null)
            //        controller.setScrollStateDirty();
        }

        IEnumerator fadeRow()
        {
            float r;
            while (Time.time < startTime + initialFlashTime)
            {
                r = (Time.time - startTime) / initialFlashTime;
                text.color = Color.Lerp(textColorFlash, textColor, r);
                //   image.color = Color.Lerp(rowColorFlash, rowColor, r);
                yield return null;
            }

            float startFade = Time.time + fadeAfter;
            while (Time.time < startFade) yield return null;
            //        float destroyTime = Time.time + destroyAfter;
            float fadeTime = destroyAfter - fadeAfter;
            r = 1;
            while (r > 0.1f)
            {
                r = 1 - (Time.time - startFade) / fadeTime;
                text.color = textColor * r;
                //    image.color = rowColor * r * r;
                yield return null;

            }
            isDisabled = true;
            //        log.setScrollStateDirty();
            if (keepInPool)
                gameObject.SetActive(false);
            else
            {
                Destroy(gameObject);
                Debug.Log("destyrtogin");
            }
        }

        void adjustSizeToFitText()
        {
            if (layoutElement == null) layoutElement = GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                layoutElement.preferredHeight = text.preferredHeight;
                layoutElement.minHeight = layoutElement.preferredHeight;
            }
            //if (controller!=null)
            //   controller.setScrollStateDirty();
        }

        public override void OnClick()
        {
            if (!text.text.Equals(stackTrace))
            {
                valueCache = text.text;
                text.text = stackTrace;
                adjustSizeToFitText();
            }
            else
            {

                text.text = valueCache;
                adjustSizeToFitText();
            }
        }
    }
}