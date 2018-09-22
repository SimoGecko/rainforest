// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

////////// DESCRIPTION //////////

public class SpawnManager : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------

    // public

    [Header("Box Spawn Ratio")]
    public float spawnRatioBase = .5f; // this is a good ratio, balanced
    const float var = .6f;
    public AnimationCurve spawnRatioCurve; // how many per second

    [Header("Conveyor speed")]
    public float conveyorSpeedBase = 1f;
    public AnimationCurve conveyorSpeedCurve; // multiplier increase

    // private
    readonly int[] numPreboxes = new int[] { 4, 6, 8 }; // how many preboxes per difficulty


    // references
    public static SpawnManager instance;
    [Header("Prefabs")]
    public Box[] boxesPrefab;
    public Kappa[] kappas;
    public Transform preboxParent;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
        //if (!isServer) Destroy(this);
    }

    void Start () {
        if (isServer) {
            GameManager.instance.OnPlay += SpawnPreboxes;
            StartCoroutine("SpawnRoutine");
        }
    }

    void Update () {
        
	}



    // --------------------- CUSTOM METHODS ----------------


    // commands
    [Command]
    public void CmdSpawnBoxAt(Vector3 p) {
        Box newBox = Instantiate(GetBoxPrefab(), p, Quaternion.Euler(0, Random.value * 360, 0)) as Box;
        NetworkServer.Spawn(newBox.gameObject);
    }

    void TrySpawnOne() {
        int k = Random.Range(0, kappas.Length);
        kappas[k].StartFalling();
    }

    void SpawnPreboxes() {
        int[] permutation = Utility.RandomPermutation(preboxParent.childCount);
        int difficultyToSpawn = numPreboxes[(int)ScoreManager.instance.difficulty];
        int numToSpawn = Mathf.Min(preboxParent.childCount, difficultyToSpawn);

        for (int i = 0; i < numToSpawn; i++) {
            CmdSpawnBoxAt(preboxParent.GetChild(permutation[i]).position);
            //Instantiate(GetBoxPrefab(), preboxParent.GetChild(permutation[i]).position, Quaternion.Euler(0, Random.value * 360, 0));
        }
    }

    // queries
    float WaitTime() {
        float difficultyMultiplier = ScoreManager.instance.DifficultyMult(); //spawnDifficultyMultipliers[(int)GameManager.instance.difficulty];
        float avg = spawnRatioCurve.Evaluate(ScoreManager.instance.ProgressPercent()) * spawnRatioBase * difficultyMultiplier; // how many per second
        float val = Utility.NormalFromTo(1 - var, 1 + var) * avg; // how many per second with some variance
        float result = 1f / val; // how many seconds waittime
        //Debug.Log("waittime=" + result);
        return result;
    }

    public Box GetBoxPrefab() {
        return boxesPrefab[Random.Range(0, boxesPrefab.Length)];
    }

    public float GetConveyorSpeed() {
        float difficultyMultiplier = ScoreManager.instance.DifficultyMult();//conveyorDifficultyMultipliers[(int)GameManager.instance.difficulty];
        return  conveyorSpeedCurve.Evaluate(ScoreManager.instance.ProgressPercent()) * conveyorSpeedBase * difficultyMultiplier; // how many per second
    }

    


    // other
    IEnumerator SpawnRoutine() {
        while (true) {
            if (!GameManager.Playing && !GameManager.instance.spawnFromBeginning) {
                yield return new WaitForSeconds(1);
            }
            else {
                TrySpawnOne();
                yield return new WaitForSeconds(WaitTime());
            }
        }
    }

}