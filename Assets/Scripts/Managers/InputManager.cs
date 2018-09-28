// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////////// DESCRIPTION //////////

public class InputManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public


    // private


    // references
    public VirtualJoystick joystick;
    public static InputManager instance;



    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start() {

    }

    void Update() {

    }



    // --------------------- CUSTOM METHODS ----------------


    // commands



    // queries
    public Vector2 GetInput(int i = 0) {
        if (GameManager.Mobile) {
            return joystick.InputValue.To3();
        } else {

            Vector2 k = Vector2.zero; // keyboard
            Vector2 j = Vector2.zero; // joystick
            if (ElementManager.Single) {
                //single
                k = new Vector2(Input.GetAxis("Horizontal"),  Input.GetAxis("Vertical"));
                j = new Vector2(Input.GetAxis("Horizontal1"), Input.GetAxis("Vertical1"));
            } else {
                //multiple
                j = new Vector2(Input.GetAxis("Horizontal" + (i + 1)), Input.GetAxis("Vertical" + (i + 1)));
                if (i == 0) k = new Vector2(Input.GetAxis("HorizontalAD"), Input.GetAxis("VerticalWS"));
                if (i == 1) k = new Vector2(Input.GetAxis("HorizontalLR"), Input.GetAxis("VerticalUD"));
            }
            return k + j;
        }
    }

    public bool GetInteractInput(int i = 0) {
        bool k = false;
        bool j = false;
        if (ElementManager.Single) {
            //single
            k = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
            j =Input.GetButtonDown("Interact1");
        } else {
            //multiple
            if (i == 0) k = Input.GetKeyDown(KeyCode.Space);
            if (i == 1) k = Input.GetKeyDown(KeyCode.Return);
            j = Input.GetButtonDown("Interact" + (i + 1));
        }
        return k || j;
    }

    public bool GetSprintInput(int i = 0) {
        bool k = false;
        bool j = false;
        if (ElementManager.Single) {
            //single
            k = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            j = (Input.GetAxis("Sprint1L") != 0) || (Input.GetAxis("Sprint1R") != 0);
        } else {
            //multiple
            if (i == 0) k = Input.GetKey(KeyCode.LeftShift);
            if (i == 1) k = Input.GetKey(KeyCode.RightShift);
            j = (Input.GetAxis("Sprint" + (i + 1) + "L") != 0) || (Input.GetAxis("Sprint" + (i + 1) + "R") != 0);
        }
        return k || j;
    }


    public bool PauseInput() {
        return Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Escape);
    }
    public bool ResumeInput() {
        return Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 0");
    }
    public bool RestartInput() {
        return Input.GetKeyDown("joystick button 6") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1");
    }
    public bool OverRestartInput() {
        return Input.GetKeyDown("joystick button 7");
    }


    // other

}