// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

[RequireComponent(typeof(Animator))]
public class Kappa : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float spawnDelay = .1f;


    // private
    bool spawning;


    // references
    Animator anim;
	
	
	// --------------------- BASE METHODS ------------------
	void Awake () {
        anim = GetComponent<Animator>();
    }
	
	void Update () {
        
	}



    // --------------------- CUSTOM METHODS ----------------


    // commands
    public void StartFalling() {
        if (spawning) return;
        spawning = true;
        anim.SetTrigger("fallout");
        Invoke("SpawnBox", spawnDelay);
    }

    public void SpawnBox() {
        //actual spawn code
        Box boxPrefab = SpawnManager.instance.GetBoxPrefab();
        Box box = Instantiate(boxPrefab, transform.position, Quaternion.Euler(0, Random.value * 360, 0)) as Box;
        AudioManager.Play("box_fall");
        spawning = false;
    }



    // queries



    // other

}