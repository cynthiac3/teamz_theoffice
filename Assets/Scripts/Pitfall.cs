using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitfall : MonoBehaviour {

    public bool destructable;
    public GameObject debris;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {

        if (destructable && other.tag == "Player")
        {
            Vector3 offset = new Vector3(0, 3.5f, 0);
            GameObject parent = transform.parent.gameObject;
            Destroy(parent);
            Instantiate(debris, parent.transform.position + offset, parent.transform.rotation);
        }
    }
}
