using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelFlip : MonoBehaviour {

    private bool faceRight;
    int playerNum;
    private ParticleSystem ps; // foam particles from extinguisher
    GameObject extinguisher;

    // Use this for initialization
    void Start () {
        faceRight = true;
        playerNum = transform.parent.GetComponent<Player1Controller>().playerNum;
        ps =  transform.parent.GetChild(2).GetComponent<ParticleSystem>();
        extinguisher = transform.parent.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update () {
        FaceDirection();
	}

    void FaceDirection()
    {
        Vector3 rot = transform.rotation.eulerAngles;

        if (Input.GetAxis("Horizontal" + playerNum) > 0 && !faceRight)
        {
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            ps.transform.rotation *= Quaternion.Euler(Vector3.up * 180);
            extinguisher.transform.localPosition = new Vector3(0.016f, 0.14f,-0.364f);
            faceRight = true;
        }
        if (Input.GetAxis("Horizontal" + playerNum) < 0 && faceRight)
        {
            rot = new Vector3(rot.x, rot.y + 180, rot.z);
            ps.transform.rotation *= Quaternion.Euler(Vector3.up * 180);
            extinguisher.transform.localPosition = new Vector3(0.113f, 0.14f, 0.291f);
            faceRight = false;
        }

        transform.rotation = Quaternion.Euler(rot);

    }
}
