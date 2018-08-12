// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Button : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float percentToBeFilled = .75f;

    // private


    // references
    public Deposit deposit;
	
	// --------------------- BASE METHODS ------------------
	void Start () {
		
	}
	
	void Update () {
        
	}

    private void OnMouseDown() {
        if (CanMove() || true) {
            Move();
        }
    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    private void Move() {
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", 6 * deposit.transform.forward, "time", 4f, "easeType", iTween.EaseType.easeInOutSine));
        Invoke("CleanShelf", 5);
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", -6 * deposit.transform.forward, "time", 4f, "easeType", iTween.EaseType.easeInOutSine, "delay", 6f));
    }

    void CleanShelf() {
        deposit.Clear();
    }


    // queries
    bool CanMove() {
        return deposit.PercentFilled() >= percentToBeFilled;
    }


    // other

}