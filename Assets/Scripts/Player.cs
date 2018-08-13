﻿// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float speed = 5;
    public float angularSpeed = 180;
    public float pickupArea = 3f;


    // private
    float ref1;
    Vector3 input;
    bool animStart, animEnd;
    float animHelloTime;



    // references
    public static Player instance;
    CharacterController cc;
    Animator anim;
    public Transform pickupCenter;
    AudioSource feet;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        animHelloTime = Time.time + Random.Range(2f, 4f);
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        feet = GetComponent<AudioSource>();
	}
	
	void Update () {
        if (GameManager.Playing) {
            SetToGround();
            Move();
        }
        else {
            input = Vector3.zero;
        }
        DealWithAnimations();
        feet.volume = input.normalized.magnitude/40;
        feet.pitch = Mathf.Lerp(feet.pitch, Random.Range(.8f, 1.2f), Time.deltaTime * 2);

	}

    private void FixedUpdate() {
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void Move() {
        input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 angularVel = Vector3.up * input.x * angularSpeed * Mathf.Lerp(.6f, 1f, Mathf.Abs(input.z)) *Mathf.Sign(input.z);
        Vector3 vel = transform.forward * input.z * speed;
        //rb.angularVelocity = angularVel * Mathf.Deg2Rad;
        //rb.velocity = vel;
        transform.Rotate(angularVel *  Time.deltaTime, Space.World);
        cc.Move(vel * Time.deltaTime);
        //transform.Translate(vel* Time.deltaTime, Space.World);
        
    }

    void SetToGround() {
        //to avoid bugs
        Vector3 temp = transform.position;
        temp.y = 0;
        transform.position = temp;
    }

    void DealWithAnimations() {
        //start
        if (Time.time > animHelloTime) {
            anim.SetTrigger("hello");
            animHelloTime = Time.time + Random.Range(4f, 8f);
        }

        //play
        if (GameManager.Playing && !animStart) {
            animStart = true;
            anim.SetTrigger("start");
        }
        bool running = input.magnitude > .1f;
        anim.SetBool("running", running);

        //end
        if (GameManager.Gameover && !animEnd) {
            animEnd = true;
            anim.SetTrigger("end");
        }
    }

    public void PushButtonAnim() {
        anim.SetTrigger("button");

    }

    /*
    void Move2() {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        Vector3 displacement = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * input.normalized; // camera
        
        //float dirZ = Vector3.Dot(transform.forward, input);
        //float dirX = Vector3.Dot(transform.right, input);

        float angle = input.magnitude>0? Vector3.SignedAngle(transform.forward, displacement, Vector3.up) : 0;
        Vector3 angularVel = Vector3.up * Mathf.Clamp(-angularSpeed, angularSpeed, angle);// * angularSpeed;
        Vector3 vel = transform.forward * input.magnitude * speed;

        //rb.angularVelocity = angularVel * Mathf.Deg2Rad;
        //rb.velocity = vel;
        transform.Rotate(angularVel * Time.deltaTime, Space.World);
        transform.Translate(vel * Time.deltaTime, Space.World);
    }

    void Move3() {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 displacement = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * input.normalized; // camera
        float goalAngle = transform.eulerAngles.y;
        if(displacement.magnitude>.05f)
            goalAngle = Mathf.Atan2(displacement.z, displacement.x)*Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, 90-goalAngle, ref ref1, .2f);
        transform.eulerAngles = Vector3.up * angle;

        transform.position += transform.forward * speed * displacement.magnitude * Time.deltaTime;
    }*/



    // queries
    public bool CloseEnough(Transform t) {
        Vector3 vec = pickupCenter.position - t.transform.position;
        vec.y = 0;
        float dist = Vector3.SqrMagnitude(vec);
        return dist <= pickupArea * pickupArea;
    }


    // other
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(pickupCenter.position, pickupArea);
    }

}