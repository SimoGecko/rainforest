// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Conveyor : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public const float conveyorSpeed = 1f;
    public bool isSwitch = false;
    public bool flipX, flipZ;// = false;
    public float forwardProbability = .5f;

    // private
    float changeDirTimer;
    bool goForward = true;


    // references


    // --------------------- BASE METHODS ------------------
    void Start () {
		
	}
	
	void Update () {
        
	}

    private void OnTriggerStay(Collider other) {
        Box box = other.GetComponent<Box>();
        if (box != null) {
            box.SetConveyorSpeed(dir /* * conveyorSpeed*/);
        }
        //other.transform.position += transform.forward * conveyorSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (isSwitch && CanSwitch && RandomSwitch) {
            goForward = !goForward;
            changeDirTimer = Time.time + 1f;
        }
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands



    // queries
    Vector3 dir  { get { return goForward  ? transform.forward * (flipZ ? -1 : 1) : transform.right * (flipX ? -1 : 1); } }
    Vector3 dir2 { get { return !goForward ? transform.forward * (flipZ ? -1 : 1) : transform.right * (flipX ? -1 : 1); } }
    bool CanSwitch { get { return Time.time > changeDirTimer; } }
    bool RandomSwitch { get { return Random.value <= (goForward ? 1 - forwardProbability : forwardProbability); } }

    // other
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + dir * 1);
        Gizmos.DrawCube(transform.position + dir * 1, Vector3.one*.1f);
        if (isSwitch) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + dir2 * 1);
            Gizmos.DrawCube(transform.position + dir2 * 1, Vector3.one * .1f);
        }
    }

}