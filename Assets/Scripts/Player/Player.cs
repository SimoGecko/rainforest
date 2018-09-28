// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

////////// player movement and animation //////////

[RequireComponent(typeof(CharacterController))]
public class Player : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public int id; // localid, netid


    // private
    float animHelloTime;


    // references
    Animator anim;
    NetworkAnimator netanim;
    AudioSource feetSound;


    public Cart Cart { get; private set; }
    public ComicBubble Bubble { get; private set; }

    public Vector3 MoveDir { get; private set; }
    public bool Running { get; private set; }

    [HideInInspector]
    public PlayerMovement pM;
    [HideInInspector]
    public PlayerInteraction pI;

    // --------------------- BASE METHODS ------------------

    void Start () {
        //set id
        id = ElementManager.instance.nextID++;
        ElementManager.instance.FindPlayers();
        SetPlayerColor(id);

        Cart = GetComponentInChildren<Cart>();
        Bubble = GetComponentInChildren<ComicBubble>();

        pM = GetComponent<PlayerMovement>();
        pI = GetComponent<PlayerInteraction>();
        feetSound = GetComponent<AudioSource>();

        if (!isLocalPlayer) return;//Destroy(this);

        anim = GetComponent<Animator>();
        netanim = GetComponent<NetworkAnimator>();


        GameManager.instance.EventOnPlay += AnimStart;
        GameManager.instance.EventOnGameover += AnimEnd;
        animHelloTime = Time.time + Random.Range(2f, 4f);
    }
	
	void Update () {
        if (isLocalPlayer) {
            //get input
            MoveDir = InputManager.instance.GetInput(InputId).To3().normalized;
            Running = InputManager.instance.GetSprintInput(InputId);
            DealWithAnimations();
        }
        
        DealWithSound();
	}




    // --------------------- CUSTOM METHODS ----------------


    // commands
    void SetPlayerColor(int _id) {
        Texture2D texcolors = ElementManager.instance.playerTextures[id];
        GetComponentInChildren<SkinnedMeshRenderer>().material.mainTexture = texcolors;
        GetComponentInChildren<MeshRenderer>().material.mainTexture = texcolors;
    }

    void DealWithAnimations() {
        if (GameManager.Menu && Time.time > animHelloTime) {
            netanim.SetTrigger("hello");
            animHelloTime = Time.time + Random.Range(4f, 8f);
        }
        if (GameManager.Playing) {
            bool running = MoveDir.magnitude > .1f;
            anim.SetBool("running", running);
        }
    }

    void AnimStart() { netanim.SetTrigger("start"); }
    void AnimEnd()   { netanim.SetTrigger("end"); }
    public void AnimButton() { if(isLocalPlayer) netanim.SetTrigger("button"); }


    void DealWithSound() {
        feetSound.volume = MoveDir.magnitude / 20;
        feetSound.pitch = Mathf.Lerp(feetSound.pitch, Random.Range(.8f, 1.2f), Time.deltaTime / 2);
    }


    // queries
    public int InputId { get { return 0; } } // standard for now



    // other
    
    

}