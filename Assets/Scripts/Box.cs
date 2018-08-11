// (c) Simone Guggiari 2018

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


    // references
    Rigidbody rb;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void LateUpdate () {
        vel = Vector3.Lerp(vel, conveyorVelocity, Time.deltaTime*5);
        transform.position += vel * Time.deltaTime;
        conveyorVelocity = Vector3.zero;
	}

    private void OnMouseDown() {
        if (true) {//check if close enough
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
        transform.parent = Cart.instance.transform;
        transform.position = Cart.instance.TransfFromPos(size);

    }

    void DepositBox() {
        carrying = false;
        rb.isKinematic = true;

    }

    public void SetConveyorSpeed(Vector3 speed) {
        conveyorVelocity = speed;
    }



	// queries



	// other
	
}