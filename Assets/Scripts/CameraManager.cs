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

    // private
    Vector3 offset;
    Vector3 ref1;
    bool inGameView;
    bool follow;
    //Vector3 eulerOffset;

    // references
    public Transform target;
    public Transform gameView;


    // --------------------- BASE METHODS ------------------
    void Start () {
        offset = gameView.position - target.position;
        //eulerOffset = gameView.eulerAngles;
        /*
        post = new GameObject().transform;
        post.position = transform.position;
        post.rotation = transform.rotation;
        post.parent = target;*/

    }

    void Update () {
        
	}

    void LateUpdate() {
        if (GameManager.Playing) {
            if (!inGameView) TransitionToGameView();
            if(follow)
                FollowTarget();
        }
    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    void FollowTarget() {
        Vector3 targetPos = Vector3.Lerp(Vector3.zero, target.position, 0.5f) + offset;
        //Vector3 targetPos2 = target.localToWorldMatrix * offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref ref1, smoothTime);
        //transform.eulerAngles = Utility.SmoothDampAngle(transform.eulerAngles, post.eulerAngles, ref ref2, smoothTime);
        //transform.LookAt(target);
    }

    void FollowTrue() { follow = true; }

    void TransitionToGameView() {
        inGameView = true;
        iTween.MoveTo  (gameObject, iTween.Hash("position", gameView, "time", transitionTime, "easeType", iTween.EaseType.easeInOutSine));
        iTween.RotateTo(gameObject, iTween.Hash("rotation", gameView, "time", transitionTime, "easeType", iTween.EaseType.easeInOutSine));
        Invoke("FollowTrue", transitionTime);
    }


    // queries



    // other
    private void OnDrawGizmos() {
        if (!Application.isPlaying) {
            //edit mode
        }
    }

}