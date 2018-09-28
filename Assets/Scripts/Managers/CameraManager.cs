// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////////// DESCRIPTION //////////

public class CameraManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float smoothTime = 1f;
    public float transitionTime = 2f;
    public float followPlayerWeight = .5f;

    // private
    Vector3 offset;
    Vector3 ref1;
    bool follow;
    //Vector3 eulerOffset;

    // references
    public Transform[] targets;
    public Transform gameView, titleView;


    // --------------------- BASE METHODS ------------------
    void Start () {
        targets = new Transform[4];
        for (int i = 0; i < 4; i++) {
            if(ElementManager.instance.GetPlayer(i)!=null)
                targets[i] = ElementManager.instance.GetPlayer(i).transform;
        }

        offset = gameView.position;// - targets[0].position;
        transform.position = titleView.position;
        transform.rotation = titleView.rotation;

        GameManager.instance.EventOnPlay += TransitionToGameView;
    }

    void Update () {
        
	}

    void LateUpdate() {
        if (GameManager.Playing && follow) {
            FollowTarget();
        }
    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    void TransitionToGameView() {
        iTween.MoveTo  (gameObject, iTween.Hash("position", gameView, "time", transitionTime, "easeType", iTween.EaseType.easeInOutSine));
        iTween.RotateTo(gameObject, iTween.Hash("rotation", gameView, "time", transitionTime, "easeType", iTween.EaseType.easeInOutSine));
        Invoke("FollowTrue", transitionTime);
    }

    void FollowTarget() {
        Vector3 targetPos = TargetPos() + offset;
        //TODO add multiple targets each with a weight and blend them

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref ref1, smoothTime);
    }

    void FollowTrue() { follow = true; }

    


    // queries
    Vector3 TargetPos() {
        Vector3 result = Vector3.zero;
        int numPlayers = ElementManager.NumPlayers;
        for (int i = 0; i < numPlayers; i++) {
            if(targets[i]!=null)
                result += targets[i].position * followPlayerWeight / numPlayers;
        }
        return result;
        /*
        if (!GameManager.instance.Coop) {
            return target.position * followPlayerWeight;//Vector3.Lerp(Vector3.zero, target.position, followPlayerWeight);
        } else {
            return (target.position + target1.position) * followPlayerWeight/2;
        }*/
    }



    // other
    

}