// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class ConveyorManager : MonoBehaviour {
	// --------------------- VARIABLES ---------------------
	
	// public


	// private


	// references
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
		
	}
	
	void Update () {
        
	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands
    [ContextMenu("Align")]
    void AlignAll() {
        Conveyor[] allc = FindObjectsOfType<Conveyor>();
        foreach(var c in allc) {
            Vector3 v = c.transform.position;
            c.transform.position = new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
        }
    }



	// queries



	// other
	
}