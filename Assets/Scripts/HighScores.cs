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
    public int numLeadToDisplay = 20;

    // private
    const string privateCode = "TYeLGz9gOUy4ejx_Dy7ACA_FHLzK6VyUyMbCRH2ShcZg";
    const string publicCode = "5b70851c191a8b0bccbf6efc";
    const string webURL = "http://dreamlo.com/lb/";
    string username;

    bool alreadySubmitted;

    // references
    public static HighScores instance;
    public Text leadUserText, leadScoreText, leadTimeText;
    public InputField inputUsername;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        string randomUser = "user_" + Random.Range(0, 2048);
        username = PlayerPrefs.GetString("user", randomUser);

        //UploadHighscore("Simone", 90, 35);
        //UploadHighscore("Mary", 80, 155);

        DownloadHighscores();
    }
	
	void Update () {
        
	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands
    public void Submit() { // called from button
        if (!alreadySubmitted) {
            /*if(username!=inputUsername.text) // delete old
                RemoveHighscore(username);*/

            username = inputUsername.text;
            PlayerPrefs.SetString("user", username);

            UploadHighscore(username, GameManager.instance.Score, GameManager.instance.Timer);
            alreadySubmitted = true;
        }
    }

    public void SubmitRandomUsername() { // called at end by default
        UploadHighscore(username, GameManager.instance.Score, GameManager.instance.Timer);
    }


    void UploadHighscore(string username, int score, int time) {
        StartCoroutine(UploadNewHighscore(username, score, time));
    }

    void DownloadHighscores() {
        StartCoroutine(DownloadHighscoresFromDatabase());
    }

    void RemoveHighscore(string username) {
        StartCoroutine(RemoveSingleHighscore(username));
    }

    void FormatHighscores(string textStream) {
        string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        highscores = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++) {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            highscores[i] = new Highscore(entryInfo[0], int.Parse(entryInfo[1]), int.Parse(entryInfo[2]));
            //Debug.Log(highscores[i].username + "," + highscores[i].score);
        }

        //apply them
        leadUserText.text = UserString(numLeadToDisplay);
        leadScoreText.text = ScoreString(numLeadToDisplay);
        leadTimeText.text = TimeString(numLeadToDisplay);
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
    string TimeString(int num) {
        string result = "";
        for (int i = 0; i < Mathf.Min(num, highscores.Length); i++) {
            result += Utility.ToReadableTime(highscores[i].time) + "\n";
        }
        return result;
    }


    // other
    IEnumerator UploadNewHighscore(string username, int score, int time) {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score + "/" + time);
        yield return www;

        if (string.IsNullOrEmpty(www.error)) {
            //Debug.Log("upload successful");
            //DOWNLOAD TO SYNC
            DownloadHighscores();
        }
        else {
            Debug.LogError("Error uploading: " + www.error);
        }
    }

    IEnumerator RemoveSingleHighscore(string username) {
        WWW www = new WWW(webURL + privateCode + "/delete/" + WWW.EscapeURL(username));
        yield return www;

        if (string.IsNullOrEmpty(www.error)) {
            //Debug.Log("remove successful");
        }
        else {
            Debug.LogError("Error removing score: " + www.error);
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
    public int time;

    public Highscore(string u, int s, int t) { username = u; score = s; time = t; }
}