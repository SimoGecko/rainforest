﻿// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

////////// DESCRIPTION //////////
public enum SpeechType {
    FarAway, CartFull, ShelfFull, BoxLost, GameOver,
    Random, Begin, ButtonNotFull, BoxPickup, BoxDeposit,
    NotRightFit }; // missing: random, begin

public class ComicBubble : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    const int numSpeechType = 11;
    //public Speech[] speech;
    public Vector2 speechDurationMinMax = new Vector2(4, 6);
    float[] probOfSpeech = new float[] {
        .5f, .6f, 1f, 1f, 1f,
        .2f, .3f, .6f, .1f, .1f,
        .8f };


    // private
    Dictionary<SpeechType, string[]> speechDic;
    bool speaking;

    // references
    public TextMeshPro bubbleText;
    public GameObject bubble;
    public TextAsset speechText;
    public static ComicBubble instance;


    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        //CreateDictionary();
        ParseSpeechText();
	}
	
	void Update () {
        transform.LookAt(Camera.main.transform);
	}

	
	
	// --------------------- CUSTOM METHODS ----------------
	
	
	// commands
    /*
    void CreateDictionary() {
        speechDic = new Dictionary<SpeechType, string[]>();
        
        foreach (var s in speech) {
            speechDic.Add(s.speechType, s.text);
        }
    }*/

    void ParseSpeechText() {
        List<string>[] result = new List<string>[numSpeechType];

        string text = speechText.text;
        string[] lines = text.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        int index = -1;
        //read
        for (int i = 0; i < lines.Length; i++) {
            if (lines[i][0] == '-') {
                index++; // another type
                result[index] = new List<string>();
            }
            else {
                result[index].Add(lines[i].Substring(1));
            }
        }

        //insert
        speechDic = new Dictionary<SpeechType, string[]>();
        for (int i = 0; i < numSpeechType; i++) {
            speechDic.Add((SpeechType)i, result[i].ToArray());
        }
    }

    public void Speak(SpeechType speechType) {
        if (!speaking && Random.value<probOfSpeech[(int)speechType]) { // no override allowed
            speaking = true;
            bubbleText.text = GetSpeech(speechType);
            bubble.SetActive(true);
            Invoke("EndSpeak", Random.Range(speechDurationMinMax.x, speechDurationMinMax.y));
        }
    }

    void EndSpeak() {
        speaking = false;
        bubble.SetActive(false);
    }



	// queries
    string GetSpeech(SpeechType speechType) {
        if (speechDic.ContainsKey(speechType)) {
            string[] text = speechDic[speechType];
            return text[Random.Range(0, text.Length)];
        }
        else {
            Debug.LogError("no text found");
            return "...";
        }
    }



	// other
    /*
    [System.Serializable]
    public class Speech {
        public SpeechType speechType;
        public string[] text;
    }*/
	
}