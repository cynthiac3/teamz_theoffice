using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foam : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Fire"))    // If touched by foam, extinguish
        {
            Debug.Log("Collision foam and fire.");
            transform.parent.GetComponent<Player1Controller>().fixHealth();
            Destroy(other.gameObject);
        }
    }
}
