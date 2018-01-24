using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public interface IName
{
 	string getLabel();

	MonoBehaviour getMono();
}

[System.Serializable]
public class Renamer  {

  public MonoBehaviour target;
 /*
  [SerializeField]
  IName nameSource;

  public void Init(IName src)
  {
	  nameSource=src;
	  target=src.getMono();
  }
*//*

	public virtual void OnValidate(MonoBehaviour mb)
	{
			name=renamer.validate(name,this);

		}


 */
	public virtual void OnValidate(MonoBehaviour mb)
	{
		target=mb;
		
	//	if (nameSource==null) Debug.Log("no name source");

		}
	public	void OnDestroy(MonoBehaviour mb)
	{
		Debug.Log("DESTROY");
	}
		
	
	public	void OnEnable(MonoBehaviour mb)
	{
		Debug.Log("OnEnable");
	}
		
	
	public	void OnDisable(MonoBehaviour mb)
	{
		Debug.Log("OnDisable");
	}
	
}
