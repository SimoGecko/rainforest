// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class GaugePart : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public int difficulty;


    // private


    // references
    Gauge gauge;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        gauge = FindObjectOfType<Gauge>();
	}
	
	void Update () {
        
	}

    private void OnMouseOver() {
        gauge.SetNeedle(difficulty);
    }

    private void OnMouseDown() {
        gauge.GetClicked(difficulty);
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands



    // queries



    // other

}