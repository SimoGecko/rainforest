﻿// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class InputManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public


    // private


    // references
    public VirtualJoystick joystick; // move in gamemanger?

    public static InputManager instance;



    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start() {
        //string[] jn = Input.GetJoystickNames();
        //foreach (var j in jn) Debug.Log(j);
    }

    void Update() {
        //if (Input.GetButtonDown("Interact")) Debug.Log("INteract");
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands



    // queries
    public Vector2 GetInput(int i=0) {
        if (GameManager.instance.Coop) {
            //single
            switch (GameManager.instance.platform) {
                case GameManager.Platform.Pc:
                    return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                case GameManager.Platform.Mobile:
                    return joystick.InputValue.To3();
                case GameManager.Platform.Console:
                    return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }
        } else {
            //coop
            string playerid = (i == 0) ? "" : "2";
            switch (GameManager.instance.platform) {
                case GameManager.Platform.Pc:
                    if(i==0)
                        return new Vector2(Input.GetAxis("HorizontalAD"), Input.GetAxis("VerticalWS"));
                    else
                        return new Vector2(Input.GetAxis("HorizontalLR"), Input.GetAxis("VerticalUD"));
                case GameManager.Platform.Console:
                    return new Vector2(Input.GetAxis("Horizontal"+playerid), Input.GetAxis("Vertical"+playerid));
            }

        }
        return Vector2.zero;
    }

    public bool GetInteractInput(int i=0) {
        string playerid = (i == 0) ? "" : "2";
        if (!GameManager.instance.Coop) {
            //single
            switch (GameManager.instance.platform) {
                case GameManager.Platform.Pc:
                    return Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return);
                case GameManager.Platform.Mobile:
                    break;
                case GameManager.Platform.Console:
                    return Input.GetButtonDown("Interact");
                default: return false;
            }
        } else {
            //coop
            switch (GameManager.instance.platform) {
                case GameManager.Platform.Pc:
                    if (i == 0)
                        return Input.GetKeyDown(KeyCode.Space);
                    else
                        return Input.GetKeyDown(KeyCode.Return);
                case GameManager.Platform.Mobile:
                    break;
                case GameManager.Platform.Console:
                    return Input.GetButtonDown("Interact"+playerid);
                default: return false;
            }
        }

        
        return false;
    }



    // other

}