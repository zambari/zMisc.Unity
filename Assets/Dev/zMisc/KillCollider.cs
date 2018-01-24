using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCollider : MonoBehaviour {
	[ReadOnly]
	[SerializeField]
	int killCount;
	void Start()
	{
		Collider c = GetComponent<Collider> ();
		c.isTrigger = false;
	}
	void OnCollisionEnter(Collision c)
	{
		Destroy (c.collider.gameObject);
		killCount++;
	}
}