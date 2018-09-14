// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

////////// DESCRIPTION //////////

public class HighScores : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public bool useHighscores = true;
    public int numLeadToDisplay = 20;

    const string privateCode = "TYeLGz9gOUy4ejx_Dy7ACA_FHLzK6VyUyMbCRH2ShcZg";
    const string publicCode = "5b70851c191a8b0bccbf6efc";
    const string webURL = "https://www.dreamlo.com/lb/";

    // private
    Highscore[] highscores;

    string username;
    bool alreadySubmitted;

    public System.Action OnDownloadedScores;

    // references
    public InputField inputUsername;
    public static HighScores instance;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        string randomUser = "user_" + Random.Range(0, 2048);
        username = PlayerPrefs.GetString("user", randomUser);
        if (!HasRandomUsername()) {
            inputUsername.text = username;
        }
        DownloadHighscores();
    }
	
	void Update () {
        
	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands
    public void Submit() { // called from button
        if (!alreadySubmitted) {
            //if(HasRandomUsername()) // delete random old
                //RemoveHighscore(username);

            username = inputUsername.text;
            if(string.IsNullOrEmpty(username)) username = "user_" + Random.Range(0, 2048); // ATTENTION: if username is empty, then other entries are shifted by one
            PlayerPrefs.SetString("user", username);

            UploadHighscore(username, GameManager.instance.Score, GameManager.instance.Timer);
            alreadySubmitted = true;
        }
    }
    

    public void SubmitRandomUsername() { // called at end by default
        //UploadHighscore(username, GameManager.instance.Score, GameManager.instance.Timer);
    }

    //---------------------------
    void UploadHighscore(string username, int score, int time) {
        if(useHighscores)
            StartCoroutine(UploadNewHighscore(username, score, time));
    }

    void RemoveHighscore(string username) {
        if(useHighscores)
            StartCoroutine(RemoveSingleHighscore(username));
    }

    void DownloadHighscores() {
        if(useHighscores)
            StartCoroutine(DownloadHighscoresFromDatabase());
    }

    //---------------------------
    void FormatHighscores(string textStream) {
        string[] entries = textStream.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        highscores = new Highscore[entries.Length];

        for (int i = 0; i < entries.Length; i++) {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            highscores[i] = new Highscore(entryInfo[0], int.Parse(entryInfo[1]), int.Parse(entryInfo[2]));
        }

        if (OnDownloadedScores != null) OnDownloadedScores();
        
    }




    // queries
    bool HasRandomUsername() {
        return username.Substring(0, 5).Equals("user_");
    }

    public string UserString() {
        string result = "";
        for (int i = 0; i < Mathf.Min(numLeadToDisplay, highscores.Length); i++) {
            result += (i + 1) + ". " + highscores[i].username + "\n";
        }
        return result;
    }
    public string ScoreString() {
        string result = "";
        for (int i = 0; i < Mathf.Min(numLeadToDisplay, highscores.Length); i++) {
            result += highscores[i].score.ToString() + "\n";
        }
        return result;
    }
    public string TimeString() {
        string result = "";
        for (int i = 0; i < Mathf.Min(numLeadToDisplay, highscores.Length); i++) {
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
            //DOWNLOAD TO SYNC
            //DownloadHighscores();
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

    public struct Highscore {
        public string username;
        public int score;
        public int time;

        public Highscore(string u, int s, int t) { username = u; score = s; time = t; }
    }
}

