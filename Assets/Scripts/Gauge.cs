// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Gauge : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public


    // private
    bool alreadyClicked;
    float needleAngle, needleGoalAngle;

    // references
    public GameObject needle;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        iTween.MoveFrom(gameObject, iTween.Hash("position", transform.position - 6 * Vector3.up, "time", 1f, "delay", 3f, "easeType", iTween.EaseType.easeInOutSine));
    }

    void Update () {
        ShakeNeedle();

        /*
        if(GameManager.instance.platform == GameManager.Platform.Console) {
            if (Input.GetButtonDown("joystick button 2")) GetClicked(0);
            if (Input.GetButtonDown("joystick button 3")) GetClicked(1);
            if (Input.GetButtonDown("joystick button 1")) GetClicked(2);

        }*/
    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    public void GetClicked(int d) {
        if (alreadyClicked) return;

        alreadyClicked = true;
        GameManager.instance.StartRound(d);
        iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "time", 1f, "easeType", iTween.EaseType.easeInOutSine));
        AudioManager.Play("button_push");
        Destroy(gameObject, 3f);
    }

    public void SetNeedle(int d) {
        needleGoalAngle = 70 * (d - 1);
    }

    void ShakeNeedle() {
        needleAngle = Mathf.Lerp(needleAngle, needleGoalAngle + Random.Range(-10, 10), Time.deltaTime * 9);
        needle.transform.localEulerAngles = new Vector3(0, 0, needleAngle);
    }




    // queries



    // other

}