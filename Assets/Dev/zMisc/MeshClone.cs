using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshClone : MonoBehaviour {


	public MeshFilter cloneObject;

	MeshFilter meshFilter;
	void Start()
	{
		meshFilter=GetComponent<MeshFilter>();
		meshFilter.mesh=cloneObject.mesh;

	}
	[ExposeMethodInEditor]
	public void randomizeObject()
	{
		Mesh currentMesh=meshFilter.sharedMesh;
		Vector3[] verts=currentMesh.vertices;
		for (int i=0;i<verts.Length;i++)
		{
			verts[i]=verts[i]+Random.onUnitSphere/20;
		}
		currentMesh.vertices=verts;


	}
}
