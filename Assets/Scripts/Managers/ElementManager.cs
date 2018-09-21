// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

////////// DESCRIPTION //////////

public class ElementManager : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    [Range(1, 4)]
    public int numPlayers = 1;


    // private


    // references
    public static ElementManager instance;
    public Material normalMat, highlightMat;
    Player[] players;


    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
        FindPlayers();

    }

    void Start() {
        if (GameManager.Menu) UpdateActivePlayers();
    }

    void Update() {

    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    void FindPlayers() {
        Player[] p = FindObjectsOfType<Player>();
        players = p.OrderBy(c => c.id).ToArray();
    }

    void UpdateActivePlayers() {
        int nump = Mathf.Min(4, players.Length);
        for (int i = 0; i < nump; i++) {
            players[i].gameObject.SetActive(numPlayers > i);
        }
    }
    public void ToggleNumPlayers() {
        numPlayers++;
        if (numPlayers == 5) numPlayers = 1;
        InterfaceManager.instance.UpdatePlayerNumberUI(numPlayers);
    }


    // queries
    public Player GetPlayer(int id) {
        if (players == null || id >= players.Length)
            FindPlayers();
        if (players == null || id >= players.Length)
            return null;
        return players[id];
    }

    public static int NumPlayers { get { return instance.players.Length; } }
    public static bool Single { get { return NumPlayers == 1; } }


    // other

}