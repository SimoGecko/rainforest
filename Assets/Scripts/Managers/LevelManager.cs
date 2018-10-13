// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

////////// DESCRIPTION //////////

public class LevelManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public Level[] levels;

    // private


    // references
    public static LevelManager instance;

    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
    }

    void Start () {
        
	}
	
	void Update () {
        
	}

    // --------------------- CUSTOM METHODS ----------------


    // commands
    public void LoadLevel(int index) {
        Level levelToLoad = GetLevel(index);
        SceneManager.LoadScene("_Main");
        //in new level
        Instantiate(levelToLoad.mapPrefab);
    }


    // queries
    Level GetLevel(int index) {
        foreach(Level l in levels) {
            if (l.index == index) return l;
        }
        return null;
    }



    // other

}

[System.Serializable]
public class Level {
    public string levelName;
    public int index;
    public int[] scorePerStar = new int[] { 100, 200, 300 };
    public GameObject mapPrefab;
}