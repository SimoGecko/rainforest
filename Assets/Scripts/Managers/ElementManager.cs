﻿// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


////////// DESCRIPTION //////////

public class ElementManager : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    [Range(1, 4)]
    public int localNumPlayers = 1;

    public Rect playArea;

    // private
    public List<Player> players;


    // references
    public static ElementManager instance;

    public Material normalMat, highlightMat;
    public GameObject shatterEffect;
    public Texture2D[] playerTextures;


    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
        FindPlayers();
    }

    void Start() {

    }

    void Update() {
        if (GameManager.Menu) UpdateActivePlayers();

    }


    // --------------------- CUSTOM METHODS ----------------


    // commands

    public void FindPlayers() {
        Player[] p = FindObjectsOfType<Player>();
        players = p.OrderBy(c => c.id).ToList();
    }

    void UpdateActivePlayers() {
        int nump = Mathf.Min(4, players.Count);
        for (int i = 0; i < nump; i++) {
            players[i].gameObject.SetActive(localNumPlayers > i);
        }
    }
    public void ToggleNumPlayers() {
        localNumPlayers++;
        if (localNumPlayers == 5) localNumPlayers = 1;
        InterfaceManager.instance.UpdatePlayerNumberUI(localNumPlayers);
    }

    // queries
    public Player GetPlayer(int id) {
        /*if (players == null || id >= players.Count)
            FindPlayers();*/
        if (players == null || id >= players.Count)
            return null;
        return players[id];
    }

    public static int NumPlayers { get { return /*instance.players.Count*/ instance.localNumPlayers; } }
    public static bool Single { get { return NumPlayers == 1; } }


    // other
    private void OnDrawGizmos() {
        Gizmos.color = Color.Lerp(Color.blue, Color.red, .5f);
        Gizmos.DrawWireCube(playArea.center.To3(), playArea.size.To3());
    }

}