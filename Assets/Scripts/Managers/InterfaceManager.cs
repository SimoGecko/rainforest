// (c) Simone Guggiari 2018

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;


////////// DESCRIPTION //////////

public class InterfaceManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float animTime = 2f;


    // private
    bool inTutorial;

    // references
    [Header("TitleUI")]
    public GameObject title;
    public GameObject subtitle;
    public TextMeshPro playersText;

    [Header("GameUI")]
    public GameObject gameUI;
    public GameObject mobileUI;
    public GameObject score;
    public Text scoreText;
    public GameObject[] playerScore;
    public Text[] playerScoreText;

    //public Text timerText;
    public GameObject[] lifeUI;
    public GameObject[] lifeUIgrey;

    [Header("PauseUI")]
    public GameObject pauseUI;

    [Header("OverUI")]
    public GameObject gameoverUI;
    public GameObject scoreOver;
    public Text scoreOverText;
    public GameObject[] playerScoreOver;
    public Text[] playerScoreOverText;
    public Text timerOverText;
    BlurOptimized blur;

    [Header("LeaderboardUI")]
    public Text leadUserText;
    public Text leadScoreText;
    public Text leadTimeText;
    public Text submit;

    [Header("TutorialUI")]
    public GameObject tutorialUI;


    public static InterfaceManager instance;


    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        AnimateTitle();
        HighScores.instance.OnDownloadedScores += UpdateLeaderboardUI;
        blur = FindObjectOfType<BlurOptimized>();


        GameManager.instance.EventOnPlay += () => { SetupNumPlayersUI(); ShowGameUI(); } ;
        GameManager.instance.EventOnPause += () => SetPauseUI(true);
        GameManager.instance.EventOnResume += () => SetPauseUI(false);
        GameManager.instance.EventOnGameover += () => Invoke("ShowGameoverUI", 3f);
    }
	
	void Update () {

	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands
    void SetupNumPlayersUI() {
        if(GameManager.Competitive) {
            score.SetActive(false);
            scoreOver.SetActive(false);
            for (int i = 0; i < ElementManager.NumPlayers; i++) {
                playerScore[i].SetActive(true);
                playerScoreOver[i].SetActive(true);
            }
        }
    }

    public void UpdatePlayerNumberUI(int nump) {
        playersText.text = "players: " + nump;
    }

    void AnimateTitle() {
        iTween.MoveFrom(title,    iTween.Hash("position", title.transform.position + title.transform.right * 35, "time", animTime, "easeType", iTween.EaseType.easeInOutSine));
        iTween.FadeFrom(subtitle, iTween.Hash("alpha", 0f, "time", animTime, "delay", 2f));
    }
    
    public void UpdateScoreUI(int score) {
        scoreText.text = score.ToString();
    }

    public void UpdatePlayerScoreUI(int id, int score) {
        playerScoreText[id].text = score.ToString();
        playerScoreOverText[id].text = score.ToString();
    }

    public void UpdateLifeUI(int lifes) {
        for (int i = 0; i < 3; i++) {
            bool hasIthLife = lifes >= 3 - i;
            lifeUI[i].SetActive(!hasIthLife);
            lifeUIgrey[i].SetActive(hasIthLife);
        }
    }

    public void SetPauseUI(bool b) {
        blur.enabled = b;
        pauseUI.SetActive(b);
    }

    public void ToggleTutorial(bool b) {
        inTutorial = b;
        tutorialUI.SetActive(inTutorial);
        blur.enabled = inTutorial;
    }

    void UpdateLeaderboardUI() {
        leadUserText.text = HighScores.instance.UserString();
        leadScoreText.text = HighScores.instance.ScoreString();
        leadTimeText.text = HighScores.instance.TimeString();
    }


    public void ShowGameUI() {
        UpdateScoreUI(0);
        UpdateLifeUI(3);
        gameUI.SetActive(true);
        if (GameManager.Mobile) mobileUI.SetActive(true);
    }

    public void ShowGameoverUI() {
        blur.enabled = true;
        gameUI.SetActive(false);
        mobileUI.SetActive(false);
        gameoverUI.SetActive(true);
        scoreOverText.text = ScoreManager.instance.Score.ToString();
        timerOverText.text = Utility.ToReadableTime(ScoreManager.instance.Timer);
    }


    public void ChangeTextToDone() {
        submit.text = "done!";
    }


    // queries
    public bool InTutorial { get { return inTutorial; } }

    

    // other

}