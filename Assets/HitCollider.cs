using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollider : MonoBehaviour {

    public string hitName;
    public GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Transform>().tag == "Player")
        {
            other.GetComponent<Player1Controller>().getPunched();
        }
    }
}
