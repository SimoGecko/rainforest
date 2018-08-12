// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

////////// DESCRIPTION //////////

public class GameManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------
    public enum Difficulty { Easy, Medium, Hard}
    // public
    public bool loseLifes = true;
    public float timeScale = 1f;
    public Difficulty difficulty = Difficulty.Medium;

    // private
    int score;
    int lifes;
    const int scoreMaxDifficulty = 100;
    float timer;


    // references
    public Text scoreText;
    public Text lifeText;
    public Text timerText;
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
        timer += Time.deltaTime;
        UpdateUI();
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
        scoreText.text = "score:    x " + score.ToString();
        timerText.text = "timer: " + Utility.ToReadableTime(timer);// timer.ToString();
        lifeText.text  = "lives: " + lifes.ToString();
    }

    public void LoseLife() {
        if(loseLifes)
            lifes--;
        if (lifes == 0) GameOver();
        UpdateUI();
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