// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Toggle : MonoBehaviour {
    // --------------------- VARIABLES ---------------------
    public enum ToggleType { Tutorial, Coop}
    // public
    public ToggleType type;

    // private


    // references


    // --------------------- BASE METHODS ------------------
    void Start() {

    }

    void Update() {
        if(type == ToggleType.Tutorial) {
            //escape tutorial with whatever
            if (InTutorial) {

                if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) {
                    //ToggleTutorial();
                }
            }
        }


        if (GameManager.instance.Console && Input.GetKeyDown("joystick button 0"))
            ToggleCoop();
        
    }

    private void OnMouseDown() {
        DoToggle();
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void DoToggle() {
        if (type == ToggleType.Tutorial) ToggleTutorial();
        if (type == ToggleType.Coop) ToggleCoop();
    }

    void ToggleTutorial() {
        InterfaceManager.instance.ToggleTutorial();
    }

    void ToggleCoop() {
        if(!InterfaceManager.instance.InTutorial)
            GameManager.instance.Coop = !GameManager.instance.Coop;
    }


    // queries
    bool InTutorial { get { return InterfaceManager.instance.InTutorial; } }


    // other

}