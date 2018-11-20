using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAlarms : MonoBehaviour {

    public GameObject[] alarmSet;

	// Use this for initialization
	void Start () {

        for(int i = 0; i < 5; i++ )
        {
            int alarmPos = Random.Range(0, 5);
            Instantiate(alarmSet[(i*5) + alarmPos], transform);
        }

	}

}
