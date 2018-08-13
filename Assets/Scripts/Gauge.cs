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
    float goalAngle;
    float angle;

    // references
    public GameObject needle;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        //iTween.ScaleFrom(gameObject, iTween.Hash("scale", Vector3.zero, "time", 2f, "delay", 3f, "easeType", iTween.EaseType.easeInOutSine));
        iTween.MoveFrom(gameObject, iTween.Hash("position", transform.position-Vector3.up*6, "time", 1f, "delay", 3f, "easeType", iTween.EaseType.easeInOutSine));

    }

    void Update () {
        ShakeNeedle();
	}


    // --------------------- CUSTOM METHODS ----------------


    // commands
    public void GetClicked(int d) {
        if (alreadyClicked) return;
        Debug.Log("difficulty=" + d);
        alreadyClicked = true;
        GameManager.instance.StartRound(d);
        iTween.ScaleTo(gameObject, iTween.Hash("scale", Vector3.zero, "time", 1f, "easeType", iTween.EaseType.easeInOutSine));
        Destroy(gameObject, 3f);
    }

    public void SetNeedle(int d) {
        goalAngle = 70 * (d - 1);
    }

    void ShakeNeedle() {
        angle = Mathf.Lerp(angle, goalAngle + Random.Range(-10, 10), Time.deltaTime*9);
        needle.transform.localEulerAngles = new Vector3(0, 0, angle);
    }




    // queries



    // other

}