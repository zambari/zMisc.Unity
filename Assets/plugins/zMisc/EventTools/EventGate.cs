using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGate : MonoBehaviour {

public void EventGateInput()
{
	if (gateOpen)
	whenGateOpen.Invoke();
	else
	whenGateClosed.Invoke();
}
public VoidEvent whenGateOpen;
public VoidEvent whenGateClosed;


[SerializeField]
private bool _gateOpen;
public void SetGateOpen(bool b)
{
	gateOpen=b;
}
public bool gateOpen
{
	get 
	{
		return _gateOpen;
	} 
	set
	{
		_gateOpen = value;
	}
}

}
