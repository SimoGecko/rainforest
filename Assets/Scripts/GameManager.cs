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
    public float timeScale = 1f;


    // private
    int score;
    int lifes;
    const int scoreMaxDifficulty = 100;


    // references
    public Text scoreText;
    public Text lifeText;
    public Text gameOverText;
    public static GameManager instance;


    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        Time.timeScale = timeScale;
        Playing = true;
        lifes = 3;
        UpdateUI();
	}
	
	void Update () {
        
	}



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void StartRound() {
    }

    public void GameOver() {
        StartCoroutine(GameoverRoutine());
    }

    public void Win() {
    }

    public void AddScore(int s) {
        score += s;
        UpdateUI();
    }

    void UpdateUI() {
        scoreText.text = "score: " + score.ToString();
        lifeText.text  = "lives: " + lifes.ToString();
    }

    public void LoseLife() {
        Debug.Log("called loselife");
        lifes--;
        UpdateUI();
        if (lifes == 0) GameOver();
    }


    // queries
    public float ProgressPercent() {
        return (float)score / scoreMaxDifficulty;
    }

    public bool Playing { get; set; }



    // other
    IEnumerator GameoverRoutine() {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "GameOver! score:" + score.ToString();
        Playing = false;
        yield return new WaitForSeconds(2f);

        //SceneManager.LoadScene("SampleScene");
    }



}