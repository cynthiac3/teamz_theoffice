using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    public Vector3 torqueDir; //Custom torque vector for each of the 4 different objects
    public GameObject pickupInd;
    private GameObject circle;

    // Use this for initialization
    void Start()
    {
        
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddRelativeTorque(torqueDir * 50);
        circle = Instantiate(pickupInd, transform.position + new Vector3(0, -0.3f, 0), Quaternion.Euler(-90, 0, 0));

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag.Equals("Player"))
        {
            Destroy(gameObject);
            Destroy(circle);
        }
    }
}
