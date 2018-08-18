// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////// DESCRIPTION //////////

[RequireComponent(typeof(Rigidbody))]
public class Box : MonoBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public int packSize; // either 1, 2, 4

    // private
    Vector3 conveyorVelocity;
    Vector3 vel;
    bool pickedup;
    bool deposited;
    bool shattered;

    List<int> positions;//which positions it's taking up on the cart

    // references
    Rigidbody rb;
    public GameObject shatterEffect;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void LateUpdate () {
        if (!pickedup && !deposited) {
            MoveOnConveyor();
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (!pickedup && !shattered && other.tag == "ground") {
            Shatter();
        }
    }

    private void OnMouseDown() {
        Tap();
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void MoveOnConveyor() {
        conveyorVelocity = conveyorVelocity.normalized * Conveyor.GetSpeed();
        vel = Vector3.Lerp(vel, conveyorVelocity, Time.deltaTime * 5);
        transform.position += vel * Time.deltaTime;
        conveyorVelocity = Vector3.zero;
    }

    public void Tap() {
        if (!GameManager.Playing || pickedup || deposited) return;

        if (Player.instance.CloseEnough(transform)) {//check if close enough
            if (Cart.instance.HasSpaceFor(packSize)) {
                //PickupBox();
                Cart.instance.Pickup(this); // TODO refactor
            }
            else {
                ComicBubble.instance.Speak(SpeechType.CartFull);
            }
        }
        else {
            ComicBubble.instance.Speak(SpeechType.FarAway);
        }
    }

    public void PickupBox() { // TODO add which cart is pickedup to
        pickedup = true;
        StopRB();

        Cart cart = Cart.instance;

        positions = cart.FreePosition(packSize);
        transform.parent = cart.transform;
        transform.position = cart.TransfFromPos(positions);
        transform.localRotation = Quaternion.identity;

        ComicBubble.instance.Speak(SpeechType.BoxPickup);
        AudioManager.Play("box_drop"); // TODO audio pickup
        Player.instance.AnimButton();
    }

    public void DepositBox(Deposit dep) {
        pickedup = false;
        deposited = true;
        StopRB();
        //add score
        GameManager.instance.AddScore(packSize);
        dep.PositionBox(this);
    }

    void Shatter() {
        ComicBubble.instance.Speak(SpeechType.BoxLost);
        AudioManager.Play("box_crash");
        GameManager.instance.LoseLife();

        shattered = true;
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
    public bool OnCart { get { return pickedup; } }
    public bool Deposited { get { return deposited; } }
    public List<int> Positions { get { return positions; } }


	// other
	
}