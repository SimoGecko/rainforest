// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Button : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float percentToBeFilled = .75f;
    public float animTime = 2f;
    public float delay = 1f;
    public float moveAmount = 10;

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
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", -moveAmount * Vector3.forward, "time", animTime, "easeType", iTween.EaseType.easeInOutSine));
        Invoke("CleanShelf", animTime + delay/2);
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", +moveAmount * Vector3.forward, "time", animTime, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime + delay));
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