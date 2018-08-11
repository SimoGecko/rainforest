// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

////////// DESCRIPTION //////////

public static class Utility {
    // --------------------- VARIABLES ---------------------
    public static bool HasArrived(this NavMeshAgent agent) {
        if (!agent.pathPending) {
            if (agent.remainingDistance <= agent.stoppingDistance) {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) {
                    return true;
                }
            }
        }
        return false;
    }

    public static string ToReadableTime(float t) {
        System.TimeSpan ts = System.TimeSpan.FromSeconds(t);
        string res = String.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
        return res;
    }

    public static Vector3 SmoothDampAngle(Vector3 from, Vector3 to, ref Vector3 r, float smoothTime) {
        return new Vector3(
            Mathf.SmoothDampAngle(from.x, to.x, ref r.x, smoothTime),
            Mathf.SmoothDampAngle(from.y, to.y, ref r.y, smoothTime),
            Mathf.SmoothDampAngle(from.z, to.z, ref r.z, smoothTime)
            );
    }


    // --------------------- MATH ---------------------

    public static float Normal() {
        float result = 0;
        for (int i = 0; i < 12; i++) result += UnityEngine.Random.value;
        return result - 6;
    }

    public static float Normal(float mu, float sigma) {
        return Normal() * sigma + mu;
    }

    public static float NormalFromTo(float a, float b) {
        float cutoff = 3.3f;
        float n01 = Mathf.Clamp01(Normal() / (2 * cutoff) + 1);
        return a + n01 * (b - a);
    }

    public static Vector3 To3(this Vector2 v) {
        return new Vector3(v.x, 0, v.y);
    }

}