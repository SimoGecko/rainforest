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
    int currentD;

    // references
    public GameObject needle;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        iTween.MoveFrom(gameObject, iTween.Hash("position", transform.position - 6 * Vector3.up, "time", 1f, "delay", 3f, "easeType", iTween.EaseType.easeInOutSine));
    }

    void Update () {
        if (!GameManager.Menu) return;
        ShakeNeedle();

        if (GameManager.Console) {
            if (Input.GetKeyDown("joystick button 3") && !InterfaceManager.instance.InTutorial) GetClicked();
            //if (Input.GetKeyDown("joystick button 3")) GetClicked(1);
            //if (Input.GetKeyDown("joystick button 1")) GetClicked(2);

        }
        CheckInputController();
    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    void CheckInputController() {
        Vector2 inp = new Vector2(Input.GetAxisRaw("Horizontal0"), Input.GetAxisRaw("Vertical0"));
        if (inp.magnitude > .2f) {
            inp.Normalize();
            float angle = Mathf.Atan2(inp.y, inp.x)*Mathf.Rad2Deg;

            if (135 < angle && angle < 225) SetNeedle(0);
            if (45  < angle && angle < 135) SetNeedle(1);
            if (-45 < angle && angle <  45) SetNeedle(2);
        }

    }


    public void GetClicked(int d=-1) {
        if (alreadyClicked) return;
        if (d == -1) d = currentD;

        alreadyClicked = true;
        ScoreManager.instance.SetDifficulty(d);
        GameManager.instance.Play();
        iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "time", 1f, "easeType", iTween.EaseType.easeInOutSine));
        AudioManager.Play("button_push");
        Destroy(gameObject, 3f);
    }

    public void SetNeedle(int d) {
        currentD = d;
        needleGoalAngle = 70 * (d - 1);
    }

    void ShakeNeedle() {
        needleAngle = Mathf.Lerp(needleAngle, needleGoalAngle + Random.Range(-10, 10), Time.deltaTime * 9);
        needle.transform.localEulerAngles = new Vector3(0, 0, needleAngle);
    }




    // queries



    // other

}