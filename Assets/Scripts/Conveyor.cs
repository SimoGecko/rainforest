// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Conveyor : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public bool isSwitch = false;
    public bool flipX, flipZ;
    public float forwardProbability = .5f;
    const float switchCooldown = 1f;

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
            box.SetConveyorSpeed(Dir); // add the correct direction
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (isSwitch && CanSwitch && RandomSwitch) {
            SwitchDirection();
        }
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void SwitchDirection() {
        goForward = !goForward;
        changeDirTimer = Time.time + switchCooldown;
    }


    // queries
    public static float GetSpeed() {
        return GameManager.Playing? SpawnManager.instance.GetConveyorSpeed() : 0;
    }
    

    Vector3 Dir  { get { return goForward  ? transform.forward * (flipZ ? -1 : 1) : transform.right * (flipX ? -1 : 1); } }
    Vector3 Dir2 { get { return !goForward ? transform.forward * (flipZ ? -1 : 1) : transform.right * (flipX ? -1 : 1); } }

    bool CanSwitch { get { return Time.time > changeDirTimer; } }
    bool RandomSwitch { get { return Random.value <= (goForward ? 1 - forwardProbability : forwardProbability); } }

    // other
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Dir * 1);
        Gizmos.DrawCube(transform.position + Dir * 1, Vector3.one*.1f);
        if (isSwitch) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + Dir2 * 1);
            Gizmos.DrawCube(transform.position + Dir2 * 1, Vector3.one * .1f);
        }
    }

    //EDIT mode commands
    [ContextMenu("Align")]
    public void Align() {
        //modify this for special exceptions on alignment
        Vector3 v = transform.position;
        transform.position = new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }
    [ContextMenu("AlignAll")]
    void AlignAll() {
        Conveyor[] allc = FindObjectsOfType<Conveyor>();
        foreach (var c in allc) {
            c.Align();
        }
    }

}