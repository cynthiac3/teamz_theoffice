using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguish : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Foam"))    // If touched by foam, extinguish
        {
            Debug.Log("Collision foam and fire.");
            Destroy(gameObject);
        }
    }


}
