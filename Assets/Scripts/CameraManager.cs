// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class CameraManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float smoothTime = 1f;


    // private
    Vector3 offset;
    Vector3 eulerOffset;
    Vector3 ref1, ref2;

    // references
    public Transform target;
    //Transform post;


    // --------------------- BASE METHODS ------------------
    void Start () {
        offset = transform.position - target.position;
        eulerOffset = transform.eulerAngles;
        /*
        post = new GameObject().transform;
        post.position = transform.position;
        post.rotation = transform.rotation;
        post.parent = target;*/

    }

    void Update () {
        
	}

    void LateUpdate() {
        FollowTarget();
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


    // queries



    // other

}