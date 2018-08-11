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
    Vector3 ref1;

    // references
    public Transform target;


    // --------------------- BASE METHODS ------------------
    void Start () {
        offset = transform.position - target.position;

    }

    void Update () {
        
	}

    void LateUpdate() {
        FollowTarget();
    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    void FollowTarget() {
        Vector3 targetPos = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref ref1, smoothTime);
    }


    // queries



    // other

}