using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    public float lifetime;
    public float currentTime=0.0f;
    // Use this for initialization
    PickUpObject obj;

void Start() {
        obj = GetComponent<PickUpObject>();       
    }

    // Update is called once per frame
    void Update() {
        currentTime += Time.deltaTime;
        if (lifetime<= currentTime) {
            die();
        }
    }

    void die() {
        obj.destroyCircle();
        Destroy(gameObject);
    }
}
