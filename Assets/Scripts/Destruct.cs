using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruct : MonoBehaviour {

    public float force;
    public float radius;
    public float expiryTime;
    public float fadeSpeed;

	// Use this for initialization
	void Start () {

         foreach (Transform child in transform)
         {
            GameObject childObj = child.gameObject;
            Rigidbody rb = child.GetComponent<Rigidbody>();
            rb.AddExplosionForce(force, transform.position, radius);
         }
        Destroy(gameObject, expiryTime);

	}
	
	// Update is called once per frame
	void Update () {
        foreach (Transform child in transform)
        {
            GameObject childObj = child.gameObject;
            Color color = childObj.GetComponent<MeshRenderer>().material.color;
            color.a -= Time.deltaTime * fadeSpeed;
            childObj.GetComponent<MeshRenderer>().material.color = color;
         }
    }
}
