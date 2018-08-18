// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class SpawnManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    [Header("Box Spawn Ratio")]
    public float spawnRatioMultiplier = .5f; // this is a good ratio, balanced
    public float var = .6f;
    public AnimationCurve spawnRatioCurve; // how many per second
    public float[] spawnDifficultyMultipliers = new float[] { .65f, 1f, 1.35f };

    [Header("Conveyor speed")]
    public float conveyorBaseSpeed = 1f;
    public AnimationCurve conveyorSpeedCurve; // multiplier increase
    public float[] conveyorDifficultyMultipliers = new float[] { .65f, 1f, 1.35f };

    // private


    // references
    public static SpawnManager instance;
    [Header("Prefabs")]
    public Box[] boxesPrefab;
    public Kappa[] kappas;
    public Transform preboxParent;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        StartCoroutine("SpawnRoutine");
        GameManager.instance.OnPlay += SpawnPreboxes;
    }

    void Update () {
        
	}



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void SpawnOne() {
        int k = Random.Range(0, kappas.Length);
        kappas[k].StartFalling();
    }

    void SpawnPreboxes() {
        for (int i = 0; i < preboxParent.childCount; i++) {
            Instantiate(GetBoxPrefab(), preboxParent.GetChild(i).position, Quaternion.Euler(0, Random.value * 360, 0));
        }
    }

    // queries
    float WaitTime() {
        float difficultyMultiplier = spawnDifficultyMultipliers[(int)GameManager.instance.difficulty];
        float avg = spawnRatioCurve.Evaluate(GameManager.instance.ProgressPercent()) * spawnRatioMultiplier * difficultyMultiplier; // how many per second
        float val = Utility.NormalFromTo(1 - var, 1 + var) * avg; // how many per second with some variance
        float result = 1f / val; // how many seconds waittime
        //Debug.Log("waittime=" + result);
        return result;
    }

    public Box GetBoxPrefab() {
        return boxesPrefab[Random.Range(0, boxesPrefab.Length)];
    }

    public float GetConveyorSpeed() {
        float difficultyMultiplier = conveyorDifficultyMultipliers[(int)GameManager.instance.difficulty];
        return  conveyorSpeedCurve.Evaluate(GameManager.instance.ProgressPercent()) * conveyorBaseSpeed * difficultyMultiplier; // how many per second
    }

    


    // other
    IEnumerator SpawnRoutine() {
        while (true) {
            if (!GameManager.Playing) {
                yield return new WaitForSeconds(1);
            }
            else {
                SpawnOne();
                yield return new WaitForSeconds(WaitTime());
            }
        }
    }

}