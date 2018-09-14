// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

////////// DESCRIPTION //////////

public class GameManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------
    public enum State { Menu, Playing, Pause, Gameover }
    public enum Platform { Pc, Mobile, Console}
    public enum Difficulty { Easy, Medium, Hard }
    public enum Mode { Coop, Compet }


    // public
    public Platform platform = Platform.Pc;
    public Mode mode;

    public bool AutoStart = false;
    public bool DEBUG = false;
    [Range(1, 4)]
    public int numPlayers = 1;

    public float timeScale = 1f;
    public float eachPlayerDifficultyMultiplier = 1.5f; // 1, 1.5, 2.25, 3.375
    public float[] difficultyMultipliers = new float[] { .9f, 1.3f, 1.7f };

    const int scoreMaxDifficulty = 100;

    public event System.Action OnPlay;
    public event System.Action OnGameover;

    // private
    State state;
    [HideInInspector]
    public Difficulty difficulty = Difficulty.Medium;
    Player[] players;

    int score;
    int lifes;
    float timer;


    // references
    public static GameManager instance;
    public Material normalMat, highlightMat;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
        //RuntimePlatform platform = Application.platform;
        FindPlayers();
    }

    void Start () {
        state = State.Menu;
        if (AutoStart) Invoke("StartDebug", .4f);
	}
	
	void Update () {
        if (Menu) {
            UpdateActivePlayers();
        }


        if (DEBUG) {
            if (Input.GetKeyDown(KeyCode.G)) GameOver();
            Time.timeScale = timeScale;
            if (Input.GetKeyDown(KeyCode.U)) ScreenCapture.CaptureScreenshot("screenshot.png");
        }

        CheckPause();

        


        if (Playing) {
            timer += Time.deltaTime;
        }
	}

    public void ToggleNumPlayers() {
        numPlayers++;
        if (numPlayers == 5) numPlayers = 1;
        InterfaceManager.instance.UpdatePlayerNumberUI(numPlayers);
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void FindPlayers() {
        Player[] p = FindObjectsOfType<Player>();
        players = p.OrderBy(c => c.id).ToArray();
    }

    void UpdateActivePlayers() {
        //players[1].gameObject.SetActive(Coop);
        for (int i = 0; i < 4; i++) {
            players[i].gameObject.SetActive(numPlayers > i);
        }
    }
    /*
    void TogglePause() {
        if (state == State.Playing) SetPause(true);
        else if (state == State.Pause) SetPause(false);
    }*/

    void CheckPause() {
        if (Playing) {
            if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Escape))
                SetPause(true);
        }
        else if (Pause) {
            //resume
            if (Input.GetKeyDown("joystick button 7") || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown("joystick button 0")) {
                SetPause(false);
            }
            //restart
            if (Input.GetKeyDown("joystick button 6") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 1")) {
                Restart();
            }
        }
    }

    void SetPause(bool b) {
        state = b ? State.Pause : State.Playing;
        InterfaceManager.instance.SetPauseUI(b);
    }

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

        ComicBubble.AllSpeak(SpeechType.GameOver);
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

    public Player GetPlayer(int id) {
        return players[id];
    }

    public bool Mobile { get { return platform == Platform.Mobile; } }
    public bool Console { get { return platform == Platform.Console || platform==Platform.Pc; } }
    public bool Pc { get { return platform == Platform.Pc; } }

    public float DifficultyMult() {
        float multiplePlayersMultiplier = Mathf.Pow(eachPlayerDifficultyMultiplier, numPlayers - 1);
        return difficultyMultipliers[(int)difficulty] * multiplePlayersMultiplier;//(Coop ? coopDifficultyMultiplier : 1);
    }

    public bool Single { get { return numPlayers == 1; } }

    public static bool Playing { get { return instance.state == State.Playing; } }
    public static bool Menu    { get { return instance.state == State.Menu; } }
    public static bool Gameover{ get { return instance.state == State.Gameover; } }
    public static bool Pause   { get { return instance.state == State.Pause; } }

    public int Score { get { return score; } }
    public int Timer { get { return Mathf.RoundToInt(timer); } }
    public int Lifes { get { return lifes; } }

    // other


}