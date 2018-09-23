// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

////////// moves character //////////

public class PlayerMovement : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float speed = 18;
    public float sprintMultiplier = 1.4f;
    public float angularSpeed = 500;

    // private
    bool rotateInput = true;


    // references
    Player player;
    CharacterController cc;
    public Text controlText;


    // --------------------- BASE METHODS ------------------
    void Start () {
        if (!isLocalPlayer) Destroy(this);

        player = GetComponent<Player>();
        cc = GetComponent<CharacterController>();
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Tab)) // temporary
            ToggleControlType();

        if (GameManager.Playing) {
            Move();
            ProjectToArea();
        }
    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    void ToggleControlType() {
        rotateInput = !rotateInput;
        string textString = "control: " + (rotateInput ? "camera" : "player") + "-relative\nTAB to change";
        if (controlText != null)
            controlText.text = textString;
    }


    void Move() {
        Vector3 input = player.MoveDir;
        bool running = player.Running;

        // Overcooked style
        Vector3 inputRotated = rotateInput ? (Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * input) : input;

        float signedAngle = Vector3.SignedAngle(transform.forward, inputRotated, Vector3.up);
        float rotAmount = angularSpeed * Time.deltaTime;
        rotAmount = Mathf.Min(rotAmount, Mathf.Abs(signedAngle)); // clamp it if it's smaller
        transform.Rotate(Vector3.up * rotAmount * Mathf.Sign(signedAngle));

        float angle = Vector3.Angle(transform.forward, inputRotated);
        //project movement onto direction
        if (angle <= 90) {
            float dot = Vector3.Dot(transform.forward, inputRotated); // projection for smooth results
            float runspeed = speed * (running ? sprintMultiplier : 1);
            cc.Move(transform.forward * runspeed * inputRotated.magnitude * dot * Time.deltaTime);
        }
    }

    void ProjectToArea() {
        //to avoid bugs
        Vector3 temp = transform.position;
        Rect area = ElementManager.instance.playArea;
        temp.x = Mathf.Clamp(temp.x, area.xMin, area.xMax);
        temp.z = Mathf.Clamp(temp.z, area.yMin, area.yMax);
        temp.y = 0;
        transform.position = temp;
    }


    // queries



    // other

}