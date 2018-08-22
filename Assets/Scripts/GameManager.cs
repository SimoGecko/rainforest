// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

////////// DESCRIPTION //////////

public class GameManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------
    public enum State { Menu, Playing, Gameover }
    public enum Platform { Pc, Mobile, Console}
    public enum Difficulty { Easy, Medium, Hard }



    // public
    public Platform platform = Platform.Pc;

    public bool AutoStart = false;
    public bool DEBUG = false;
    public bool Coop = false;

    public float timeScale = 1f;
    public float coopDifficultyMultiplier = 1.5f;
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
            players[1].gameObject.SetActive(Coop);

        }

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
    void FindPlayers() {
        Player[] p = FindObjectsOfType<Player>();
        players = p.OrderBy(c => c.id).ToArray();
    }

    void StartDebug() { StartRound(0); }

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
    public bool Console { get { return platform == Platform.Console; } }
    public bool Pc { get { return platform == Platform.Pc; } }

    public float DifficultyMult() {
        return difficultyMultipliers[(int)difficulty] * (Coop ? coopDifficultyMultiplier : 1);
    }

    public static bool Playing { get { return instance.state == State.Playing; } }
    public static bool Menu    { get { return instance.state == State.Menu; } }
    public static bool Gameover{ get { return instance.state == State.Gameover; } }

    public int Score { get { return score; } }
    public int Timer { get { return Mathf.RoundToInt(timer); } }
    public int Lifes { get { return lifes; } }

    // other


}