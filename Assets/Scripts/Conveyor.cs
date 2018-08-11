// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Conveyor : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public const float conveyorSpeed = 1f;
    public bool goForward = true;
    public bool invert = false;
    public bool changeDir = false;

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
            //Vector3 dir = goForward ? transform.forward : transform.right;
            box.SetConveyorSpeed(dir /** conveyorSpeed*/);
        }
        //other.transform.position += transform.forward * conveyorSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (changeDir && Random.value <= .5f) goForward = !goForward;
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands



    // queries
    Vector3 dir { get { return (goForward ? transform.forward : transform.right) * (invert ? -1:1) ; } }



    // other
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + dir * 2);
        Gizmos.DrawCube(transform.position + dir * 2, Vector3.one*.1f);
    }

}