using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class instructionsManager : MonoBehaviour {

    public GameObject viewItems;
    bool t = false;

    // Use this for initialization
    void Start () {
        

    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene(2);
        }

        if (Input.GetKeyDown("escape"))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown("right") || Input.GetKeyDown("left"))
        {
            t = !t;
            viewItems.active = t;
        }



    }





}
