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
    public int localNumPlayers = 1;

    public Rect playArea;


    // private
    List<Player> players;

    [SyncVar(hook= "OnNextID")]
    public int nextID = 0;


    // references
    public static ElementManager instance;

    //const
    public Material normalMat, highlightMat;
    public GameObject shatterEffect;
    public Texture2D[] playerTextures;


    // --------------------- BASE METHODS ------------------
    private void Awake() {
        instance = this;
        //FindPlayers();

    }

    void Start() {
        //if (GameManager.Menu) UpdateActivePlayers();
    }

    void Update() {

    }


    // --------------------- CUSTOM METHODS ----------------


    // commands
    void OnNextID(int id) {
        FindPlayers();
    }


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

    public void AddPlayer(Player p) {
        players.Add(p);
    }


    // queries
    public Player GetPlayer(int id) {
        if (players == null || id >= players.Count)
            FindPlayers();
        if (players == null || id >= players.Count)
            return null;
        return players[id];
    }

    public static int NumPlayers { get { return instance.players.Count; } }
    public static bool Single { get { return NumPlayers == 1; } }


    // other
    private void OnDrawGizmos() {
        Gizmos.color = Color.Lerp(Color.blue, Color.red, .5f);
        Gizmos.DrawWireCube(playArea.center.To3(), playArea.size.To3());
    }

}