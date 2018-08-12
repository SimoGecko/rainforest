// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

////////// DESCRIPTION //////////

public class GameManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------
    public enum Difficulty { Easy, Medium, Hard }
    public enum State { Menu, Playing, Gameover }

    // public
    public bool loseLifes = true;
    public float timeScale = 1f;
    public Difficulty difficulty = Difficulty.Medium;
    public State state;
    const int scoreMaxDifficulty = 100;

    // private
    int score;
    int lifes;
    float timer;


    // references
    public Text scoreText;
    public GameObject[] lifeUI;
    public GameObject[] lifeUIgrey;
    public Text timerText;
    public Text gameOverText;
    public static GameManager instance;
    public GameObject gameUI;


    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        state = State.Menu;
        Time.timeScale = timeScale;
	}
	
	void Update () {
        if (Playing) {
            timer += Time.deltaTime;
            UpdateUI();
        }
        else if (Menu) {
            if (Input.GetMouseButton(0))
                StartRound();
        }
	}



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void StartRound() {
        state = State.Playing;
        score = 0;
        lifes = 3;
        timer = 0;
        UpdateUI();
        UpdateLifeUI();
        gameUI.SetActive(true);
    }

    public void GameOver() {
        StartCoroutine(GameoverRoutine());
    }

    public void Win() {
    }

    public void AddScore(int s) {
        score += s;
    }

    void UpdateUI() {
        scoreText.text = score.ToString();
        timerText.text = "timer: " + Utility.ToReadableTime(timer);// timer.ToString();
    }

    void UpdateLifeUI() {
        for (int i = 0; i < 3; i++) {
            bool hasIthLife = lifes>=3-i;
            lifeUI[i].SetActive(!hasIthLife);
            lifeUIgrey[i].SetActive(hasIthLife);
        }
    }

    public void LoseLife() {
        if(loseLifes)
            lifes--;
        if (lifes == 0) GameOver();
        UpdateLifeUI();
    }


    // queries
    public float ProgressPercent() {
        return (float)score / scoreMaxDifficulty;
    }


    public static bool Playing { get { return instance.state == State.Playing; } }
    public static bool Menu    { get { return instance.state == State.Menu; } }
    public static bool Gameover{ get { return instance.state == State.Gameover; } }


    // other
    IEnumerator GameoverRoutine() {
        gameOverText.gameObject.SetActive(true);
        gameOverText.text = "GameOver! score:" + score.ToString();
        state = State.Gameover;
        yield return new WaitForSeconds(2f);

        //SceneManager.LoadScene("SampleScene");
    }



}