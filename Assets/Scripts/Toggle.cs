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
    bool canToggle;

    // references


    // --------------------- BASE METHODS ------------------
    void Start() {

    }

    void Update() {
        if(type == ToggleType.Tutorial) {
            //escape tutorial with whatever
            if (InTutorial && canToggle) {
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0)) {
                    ToggleTutorialOff();
                }
            }
        }


        if (GameManager.instance.Console && Input.GetKeyDown("joystick button 0") && type==ToggleType.Coop)
            DoToggle();
        
    }

    private void OnMouseDown() {
        DoToggle();
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void DoToggle() {
        if (type == ToggleType.Tutorial) ToggleTutorialOn();
        if (type == ToggleType.Coop) ToggleCoop();
    }

    void ToggleTutorialOn() {
        if (!InterfaceManager.instance.InTutorial) {
            InterfaceManager.instance.ToggleTutorial(true);
            canToggle = false;
            Invoke("CanToggle", .1f);
        }
    }
    void ToggleTutorialOff() {
        InterfaceManager.instance.ToggleTutorial(false);
        canToggle = false;
    }

    void CanToggle() { canToggle = true; }

    void ToggleCoop() {
        if (!InterfaceManager.instance.InTutorial)
            //GameManager.instance.Coop = !GameManager.instance.Coop;
            GameManager.instance.ToggleNumPlayers();
    }


    // queries
    bool InTutorial { get { return InterfaceManager.instance.InTutorial; } }


    // other

}