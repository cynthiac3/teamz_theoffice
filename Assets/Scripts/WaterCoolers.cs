using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCoolers : MonoBehaviour {

    public GameObject[] coolerSet;

    // Use this for initialization
    void Start () {
        float yDisplacement = 0;
        int variant = Random.Range(0, 3);

        for (int i = 0; i < 7; i++)
        {
            if (Random.Range(0, 4) > 0)
            {
                GameObject newCooler = Instantiate(coolerSet[variant], transform);
                newCooler.transform.position += new Vector3(0, yDisplacement, 0);
                
                variant = Random.Range(0, 3);
            }
            yDisplacement += 1.8f;
        }
    }
}
