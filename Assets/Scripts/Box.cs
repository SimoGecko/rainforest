// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

[RequireComponent(typeof(Rigidbody))]
public class Box : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public int size; // either 1, 2, 4

    // private
    Vector3 conveyorVelocity;
    Vector3 vel;
    bool pickedUp;
    [HideInInspector]
    public List<int> positions;
    bool triggered = false;

    // references
    Rigidbody rb;
    public GameObject shatterEffect;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void LateUpdate () {
        if (!pickedUp && !Deposited) {
            conveyorVelocity = conveyorVelocity.normalized * Conveyor.GetSpeed();
            vel = Vector3.Lerp(vel, conveyorVelocity, Time.deltaTime * 5);
            transform.position += vel * Time.deltaTime;
            conveyorVelocity = Vector3.zero;
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (!pickedUp && !triggered && other.tag == "ground") {
            Shatter();
            
        }
    }

    private void OnMouseDown() {
        Tap();
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    public void Tap() {
        if (!GameManager.Playing) return;
        if (Player.instance.CloseEnough(transform) && !pickedUp) {//check if close enough
            if (Cart.instance.HasSpaceFor(size)) {
                PickupBox();
                Cart.instance.Pickup(this);
            }
            else {
                ComicBubble.instance.Speak(SpeechType.CartFull);
            }
        }
        else {
            ComicBubble.instance.Speak(SpeechType.FarAway);
        }
    }

    void PickupBox() {
        pickedUp = true;
        StopRB();

        positions = Cart.instance.FreePosition(size);
        transform.parent = Cart.instance.transform;
        transform.position = Cart.instance.TransfFromPos(positions);
        transform.localRotation = Quaternion.identity;

        ComicBubble.instance.Speak(SpeechType.BoxPickup);
        AudioManager.Play("box_drop");
        Player.instance.PushButtonAnim();
    }

    public void DepositBox(Deposit dep) {
        pickedUp = false;
        StopRB();
        //add score
        Deposited = true;
        GameManager.instance.AddScore(size);
        //Destroy(gameObject);
        dep.PositionBox(this);
    }

    void Shatter() {
        ComicBubble.instance.Speak(SpeechType.BoxLost);
        AudioManager.Play("box_crash");
        GameManager.instance.LoseLife();
        triggered = true;
        GameObject shatter = Instantiate(shatterEffect, transform.position, Quaternion.Euler(0, Random.value * 360, 0));
        Destroy(shatter, 30f);
        Destroy(gameObject);
    }

    void StopRB() {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        conveyorVelocity = Vector3.zero;
        vel = Vector3.zero;
    }

    public void SetConveyorSpeed(Vector3 speed) {
        conveyorVelocity += speed;
    }



	// queries
    public bool OnCart { get { return pickedUp; } }
    public bool Deposited { get; set; }



	// other
	
}