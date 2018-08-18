// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

////////// DESCRIPTION //////////

public class GameManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------
    public enum Difficulty { Easy, Medium, Hard }
    public enum State { Menu, Playing, Gameover }
    public enum Platform { Pc, Mobile, Console}


    // public
    public bool DEBUG = false;
    public bool Mobile = false;
    public float timeScale = 1f;

    public Difficulty difficulty = Difficulty.Medium;
    const int scoreMaxDifficulty = 100;

    public System.Action OnPlay;
    public System.Action OnGameover;

    // private
    State state;

    int score;
    int lifes;
    float timer;


    // references
    public static GameManager instance;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
        //RuntimePlatform platform = Application.platform;
    }

    void Start () {
        state = State.Menu;
	}
	
	void Update () {
        if (DEBUG) {
            if (Input.GetKeyDown(KeyCode.G)) GameOver();
            Time.timeScale = timeScale;
        }

        if (Playing) {
            timer += Time.deltaTime;
        }
	}



    // --------------------- CUSTOM METHODS ----------------


    // commands
    public void StartRound(int diff) { // called from gauge
        if(OnPlay!=null) OnPlay();
        state = State.Playing;

        score = 0;
        timer = 0;
        lifes = 3;
        difficulty = (Difficulty)diff;
        //SHOW GAME UI
        InterfaceManager.instance.ShowGameUI();
    }

    void GameOver() {
        if(OnGameover!=null) OnGameover();
        state = State.Gameover;

        ComicBubble.instance.Speak(SpeechType.GameOver);
        HighScores.instance.SubmitRandomUsername();
        Invoke("GameOverDelay", 3f);
    }

    void GameOverDelay() {
        InterfaceManager.instance.ShowGameoverUI();
    }

    public void Restart() { //called from button
        SceneManager.LoadScene("SampleScene");
    }
    
    public void AddScore(int s) {
        score += s;
        InterfaceManager.instance.UpdateScoreUI();
    }

    public void LoseLife() {
        if (!DEBUG) {
            lifes--;
            if (lifes == 0) GameOver();
            InterfaceManager.instance.UpdateLifeUI(lifes);
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
    public int Lifes { get { return lifes; } }

    // other


}