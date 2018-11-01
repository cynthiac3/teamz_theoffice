using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{

    public float velocity;

    private Rigidbody mRigidbody;
    private float angularVelocity;
    private bool corner;
    private bool atAlarm;
    private float totalAngle;
    private Vector3 center;         // Center of rotation circle
    private float raduis;           // Radius of rotation circle
    private const string cornerTriggerTag = "CornerTrigger";
    private const string alarmTag = "FireAlarm";
    private const string lvlTag = "lvlTag";
    private int currentLvl;

    private void Start() {
        mRigidbody = GetComponent<Rigidbody>();
        raduis = Mathf.Abs(transform.position.z);
        angularVelocity = velocity / (2 * raduis);
        totalAngle = 0;
        corner = false;
        atAlarm = false;
        currentLvl = 1;
    }

    private void Update() {
        float inputHorizontal = Input.GetAxis("Horizontal2");
        Vector3 pos = mRigidbody.position;

        if (!corner)        // Move the character in a straight line
        {
            Vector3 movement = transform.forward * velocity * inputHorizontal * Time.deltaTime;
            pos += movement;

            // Round depth to +radius or -raduis            
            int sign = (int)Mathf.Round(pos.z / Mathf.Abs(pos.z));
            pos.z = sign * raduis;
        }
        else            // Move the character in a circular motion
        {
            totalAngle += -inputHorizontal * angularVelocity * Time.deltaTime;
            pos.x = center.x + raduis * Mathf.Sin(totalAngle);
            pos.z = center.z + raduis * Mathf.Cos(totalAngle);
        }

        mRigidbody.position = pos;
        mRigidbody.rotation = Quaternion.AngleAxis(totalAngle * Mathf.Rad2Deg - 90, Vector3.up);

    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Equals(cornerTriggerTag))     // Entering a corner
        {
            corner = true;
            center = transform.position;
            center.z = 0;
        }
        //added other possible collisions
        //collision to allow interaction with a fire alarm
        if (other.tag.Equals(alarmTag))
        {
            atAlarm = true;
        }
        //collision to track which floor a player is on
        if (other.tag.Equals(lvlTag))
        {
            string lvlId = other.GetComponent<Transform>().name;
            string lvlTrackers;
            for (int i = 1; i < 11; i++)
            {
                lvlTrackers = "lvl";
                if (lvlId == (lvlTrackers + i))
                {
                    currentLvl = i;
                    //Debug.Log("P2's current lvl is " + currentLvl);
                }

            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag.Equals(cornerTriggerTag))
        {                                           // Exiting a corner

            corner = false;

            // Round rotation to k * PI
            // Find k
            float k = Mathf.Round(totalAngle / (Mathf.PI));
            totalAngle = k * Mathf.PI;

        }
        if (other.tag.Equals(alarmTag))
        {
            atAlarm = false;
        }
    }

    //getters for use in "Sprinklers.cs"
    public bool getAlarmStatus()
    {
        return atAlarm;
    }

    public int getLvl()
    {
        return currentLvl;
    }
}
