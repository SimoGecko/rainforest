﻿// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

[RequireComponent(typeof(Rigidbody))]
public class Box : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public int size; // either 1, 2, 4

    // private
    Vector3 conveyorVelocity;
    Vector3 vel;
    bool carrying;
    [HideInInspector]
    public List<int> positions;
    bool triggered = false;


    // references
    Rigidbody rb;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void LateUpdate () {
        if (!carrying) {
            conveyorVelocity = conveyorVelocity.normalized * Conveyor.conveyorSpeed;
            vel = Vector3.Lerp(vel, conveyorVelocity, Time.deltaTime * 5);
            transform.position += vel * Time.deltaTime;
            conveyorVelocity = Vector3.zero;
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (!carrying && !triggered && other.tag == "ground") {
            GameManager.instance.LoseLife();
            triggered = true;
        }
    }

    private void OnMouseDown() {
        if (Player.instance.CloseEnough(this) && !carrying) {//check if close enough
            if (Cart.instance.HasSpaceFor(size)) {
                PickupBox();
                Cart.instance.Pickup(this);
            }
        }
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void PickupBox() {
        carrying = true;
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        positions = Cart.instance.FreePosition(size);
        transform.parent = Cart.instance.transform;
        transform.position = Cart.instance.TransfFromPos(positions);
        transform.localRotation = Quaternion.identity;
    }

    public void DepositBox() {
        carrying = false;
        rb.isKinematic = true;
        //add score
        GameManager.instance.AddScore(size);
        Destroy(gameObject);

    }

    public void SetConveyorSpeed(Vector3 speed) {
        conveyorVelocity += speed;
    }



	// queries



	// other
	
}