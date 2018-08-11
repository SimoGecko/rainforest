// (c) Simone Guggiari 2018

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



    // references
    public static Player instance;
    CharacterController cc;
    public Transform cart;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        cc = GetComponent<CharacterController>();
	}
	
	void Update () {
        Move1();
	}

    private void FixedUpdate() {
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void Move1() {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 angularVel = Vector3.up * input.x * angularSpeed * Mathf.Lerp(.6f, 1f, Mathf.Abs(input.z)) *Mathf.Sign(input.z);
        Vector3 vel = transform.forward * input.z * speed;
        //rb.angularVelocity = angularVel * Mathf.Deg2Rad;
        //rb.velocity = vel;
        transform.Rotate(angularVel *  Time.deltaTime, Space.World);
        cc.Move(vel * Time.deltaTime);
        //transform.Translate(vel* Time.deltaTime, Space.World);
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
    public bool CloseEnough(Box b) {
        float dist = Vector3.SqrMagnitude(cart.position - b.transform.position);
        return dist <= pickupArea * pickupArea;
    }


    // other
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(cart.position, pickupArea);
    }

}