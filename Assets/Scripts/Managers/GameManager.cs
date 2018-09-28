// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

////////// DESCRIPTION //////////

public class GameManager : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------
    public delegate void GameEvent();

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

    
    [SyncEvent] public event GameEvent EventOnPlay;
    [SyncEvent] public event GameEvent EventOnGameover;
    [SyncEvent] public event GameEvent EventOnPause;
    [SyncEvent] public event GameEvent EventOnResume;

    // private
    [SyncVar]public State state;

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
            if (autoStart && isServer) {
                autoStart = false;
                Invoke("CmdPlay", .2f);// Play();
            }
        }

        if (Playing) {
            if (InputManager.instance.PauseInput()) CmdPause();
        }

        if (Pausing) {
            if (InputManager.instance.ResumeInput()) CmdResume();
            if (InputManager.instance.RestartInput()) CmdRestart();
        }

        if (Gameover) {
            if (InputManager.instance.OverRestartInput()) CmdRestart();
        }

        //temporary
        if (DEBUG) {
            Time.timeScale = timeScale;
            if (Input.GetKeyDown(KeyCode.G)) CmdGameOver();
            if (Input.GetKeyDown(KeyCode.U)) ScreenCapture.CaptureScreenshot("screenshot.png");
        }
    }

    



    // --------------------- CUSTOM METHODS ----------------


    // commands
    [Command]
    public void CmdPlay() {
        state = State.Playing;
        if (EventOnPlay != null) EventOnPlay();
    }
    [ClientRpc]
    void RpcPlay() { Play(); }

    void Play() {
        if (EventOnPlay != null) EventOnPlay();
        state = State.Playing;
    }

    [Command]
    public void CmdPause() { Pause();  RpcPause(); }
    [ClientRpc]
    void RpcPause() { Pause(); }

    void Pause() { 
        if(EventOnPause!=null) EventOnPause();
        state = State.Pause;
    }

    [Command]
    public void CmdResume() { Resume();  RpcResume(); }
    [ClientRpc]
    void RpcResume() { Resume(); }

    void Resume() {
        if (EventOnResume != null) EventOnResume();
        state = State.Playing;
    }

    [Command]
    public void CmdGameOver() { GameOver(); RpcGameOver(); }
    [ClientRpc]
    void RpcGameOver() { GameOver(); }

    void GameOver() {
        if (EventOnGameover != null) EventOnGameover();
        state = State.Gameover;
    }

    [Command]
    public void CmdRestart() { Restart();  RpcRestart(); }
    [ClientRpc]
    void RpcRestart() { Restart(); }

    void Restart() {
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