// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

////////// DESCRIPTION //////////

public class GameManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public


    // private
    int score;
    const int scoreMaxDifficulty = 100;


	// references
    public static GameManager instance;


    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        Playing = true;
	}
	
	void Update () {
        
	}



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void StartRound() {
    }

    public void GameOver() {
        //StartCoroutine(GameoverRoutine());
        SceneManager.LoadScene("main");
    }

    public void Win() {
    }



    // queries
    public float ProgressPercent() {
        return (float)score / scoreMaxDifficulty;
    }

    public bool Playing { get; set; }



    // other
   

}