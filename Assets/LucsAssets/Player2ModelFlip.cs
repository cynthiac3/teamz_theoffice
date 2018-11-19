using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2ModelFlip : MonoBehaviour {

    private bool faceRight;
    int playerNum;

	// Use this for initialization
	void Start () {
        faceRight = true;
        playerNum = transform.parent.GetComponent<Player1Controller>().playerNum;
	}

    // Update is called once per frame
    void Update() {
        FaceDirection();
	}

    void FaceDirection()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        Debug.Log("Horizontal" + playerNum);

        if (Input.GetAxis("Horizontal" + playerNum) > 0 && !faceRight)
        {
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            faceRight = true;
        }
        if (Input.GetAxis("Horizontal" + playerNum) < 0 && faceRight)
        {
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            faceRight = false;
        }

        transform.rotation = Quaternion.Euler(rot);

    }

}
