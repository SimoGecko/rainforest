// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class HighScores : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public Highscore[] highscores;

    // private
    const string privateCode = "TYeLGz9gOUy4ejx_Dy7ACA_FHLzK6VyUyMbCRH2ShcZg";
    const string publicCode = "5b70851c191a8b0bccbf6efc";
    const string webURL = "http://dreamlo.com/lb/";


	// references
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        UploadHighscore("Simone", 90);
        //UploadHighscore("Mary", 80);
        DownloadHighscores();
    }
	
	void Update () {
        
	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands

    public void UploadHighscore(string username, int score) {
        StartCoroutine(UploadNewHighscore(username, score));
    }

    public void DownloadHighscores() {
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
    }


    // queries



    // other
    IEnumerator UploadNewHighscore(string username, int score) {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error)) {
            Debug.Log("upload successful");
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