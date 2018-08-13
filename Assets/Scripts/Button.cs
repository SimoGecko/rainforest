﻿// (c) Simone Guggiari 2018

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
    bool alreadyPushed;


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
        if (Player.instance.CloseEnough(transform)) {
            if (CanMove() || GameManager.instance.DEBUG) {
                if (!alreadyPushed)
                    Move();
            }
            else {
                ComicBubble.instance.Speak(SpeechType.ButtonNotFull);
            }
        }
        else {
            ComicBubble.instance.Speak(SpeechType.FarAway);

        }



        //TOO FAR AWAY

    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    private void Move() {
        alreadyPushed = true;
        AnimateShelf();
        AnimateSlidingDoor();
        Invoke("CleanShelf", animTime + delay/2);
        Invoke("AlreadyPushedOff", 2 * animTime + delay);
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

    void AlreadyPushedOff() { alreadyPushed = false; }


    // queries
    bool CanMove() {
        return deposit.PercentFilled() >= percentToBeFilled;
    }


    // other

}