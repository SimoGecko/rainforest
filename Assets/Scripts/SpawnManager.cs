// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class SpawnManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float spawnRatioMultiplier = 1f;
    public float var = .4f;
    public AnimationCurve spawnRatioCurve; // how many per second

    // private


    // references
    public static SpawnManager instance;
    public Box[] boxesPrefab;
    public Transform[] spawnPoints;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        StartCoroutine("SpawnRoutine");
    }

    void Update () {
        
	}



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void SpawnOne() {
        int lane = Random.Range(0, spawnPoints.Length);
        Transform t = spawnPoints[lane];
        Box box = Instantiate(boxesPrefab[Random.Range(0, boxesPrefab.Length)], t.position, Quaternion.Euler(0, Random.value*380, 0)) as Box;
        //set it up
    }


    // queries
    float WaitTime() {
        float avg = spawnRatioCurve.Evaluate(GameManager.instance.ProgressPercent()) * spawnRatioMultiplier; // how many per second
        float val = Utility.NormalFromTo(1 - var, 1 + var) * avg; // how many per second with some variance
        float result = 1f / val; // how many seconds waittime
        //Debug.Log("waittime=" + result);
        return result;
    }



    // other
    IEnumerator SpawnRoutine() {
        while (true) {
            if (!GameManager.instance.Playing) {
                yield return new WaitForSeconds(1);
            }
            else {
                SpawnOne();
                yield return new WaitForSeconds(WaitTime());
            }
        }
    }

}