// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

////////// DESCRIPTION //////////

public class GameManager : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------
    public enum State { Menu, Playing, Pause, Gameover }
    public enum Platform { Pc, Mobile, Console}
    public enum Mode { Coop, Compet }
    public enum Multiplayer { Single, Local, Online}


    // public
    [Header("Debug")]
    public bool DEBUG;
    public bool invincible = false;
    public bool autoStart = false;
    public bool spawnFromBeginning = false; // if true ignores game state
    public float timeScale = 1f;

    
    public event System.Action OnPlay;
    public event System.Action OnGameover;
    public event System.Action OnPause;
    public event System.Action OnResume;

    // private
    public State state;
    Platform platform;
    Mode mode;


    // references
    public static GameManager instance;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        state = State.Menu;
        platform = Platform.Pc;
        mode = Mode.Compet;
	}
	
	void Update () {
        if (Menu) {
            if (autoStart) {
                autoStart = false;
                Invoke("Play", .2f);// Play();
            }
        }

        if (Playing) {
            if (InputManager.instance.PauseInput()) Pause();
        }

        if (Pausing) {
            if (InputManager.instance.ResumeInput()) Resume();
            if (InputManager.instance.RestartInput()) Restart();
        }

        if (Gameover) {
            if (InputManager.instance.OverRestartInput()) Restart();
        }

        //temporary
        if (DEBUG) {
            Time.timeScale = timeScale;
            if (Input.GetKeyDown(KeyCode.G)) GameOver();
            if (Input.GetKeyDown(KeyCode.U)) ScreenCapture.CaptureScreenshot("screenshot.png");
        }
    }

    



    // --------------------- CUSTOM METHODS ----------------


    // commands
    public void Play() {
        if (OnPlay != null) OnPlay();
        state = State.Playing;
    }

    public void Pause() {
        if(OnPause!=null) OnPause();
        state = State.Pause;
    }

    public void Resume() {
        if (OnResume != null) OnResume();
        state = State.Playing;
    }

    public void GameOver() {
        if (OnGameover != null) OnGameover();
        state = State.Gameover;
    }

    public void Restart() {
        SceneManager.LoadScene("SampleScene");
    }


    // queries
    public static bool Mobile  { get { return instance.platform == Platform.Mobile; } }
    public static bool Console { get { return instance.platform == Platform.Console || instance.platform ==Platform.Pc; } }
    public static bool Pc      { get { return instance.platform == Platform.Pc; } }

    public static bool Menu    { get { return instance.state == State.Menu; } }
    public static bool Playing { get { return instance.state == State.Playing; } }
    public static bool Pausing { get { return instance.state == State.Pause; } }
    public static bool Gameover{ get { return instance.state == State.Gameover; } }

    public static bool Cooperative { get { return instance.mode == Mode.Coop; } }
    public static bool Competitive { get { return instance.mode == Mode.Compet; } }




    // other


}