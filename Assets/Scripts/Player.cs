// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float speed = 5;
    public float angularSpeed = 180;


    // private


    // references
    Rigidbody rb;
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void Update () {
        Move();
	}

    private void FixedUpdate() {
        Move();
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void Move() {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        /*
        input = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * input; // camera
        float dirZ = input.z;//Vector3.Dot(transform.forward, input);
        float dirX = input.x;//Vector3.Dot(transform.right, input);*/
        Vector3 vel = transform.forward * input.z * speed;
        Vector3 angularVel = Vector3.up * input.x * angularSpeed*Mathf.Deg2Rad * Mathf.Lerp(.6f, 1f, Mathf.Abs(input.z)) *Mathf.Sign(input.z);
        rb.angularVelocity = angularVel;
        rb.velocity = vel;
        //transform.Rotate(Vector3.up *  Time.deltaTime * , Space.World);
        //transform.Translate(vel* Time.deltaTime, Space.World);
    }



	// queries



	// other
	
}