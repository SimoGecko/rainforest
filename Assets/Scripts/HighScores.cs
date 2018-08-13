// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

////////// DESCRIPTION //////////

public class HighScores : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public Highscore[] highscores;
    public int numLeadToDisplay = 10;

    // private
    const string privateCode = "TYeLGz9gOUy4ejx_Dy7ACA_FHLzK6VyUyMbCRH2ShcZg";
    const string publicCode = "5b70851c191a8b0bccbf6efc";
    const string webURL = "http://dreamlo.com/lb/";

    bool alreadySubmitted;

    // references
    public static HighScores instance;
    public Text leadUserText, leadScoreText;
    public InputField inputUsername;

    // --------------------- BASE METHODS ------------------
    void Start () {
        //UploadHighscore("Simone", 90);
        //UploadHighscore("Mary", 80);
        DownloadHighscores();
    }
	
	void Update () {
        
	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands
    public void Submit() { // called from button
        if (!alreadySubmitted) {
            UploadHighscore(inputUsername.text, GameManager.instance.Score);
            alreadySubmitted = true;
        }
    }


    void UploadHighscore(string username, int score) {
        StartCoroutine(UploadNewHighscore(username, score));
    }

    void DownloadHighscores() {
        StartCoroutine(DownloadHighscoresFromDatabase());
    }

    void FormatHighscores(string textStream) {
        string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        highscores = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++) {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            highscores[i] = new Highscore(entryInfo[0], int.Parse(entryInfo[1]));
            //Debug.Log(highscores[i].username + "," + highscores[i].score);
        }

        //apply them
        leadUserText.text = UserString(numLeadToDisplay);
        leadScoreText.text = ScoreString(numLeadToDisplay);
    }




    // queries
    string UserString(int num) {
        string result = "";
        for (int i = 0; i < Mathf.Min(num, highscores.Length); i++) {
            result += (i + 1) + ". " + highscores[i].username + "\n";
        }
        return result;
    }

    string ScoreString(int num) {
        string result = "";
        for (int i = 0; i < Mathf.Min(num, highscores.Length); i++) {
            result += highscores[i].score.ToString() + "\n";
        }
        return result;
    }


    // other
    IEnumerator UploadNewHighscore(string username, int score) {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error)) {
            Debug.Log("upload successful");
            //DOWNLOAD TO SYNC
            DownloadHighscores();
        }
        else {
            Debug.LogError("Error uploading: " + www.error);
        }
    }

    IEnumerator DownloadHighscoresFromDatabase() {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error)) {
            FormatHighscores(www.text);
        }
        else {
            Debug.LogError("Error downloading: " + www.error);
        }
    }

}

public struct Highscore {
    public string username;
    public int score;

    public Highscore(string u, int s) { username = u; score = s; }
}