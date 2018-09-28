// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////////// DESCRIPTION //////////

public class Button : MonoBehaviour, IInteractable {
    // --------------------- VARIABLES ---------------------

    // public
    //public float percentToBeFilled = .8f;
    const float animTime = 2f;
    const float animDelay = 1f;
    const float moveAmount = 10;
    public float introDelay;

    // private
    bool alreadyPushed;


    // references
    public Deposit deposit;
    public Door door;
    SpriteRenderer buttonLight;
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        GameManager.instance.EventOnPlay += PlayEntranceAnimation;
        buttonLight = GetComponentInChildren<SpriteRenderer>();
	}
	
	void Update () {
        if (GameManager.Playing) {
            CheckBlinking();
            
        }
	}

    // --------------------- CUSTOM METHODS ----------------


    // commands
    void PlayEntranceAnimation() {
        Invoke("AnimateEntrance", 2f + introDelay);
    }

    public void Interact(Player p) {
        if (!p.pI.CloseEnough(transform)) {
            p.Bubble.Speak(SpeechType.FarAway);
            return;
        }
        if (!deposit.FilledEnough()) {
            p.Bubble.Speak(SpeechType.ButtonNotFull);
            return;
        }
        
        if (!alreadyPushed) {
            EmptyShelfWithAnimation();
            p.AnimButton();
            AudioManager.Play("button_push");
            AudioManager.Play("shelf_moving");
        }
    }

    /*
    public void Tap(Player p) {
        if (p.CloseEnough(transform)) {
            if (deposit.FilledEnough() || GameManager.instance.DEBUG) {
                if (!alreadyPushed) {
                    EmptyShelfWithAnimation();
                    p.AnimButton();
                    AudioManager.Play("button_push");
                    AudioManager.Play("shelf_moving");
                }
            }
            else {
                p.Bubble.Speak(SpeechType.ButtonNotFull);
            }
        }
        else {
            p.Bubble.Speak(SpeechType.FarAway);
        }
    }*/

    private void EmptyShelfWithAnimation() {
        alreadyPushed = true;
        AnimateShelf();
        AnimateSlidingDoor();
        Invoke("CleanShelf", animTime + animDelay/2);
        Invoke("AlreadyPushedOff", 2 * animTime + animDelay);
    }

    void CheckBlinking() {
        bool blinking = deposit.FilledEnough();
        bool rightTime = Mathf.RoundToInt(Time.time) % 2 == 0;
        if(buttonLight!=null)
            buttonLight.enabled = blinking && rightTime;
    }

    void CleanShelf() { deposit.Clear(); }
    void AlreadyPushedOff() { alreadyPushed = false; }



    void AnimateEntrance() {
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", +moveAmount * Vector3.forward, "time", animTime, "easeType", iTween.EaseType.easeInOutSine));
        iTween.MoveBy(door.gameObject, iTween.Hash("amount", +moveAmount * Vector3.right, "time", animTime / 2, "easeType", iTween.EaseType.easeInOutSine));
        iTween.MoveBy(door.gameObject, iTween.Hash("amount", -moveAmount * Vector3.right, "time", animTime / 2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime / 2));
    }

    void AnimateShelf() {
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", -moveAmount * Vector3.forward, "time", animTime, "easeType", iTween.EaseType.easeInOutSine));
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", +moveAmount * Vector3.forward, "time", animTime, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime + animDelay));
    }

    void AnimateSlidingDoor() {
        iTween.MoveBy(door.gameObject, iTween.Hash("amount", +moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine));
        iTween.MoveBy(door.gameObject, iTween.Hash("amount", -moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime/2));
        iTween.MoveBy(door.gameObject, iTween.Hash("amount", +moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime + animDelay));
        iTween.MoveBy(door.gameObject, iTween.Hash("amount", -moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime + animDelay + animTime / 2));
    }





    // queries
    public bool CanInteract() {
        return !alreadyPushed;
    }


    // other

}