using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionGroup : MonoBehaviour {

public GameObject activeTransitionGameObject;
public  GameObject nextActiveGameObject;
public ITransition activeTransition;
public  ITransition nextActive;
public bool inTransitionInProgress;
public bool outTransitionInProgress;
[Header("debug")]
public bool manualActivation;
	[Range(0,1)]
    public float activateManual;
public bool readyForTransition { get { return (!inTransitionInProgress)&&(!outTransitionInProgress);} }

void OnValidate()
{
	getObjects();
	if (manualActivation)
	{
		int nextNr=Mathf.FloorToInt(activateManual*objects.Length);
		if (nextNr>=objects.Length) nextNr=objects.Length-1;
	  	PickActive(nextNr);
	}
}
public void RequestActivation(ITransition requester)
{
	if (Application.isPlaying)
	{

	if (activeTransition==null)
	{
		activeTransition=requester;
		activeTransitionGameObject=requester.gameObject;
		activeTransition.fadeInComplete=InTransitionComplete;
		activeTransition.AnimateIn();
		inTransitionInProgress=true;
	} else
	{
		 activeTransition.readyForNext=OutTransitionReadyForNext;
		 activeTransition.fadeOutComplete=OutTransitionFadeOutComplete;
		 activeTransition.AnimateOut();
		 outTransitionInProgress=true;
		 nextActive=requester;
		 nextActiveGameObject=requester.gameObject;
	}
	
	

	} else
	{
		if (activeTransition!=null) activeTransition.gameObject.SetActive(false);
		activeTransition=requester;
		activeTransition.InstantIn();
	}

}
void InTransitionComplete(ITransition src)
{
inTransitionInProgress=false;
nextActive=null;
activeTransition=src;
}

void OutTransitionFadeOutComplete(ITransition src)
{
    outTransitionInProgress=false;
	if (activeTransition==src) activeTransition=null;
}
void OutTransitionReadyForNext(ITransition src)
{
	nextActive.AnimateIn();
	nextActive.fadeInComplete=InTransitionComplete;
}
    void getObjects()
    {
        objects = new GameObject[transform.childCount];
        for (int i = 0; i < objects.Length; i++) objects[i] = transform.GetChild(i).gameObject;
        PickActive(0);
    }
    public GameObject[] objects;
    void Start()
    {
 
            getObjects();
    }
    void Reset()
    {
        getObjects();
    }
	
    public void PickActive(int v)
    {
        if (v < 0 || v >= objects.Length) { Debug.Log("invalid selection " + v, gameObject); return; }
        for (int i = 0; i < objects.Length; i++)

            if (objects[i] != null)
            {
                if (i == v)
                {  objects[i].SetActive(true);
							if (activeTransition!=null) 
					{
						activeTransition.readyForNext=OutTransitionReadyForNext;
						activeTransition.AnimateOut();
					}
				ITransition nextTransition=objects[i].GetComponent<ITransition>();
				if (nextTransition!=null)
				{
		
						nextActive=nextTransition;
					} else
					activeTransition=nextTransition;
					nextTransition.AnimateIn();
				}
                  
				/*	if (nextTransition!=null) nextTransition.AnimateIn();
                   nextAnimator = objects[i].GetComponent<IAnimmateInOut>();
                    if (nextAnimator != null)
                    {
                        //nextAnimator.readyForNext+=DelayedIn;
                        if (delayBeforeAnimateIn > 0)
                            Invoke("DelayedIn", delayBeforeAnimateIn);
                        else
                            nextAnimator.AnimateIn();
                    }*/

             
                else
                {
                    if (objects[i].activeSelf)
                    {
                        ITransition animator = objects[i].GetComponent<ITransition>();

                        if (animator != null)
                        {
                         //   if (!animator.isHidden())
                                animator.AnimateOut();

                        }
                        else objects[i].SetActive(false);
                    }
                }
            }
    }

    public void PickActiveFloat(float v)
    {
        PickActive(Mathf.RoundToInt(v));
    }

	
}
