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
        canToggle = true;
    }

    void Update() {
        if(type == ToggleType.Tutorial) {
            //escape tutorial with whatever
            if (InTutorial && canToggle) {
                if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0) ||
                    Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("joystick button 1") || Input.GetKeyDown("joystick button 7")) {
                    ToggleTutorialOff();
                }
            }
        }


        if (GameManager.instance.Console) {
            if (type == ToggleType.Coop && Input.GetKeyDown("joystick button 1")) ToggleCoop();
            if (type == ToggleType.Tutorial && Input.GetKeyDown("joystick button 0") && canToggle && !InTutorial) ToggleTutorialOn();
            //DoToggle();
        }
        
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
        InterfaceManager.instance.ToggleTutorial(true);
        canToggle = false;
        Invoke("CanToggle", .1f);
    }
    void ToggleTutorialOff() {
        InterfaceManager.instance.ToggleTutorial(false);
        canToggle = false;
        Invoke("CanToggle", .1f);
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