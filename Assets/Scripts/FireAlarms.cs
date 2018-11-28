using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAlarms : MonoBehaviour
{

    public GameObject[] alarmSet;
    public GameObject useCircle;
    private GameObject[] newAlarms = new GameObject[5];
    private GameObject[] circle = new GameObject[5];
    private Sprinklers sprinkScript;
    private bool spawnCoolDown;

    // Use this for initialization
    void Start()
    {
        float yDisplacement = 1.8f;

        for (int i = 0; i < 5; i++)
        {
            int alarmPos = Random.Range(0, 5);

            if (i == 0)
            {
                newAlarms[i] = Instantiate(alarmSet[(i * 5) + alarmPos], transform);
                newAlarms[i].transform.position -= new Vector3(0, yDisplacement, 0);
                circle[i] = Instantiate(useCircle, newAlarms[i].transform.position + new Vector3(0, -1.75f, 0), Quaternion.Euler(-90, 0, 0));
            }
            else if(i % 2 == 0)
            {
                newAlarms[i] = Instantiate(alarmSet[(i * 5) + alarmPos], transform);
                newAlarms[i].transform.position += new Vector3(0, yDisplacement * 3, 0);
                circle[i] = Instantiate(useCircle, newAlarms[i].transform.position + new Vector3(0, -1.75f, 0), Quaternion.Euler(-90, 0, 0));
            }
            else
            {
                newAlarms[i] = Instantiate(alarmSet[(i * 5) + alarmPos], transform);
                circle[i] = Instantiate(useCircle, newAlarms[i].transform.position + new Vector3(0, -1.75f, 0), Quaternion.Euler(-90, 0, 0));
            }
        }
        GameObject sprinkObj = GameObject.Find("Sprinklers");
        sprinkScript = sprinkObj.GetComponent<Sprinklers>();
        spawnCoolDown = true;
    }

    void Update()
    {
        if (sprinkScript.getCD() && spawnCoolDown)
        {
            spawnCoolDown = false;
            StartCoroutine(RemoveAndRestoreCircles());
        }
    }

    public IEnumerator RemoveAndRestoreCircles()
    {
        for (int i = 0; i < 5; i++)
        {
            Destroy(circle[i]);
        }
        yield return new WaitForSeconds(20);
        for (int i = 0; i < 5; i++)
        {
            circle[i] = Instantiate(useCircle, newAlarms[i].transform.position + new Vector3(0, -1.75f, 0), Quaternion.Euler(-90, 0, 0));
        }
        spawnCoolDown = true;
    }
}
