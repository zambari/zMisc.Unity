using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CollisionSplit : MonoBehaviour
{

    [ReadOnly]
    [SerializeField]
    int collisionCount;

    [ReadOnly]
    [SerializeField]
    float totalCollisionForce;
    int maxCollisionCount;

    [Range(0.1f, 20)]
    public float collisionForceTreshold = 1;
    Rigidbody rigid;

    float velocityScale = 1.5f;

    float velocityRandom = 0.2f;
	public float delay=0.2f;
	public float subsequentDelay=0.2f;
    public Rigidbody Activate(Rigidbody source=null)
    {  rigid = GetComponent<Rigidbody>();
	   if (rigid == null) rigid = gameObject.AddComponent<Rigidbody>();
      	 totalCollisionForce = 0;
        return rigid;
    }

    void Start()
    {
        
      

        Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
#if UNITY_EDITOR
        for (int i = 1; i < rbs.Length; i++)
            Undo.DestroyObjectImmediate(rbs[i]);
#else
		for(int i=1;i<rbs.Length;i++)
			Destroy(rbs[i]);
#endif
        Activate();
    }
    protected virtual void OnCollisionEnter(Collision col)
    {
        totalCollisionForce += col.impulse.magnitude;
        if (totalCollisionForce > collisionForceTreshold && split==null)
         split=StartCoroutine( Split());

    }
	Coroutine split;

    public IEnumerator  Split()
    {
		yield return new WaitForSeconds(delay);

        Vector3 velocity = rigid.velocity;
        Vector3 angularVelocity = rigid.angularVelocity;

        for (int i = 0; i < transform.childCount; i++)
        {
            CollisionSplit cs = transform.GetChild(i).GetComponent<CollisionSplit>();
            if (cs != null)
            {
                Rigidbody rb = cs.Activate(rigid);
                rb.velocity = velocity * velocityScale * (1 + Random.value * velocityRandom);
                rb.angularVelocity = angularVelocity * velocityScale * (1 + Random.value * velocityRandom);
				if (subsequentDelay>0)  yield return new WaitForSeconds(subsequentDelay);
            }
        }
       // Destroy(rigid);
    }

    [ExposeMethodInEditor]
    void AddSpllittersToChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform thisChild = transform.GetChild(i);
            CollisionSplit cs = thisChild.GetComponent<CollisionSplit>();
            if (cs == null) thisChild.gameObject.AddComponent<CollisionSplit>();
            if (transform.parent!=null)
					thisChild.SetParent(transform.parent);
        }
    }

}
