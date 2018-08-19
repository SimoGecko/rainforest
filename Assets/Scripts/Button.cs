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
    public float animDelay = 1f;
    public float moveAmount = 10;
    public float introDelay;

    // private
    bool alreadyPushed;
    bool GameManagerPlay;


    // references
    public Deposit deposit;
    public GameObject slidingDoor;
    public GameObject buttonLight;
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        //GameManager.instance.OnPlay += PlayEntranceAnimation;
	}
	
	void Update () {
        if (GameManager.Playing) {
            CheckBlinking();
            if (!GameManagerPlay) { // for some reason action doesn't work
                GameManagerPlay = true;
                PlayEntranceAnimation();
            }
        }
	}

    private void OnMouseDown() {
        if(GameManager.Playing)
            Tap();
    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    void PlayEntranceAnimation() {
        Invoke("AnimateEntrance", 2f + introDelay);
    }

    public void Tap() {
        Player p = GameManager.instance.GetPlayer(0);// TODO must be closest
        if (p.CloseEnough(transform)) {
            if (FilledEnough() || GameManager.instance.DEBUG) {
                if (!alreadyPushed) {
                    EmptyShelfWithAnimation();
                    p.AnimButton();
                    AudioManager.Play("button_push");
                    AudioManager.Play("shelf_moving");
                }
            }
            else {
                ComicBubble.instance.Speak(SpeechType.ButtonNotFull);
            }
        }
        else {
            ComicBubble.instance.Speak(SpeechType.FarAway);
        }
    }

    private void EmptyShelfWithAnimation() {
        alreadyPushed = true;
        AnimateShelf();
        AnimateSlidingDoor();
        Invoke("CleanShelf", animTime + animDelay/2);
        Invoke("AlreadyPushedOff", 2 * animTime + animDelay);
    }

    void CheckBlinking() {
        bool blinking = FilledEnough();
        if (blinking) {
            bool currentlyOn = Mathf.RoundToInt(Time.time) % 2 == 0;
            buttonLight.SetActive(currentlyOn);
        }
        else {
            buttonLight.SetActive(false);
        }
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
        iTween.MoveBy(deposit.gameObject, iTween.Hash("amount", +moveAmount * Vector3.forward, "time", animTime, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime + animDelay));
    }

    void AnimateSlidingDoor() {
        iTween.MoveBy(slidingDoor, iTween.Hash("amount", +moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine));
        iTween.MoveBy(slidingDoor, iTween.Hash("amount", -moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime/2));
        iTween.MoveBy(slidingDoor, iTween.Hash("amount", +moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime + animDelay));
        iTween.MoveBy(slidingDoor, iTween.Hash("amount", -moveAmount * Vector3.right, "time", animTime/2, "easeType", iTween.EaseType.easeInOutSine, "delay", animTime + animDelay + animTime / 2));
    }

    void AlreadyPushedOff() { alreadyPushed = false; }


    // queries
    bool FilledEnough() {
        return deposit.PercentFilled() >= percentToBeFilled;
    }


    // other

}