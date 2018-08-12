// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class InterfaceManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float animTime = 2f;


    // private


    // references
    public GameObject title;
    public GameObject subtitle;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        AnimateTitle();

    }
	
	void Update () {
        
	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands
    void AnimateTitle() {
        iTween.MoveFrom(title,    iTween.Hash("position", title.transform.position + title.transform.right * 35, "time", animTime, "easeType", iTween.EaseType.easeInOutSine));
        //iTween.MoveFrom(subtitle, iTween.Hash("position", subtitle.transform.position - subtitle.transform.up * 15, "time", animTime, "easeType", iTween.EaseType.easeInOutSine, "delay", 2f));
        iTween.FadeFrom(subtitle, iTween.Hash("alpha", 0f, "time", animTime, "delay", 2f));
    }



	// queries



	// other
	
}