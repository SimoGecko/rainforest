// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;

////////// DESCRIPTION //////////

public class GameManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------
    public bool DEBUG = false;
    public bool mobile = false;

    public enum Difficulty { Easy, Medium, Hard }
    public enum State { Menu, Playing, Gameover }


    // public
    public float timeScale = 1f;
    public Difficulty difficulty = Difficulty.Medium;
    const int scoreMaxDifficulty = 100;

    // private
    State state;
    int score;
    int lifes;
    float timer;


    // references
    [Header("game")]
    public GameObject gameUI;
    public GameObject mobileUI;
    public Text scoreText;
    public Text timerText;
    public GameObject[] lifeUI;
    public GameObject[] lifeUIgrey;

    [Header("gameover")]
    public GameObject gameoverUI;
    public Text scoreOverText;
    public Text timerOverText;
    public BlurOptimized blur;
    //blur
    

    public static GameManager instance;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        state = State.Menu;
        Time.timeScale = timeScale;
	}
	
	void Update () {
        if (DEBUG && Input.GetKeyDown(KeyCode.G)) GameOver();
        if(DEBUG) Time.timeScale = timeScale;

        if (Playing) {
            timer += Time.deltaTime;
            UpdateUI();
        }
        else if (Menu) {
            //if (Input.GetMouseButton(0)) StartRound();
        }
	}



    // --------------------- CUSTOM METHODS ----------------


    // commands
    public void StartRound(int d) { // called from gauge
        state = State.Playing;
        score = 0;
        lifes = 3;
        timer = 0;
        difficulty = (Difficulty)d;
        UpdateUI();
        UpdateLifeUI();
        gameUI.SetActive(true);
        if(mobile) mobileUI.SetActive(true);
    }

    void GameOver() {
        ComicBubble.instance.Speak(SpeechType.BoxLost);
        state = State.Gameover;


        HighScores.instance.SubmitRandomUsername();
        Invoke("GameOverDelay", 3f);
        //trigger animation...
       
        //iTween.FadeFrom(gameoverUI, iTween.Hash("alpha", 0f, "time", 2f, "delay", 2f));
        //blur
        //StartCoroutine(FadeRoutine());
    }

    void GameOverDelay() {
        blur.enabled = true;
        gameUI.SetActive(false);
        if (mobile) mobileUI.SetActive(false);
        gameoverUI.SetActive(true);
        scoreOverText.text = score.ToString();
        timerOverText.text = Utility.ToReadableTime(timer);
    }

    public void Restart() { //called from button
        SceneManager.LoadScene("SampleScene");
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
        if (!DEBUG) {
            lifes--;
            if (lifes == 0) GameOver();
            UpdateLifeUI();
        }
    }


    // queries
    public float ProgressPercent() {
        return (float)score / scoreMaxDifficulty;
    }


    public static bool Playing { get { return instance.state == State.Playing; } }
    public static bool Menu    { get { return instance.state == State.Menu; } }
    public static bool Gameover{ get { return instance.state == State.Gameover; } }

    public int Score { get { return score; } }
    public int Timer { get { return Mathf.RoundToInt(timer); } }

    // other
    IEnumerator FadeRoutine() {
        blur.enabled = true;
        float percent = 0;
        float speed = .5f;
        while (percent < 1) {
            percent += Time.deltaTime * speed;
            blur.blurSize = Mathf.Lerp(0, 3, percent);
            //blur.blurIterations = Mathf.RoundToInt(Mathf.Lerp(0, 3, percent));
            yield return null;
        }
    }



}