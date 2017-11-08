using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IAnimmateInOut
{
    void animateIn();
    void animateOut();
    bool isHidden();
}
public class EventPickActive : MonoBehaviour
{
    [Header("will enable one based on index")]
    public GameObject[] objects;
    public bool getChildren;
    void OnValidate()
    {
        if (getChildren)
        {
            //getChildren=false;
            getObjects();
        }
    }
    void getObjects()
    {
        objects = new GameObject[transform.childCount];
        for (int i = 0; i < objects.Length; i++) objects[i] = transform.GetChild(i).gameObject;
        PickActive(0);
    }

    void Start()
    {
        if (getChildren)
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
                {
                    objects[i].SetActive(true);
                    IAnimmateInOut animator = objects[i].GetComponent<IAnimmateInOut>();
                    if (animator != null)
                        animator.animateIn();
                }
                else
                {
                    if (objects[i].activeSelf)
                    {
                        IAnimmateInOut animator = objects[i].GetComponent<IAnimmateInOut>();

                        if (animator != null)
                        {
                            if (!animator.isHidden())
                                    animator.animateOut();

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
