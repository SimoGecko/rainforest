// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class SpawnManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float spawnRatioMultiplier = .5f; // this is a good ratio, balanced
    public float spawnRatioMultiplierSaver = .5f; //to store the good value when experimenting
    public float var = .6f;
    public AnimationCurve spawnRatioCurve; // how many per second
    public float[] difficultyMultipliers = new float[] { .65f, 1f, 1.35f };

    // private


    // references
    public static SpawnManager instance;
    public Box[] boxesPrefab;
    public Kappa[] kappas;

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
        int k = Random.Range(0, kappas.Length);
        //Transform t = spawnPoints[lane];
        kappas[k].StartFalling();
        //Box box = Instantiate(boxesPrefab[Random.Range(0, boxesPrefab.Length)], t.position, Quaternion.Euler(0, Random.value*380, 0)) as Box;
        //set it up
    }


    // queries
    float WaitTime() {
        float difficultyMultiplier = difficultyMultipliers[(int)GameManager.instance.difficulty];
        float avg = spawnRatioCurve.Evaluate(GameManager.instance.ProgressPercent()) * spawnRatioMultiplier * difficultyMultiplier; // how many per second
        float val = Utility.NormalFromTo(1 - var, 1 + var) * avg; // how many per second with some variance
        float result = 1f / val; // how many seconds waittime
        //Debug.Log("waittime=" + result);
        return result;
    }

    public Box GetBoxPrefab() {
        return boxesPrefab[Random.Range(0, boxesPrefab.Length)];
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