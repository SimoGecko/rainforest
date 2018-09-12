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
        GameManager.instance.OnPlay += SetupNumPlayersUI;
    }
	
	void Update () {

	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands
    void SetupNumPlayersUI() {
        if(GameManager.instance.mode == GameManager.Mode.Compet) {
            score.SetActive(false);
            scoreOver.SetActive(false);
            for (int i = 0; i < GameManager.instance.numPlayers; i++) {
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
    
    public void UpdateScoreUI() {
        scoreText.text = Score.ToString();
        //timerText.text = "timer: " + Utility.ToReadableTime(Timer);
    }

    public void UpdatePlayerScoreUI(int id, int score) {
        playerScoreText[id].text = score.ToString();
    }

    public void UpdateLifeUI(int lifes) {
        for (int i = 0; i < 3; i++) {
            bool hasIthLife = lifes >= 3 - i;
            lifeUI[i].SetActive(!hasIthLife);
            lifeUIgrey[i].SetActive(hasIthLife);
        }
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
        UpdateScoreUI();
        UpdateLifeUI(3);
        gameUI.SetActive(true);
        if (GameManager.instance.Mobile) mobileUI.SetActive(true);
    }

    public void ShowGameoverUI() {
        blur.enabled = true;
        gameUI.SetActive(false);
        mobileUI.SetActive(false);
        gameoverUI.SetActive(true);
        scoreOverText.text = Score.ToString();
        timerOverText.text = Utility.ToReadableTime(Timer);
    }


    public void ChangeTextToDone() {
        submit.text = "done!";
    }


    // queries
    public int Score { get { return GameManager.instance.Score; } }
    public int Timer { get { return GameManager.instance.Timer; } }

    public bool InTutorial { get { return inTutorial; } }

    

    // other

}