using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Rigidbody rb = GetComponent<Rigidbody>();
        //Attempts to spin obj while thrown. Unfortunately hard to notice...
        rb.AddRelativeTorque(Random.Range(0, 51), Random.Range(0, 51), Random.Range(0, 51)); 
	}

    private void OnCollisionEnter(Collision other)
    {
        StartCoroutine(DestroyThrown());
    }

    public IEnumerator DestroyThrown()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
