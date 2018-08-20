// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class CoopToggle : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public


    // private


    // references


    // --------------------- BASE METHODS ------------------
    void Start() {

    }

    void Update() {
        if (GameManager.instance.Console && Input.GetKeyDown("joystick button 0"))
            ToggleCoop();
        
    }

    private void OnMouseDown() {
        ToggleCoop();
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void ToggleCoop() {
        GameManager.instance.Coop = !GameManager.instance.Coop;
    }


    // queries



    // other

}