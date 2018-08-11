// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Conveyor : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float conveyorSpeed = 5f;


	// private


	// references
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
		
	}
	
	void Update () {
        
	}

    private void OnTriggerStay(Collider other) {
        //Rigidbody rb = other.GetComponent<Rigidbody>();
        Box box = other.GetComponent<Box>();
        if (box != null) {
            box.SetConveyorSpeed(transform.forward*conveyorSpeed);
        }
        //other.transform.position += transform.forward * conveyorSpeed * Time.deltaTime;
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands



    // queries



    // other

}