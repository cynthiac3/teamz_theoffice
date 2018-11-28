using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelFlip : MonoBehaviour {

    private bool faceRight;
    int playerNum;
    private ParticleSystem ps; // foam particles from extinguisher
    GameObject extinguisher;
    Quaternion targetRotation;
    public AudioSource step1;
    public AudioSource step2;

    // Use this for initialization
    void Start () {
        faceRight = true;
        playerNum = transform.parent.GetComponent<Player1Controller>().playerNum;
        ps =  transform.parent.GetChild(2).GetComponent<ParticleSystem>();
        extinguisher = transform.parent.GetChild(1).gameObject;
        targetRotation = transform.rotation;
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

    public void turnToWall()
    {
        if (faceRight)
        {
            turnBack();
            Invoke("turn", 0.5f);
        }
        else
        {
            turn();
            Invoke("turnBack", 0.5f);
        }

    }

    void turn()
    {

        Vector3 rot = transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y + 90, rot.z);
        transform.rotation = Quaternion.Euler(rot);

        //extinguisher
        extinguisher.transform.RotateAround(transform.parent.position, Vector3.up, 90);//1 is speed

    }
    void turnBack()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot = new Vector3(rot.x, rot.y - 90, rot.z);
        transform.rotation = Quaternion.Euler(rot);

        //extinguisher
        extinguisher.transform.RotateAround(transform.parent.position, Vector3.up, -90);//1 is speed

    }

    public void Footstep()
    {
        if (Random.Range(0, 2) == 0)
            step1.Play();
        else
            step2.Play();
    }

}
