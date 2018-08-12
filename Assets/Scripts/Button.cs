// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class Button : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float percentToBeFilled = .75f; // do I leave this in the game?
    public float animTime = 2f;
    public float delay = 1f;
    public float moveAmount = 10;

    // private
    bool animatedEntrance;


    // references
    public Deposit deposit;
    public GameObject slidingDoor;
	
	// --------------------- BASE METHODS ------------------
	void Start () {
		
	}
	
	void Update () {
        if(GameManager.Playing && !animatedEntrance) {
            animatedEntrance = true;
            AnimateEntrance();
        }
	}

    private void OnMouseDown() {
        if (!GameManager.Playing) return;
        if (CanMove() || true) {
            Move();
        }
    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    private void Move() {
        AnimateShelf();
        AnimateSlidingDoor();
        Invoke("CleanShelf", animTime + delay/2);
    }

    void CleanShelf() {
        deposit.Clear();
    }

    void AnimateEntrance() {
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", +moveAmount * Vector3.forward, "time", animTime, "easeType", iTween.EaseType.easeInOutSine));
        iTween.MoveBy(slidingDoor, iTween.Hash("amount", +moveAmount * Vector3.right, "time", animTime / 2, "easeType", iTween.EaseType.easeInOutSine));
        iTween.MoveBy(slidingDoor, iTween.Hash("amount", -moveAmount * Vector3.right, "time", animTime / 2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime / 2));
    }

    void AnimateShelf() {
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", -moveAmount * Vector3.forward, "time", animTime, "easeType", iTween.EaseType.easeInOutSine));
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", +moveAmount * Vector3.forward, "time", animTime, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime + delay));
    }

    void AnimateSlidingDoor() {
        iTween.MoveBy(slidingDoor, iTween.Hash("amount", +moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine));
        iTween.MoveBy(slidingDoor, iTween.Hash("amount", -moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime/2));
        iTween.MoveBy(slidingDoor, iTween.Hash("amount", +moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime + delay));
        iTween.MoveBy(slidingDoor, iTween.Hash("amount", -moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime + delay + animTime / 2));
    }


    // queries
    bool CanMove() {
        return deposit.PercentFilled() >= percentToBeFilled;
    }


    // other

}