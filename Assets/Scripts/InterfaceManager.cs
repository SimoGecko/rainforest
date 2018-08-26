﻿// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
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

    [Header("GameUI")]
    public GameObject gameUI;
    public GameObject mobileUI;
    public Text scoreText;
    //public Text timerText;
    public GameObject[] lifeUI;
    public GameObject[] lifeUIgrey;

    [Header("OverUI")]
    public GameObject gameoverUI;
    public Text scoreOverText;
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
    }
	
	void Update () {

	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands
    void AnimateTitle() {
        iTween.MoveFrom(title,    iTween.Hash("position", title.transform.position + title.transform.right * 35, "time", animTime, "easeType", iTween.EaseType.easeInOutSine));
        iTween.FadeFrom(subtitle, iTween.Hash("alpha", 0f, "time", animTime, "delay", 2f));
    }
    
    public void UpdateScoreUI() {
        scoreText.text = Score.ToString();
        //timerText.text = "timer: " + Utility.ToReadableTime(Timer);
    }

    public void UpdateLifeUI(int lifes) {
        for (int i = 0; i < 3; i++) {
            bool hasIthLife = lifes >= 3 - i;
            lifeUI[i].SetActive(!hasIthLife);
            lifeUIgrey[i].SetActive(hasIthLife);
        }
    }

    public void ToggleTutorial() {
        inTutorial = !inTutorial;
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