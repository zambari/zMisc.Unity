//zzambari : stereoko 2017

using UnityEngine;
using UnityEngine.UI;
using Z;
namespace Z {
[ExecuteInEditMode]
[AddComponentMenu("")]
public class zDraggableNameHelper : MonoBehaviour {

	[SerializeField]
    [HideInInspector]
    Text text;
void OnValidate()
{
    setName();
}
void OnEnable()
{
      setName();
}
void Awake()
{
      setName();
}
public void setName(string s)
{

if (text!=null) text.text=s;

}
void setName()
{
    zDraggable zdr=GetComponentInParent<zDraggable>();
    if (text==null) text=GetComponent<Text>();
    if (zdr!=null&&text!=null)
        text.text=zdr.gameObject.name;
    else
     if (text!=null) text.text="_";

}

}
}