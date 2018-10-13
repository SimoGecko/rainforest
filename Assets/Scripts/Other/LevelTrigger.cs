// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class LevelTrigger : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public int levelIndex;


    // private


    // references
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        
	}
	
	void Update () {
        
	}

    private void OnTriggerEnter(Collider other) {
        LevelManager.instance.LoadLevel(levelIndex);
    }

    // --------------------- CUSTOM METHODS ----------------


    // commands



    // queries



    // other

}