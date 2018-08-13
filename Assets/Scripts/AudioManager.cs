// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

public class AudioManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public AudioClip[] audioClips;
    public AudioClip gameTheme; //menuTheme

    // private
    [Range(0, 1)]
    public float masterVolume = 1;
    [Range(0, 1)]
    public float sfxVolume = 1;
    [Range(0, 1)]
    public float speechVolume = 1;
    [Range(0, 1)]
    public float musicVolume = 1;

    AudioSource effectSource, musicSource, speechSource;
    Dictionary<string, AudioClip> groupDictionary = new Dictionary<string, AudioClip>();

    // references
    public static AudioManager instance;
    Transform audioListener;
    Transform playerT;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
            instance = this;
    }

    void Start () {
        audioListener = FindObjectOfType<AudioListener>().transform;
        playerT = FindObjectOfType<Player>().transform;

        CreateSources();
        CreateDictionary();

        //playmusic
    }
	
	void Update () {
        //if (playerT != null)
            //audioListener.position = playerT.position;
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void CreateSources() {
        //music
        GameObject newMusicSource = new GameObject("Music source");
        musicSource = newMusicSource.AddComponent<AudioSource>();
        newMusicSource.transform.parent = transform;

        //effects
        GameObject newEffectsSource = new GameObject("Effects source");
        effectSource = newEffectsSource.AddComponent<AudioSource>();
        newEffectsSource.transform.parent = transform;

        //speech
        GameObject newSpeechSource = new GameObject("Speech source");
        speechSource = newSpeechSource.AddComponent<AudioSource>();
        newSpeechSource.transform.parent = transform;
        speechSource.pitch = 1.2f;
    }

    void CreateDictionary() {
        foreach (var a in audioClips) {
            groupDictionary.Add(a.name, a);
        }
    }

    void PlaySound2D(string clipName) {
        float pitchVar = .3f;
        effectSource.pitch = Random.Range(1 - pitchVar, 1 + pitchVar);
        effectSource.PlayOneShot(GetClipFromName(clipName), sfxVolume * masterVolume);
    }

    public static void Play(string clipName) {
        instance.PlaySound2D(clipName);
    }

    public void PlaySpeech(string clipName) {
        speechSource.PlayOneShot(GetClipFromName(clipName), speechVolume * masterVolume);
    }



    // queries
    AudioClip GetClipFromName(string name) {
        if (groupDictionary.ContainsKey(name)) {
            return groupDictionary[name];
            //return clips[Random.Range(0, clips.Length)];
        }
        else {
            Debug.LogError("no sound named " + name);
            return null;
        }
    }



    // other
    /*
    [System.Serializable]
    public class NamedClip {
        public string name;
        public AudioClip clip;
    }*/

}