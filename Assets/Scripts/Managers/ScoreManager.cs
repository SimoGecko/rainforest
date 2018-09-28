// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


////////// DESCRIPTION //////////

public class ScoreManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------
    public enum Difficulty { Easy, Medium, Hard }

    // public
    public float eachPlayerDifficultyMultiplier = 1.5f; // 1, 1.5, 2.25, 3.375
    public float[] difficultyMultipliers = new float[] { .9f, 1.3f, 1.7f };

    const int scoreForMaxDifficulty = 100;
    [HideInInspector]
    public Difficulty difficulty = Difficulty.Medium;

    // private
    int score;
    List<int> playerScore;
    int lives;
    float timer;

    // references
    public static ScoreManager instance;


    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start() {
        score = 0;
        timer = 0;
        lives = 3;
        playerScore = new List<int>() { 0, 0, 0, 0 }; //4p
    }

    void Update() {
        if (GameManager.Playing)  timer += Time.deltaTime;
    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    public void SetDifficulty(int d) {
        difficulty = (Difficulty)d;
    }

    public void AddScore(int s, int id) {
        score += s;
        playerScore[id] += s;
        InterfaceManager.instance.UpdateScoreUI(score);
        InterfaceManager.instance.UpdatePlayerScoreUI(id, playerScore[id]);
    }


    public void LoseLife() {
        if (!GameManager.instance.invincible) {
            lives--;
            if (lives == 0) GameManager.instance.GameOver();
            InterfaceManager.instance.UpdateLifeUI(lives);
        }
    }


    // queries
    public int Score { get { return score; } }
    public int Timer { get { return Mathf.RoundToInt(timer); } }
    public int Lifes { get { return lives; } }
    public int PlayerScore(int id) { return playerScore[id]; }

    public float ProgressPercent() {
        return (float)score / scoreForMaxDifficulty;
    }

    public float DifficultyMult() {
        float multiplePlayersMultiplier = Mathf.Pow(eachPlayerDifficultyMultiplier, ElementManager.NumPlayers - 1);
        return difficultyMultipliers[(int)difficulty] * multiplePlayersMultiplier;//(Coop ? coopDifficultyMultiplier : 1);
    }

    // other

}