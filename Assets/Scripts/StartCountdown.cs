using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCountdown : MonoBehaviour {

    private float timer = 0.0f;
    public TextMeshProUGUI countDownText;


	// Use this for initialization
	void Start () {
        countDownText.text = "3";
    }
	
	// Update is called once per frame
	void Update () {

        timer += Time.deltaTime;

        if (timer>1f)
        {
            countDownText.text = "2";
        }
        if (timer > 2f)
        {
            countDownText.text = "1";
        }
        if (timer > 3f)
        {
            countDownText.text = "Go!";
        }
        if (timer > 3.3f)
        {
            countDownText.text = "";
            Player1Controller.StartGame();
            Player2Controller.StartGame();
        }


    }
}
