// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

////////// DESCRIPTION //////////

[RequireComponent(typeof(Rigidbody))]
public class Box : NetworkBehaviour {
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
    Cart carryingCart;
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
    



    // --------------------- CUSTOM METHODS ----------------


    // commands
    void MoveOnConveyor() {
        conveyorVelocity = conveyorVelocity.normalized * Conveyor.GetSpeed();
        vel = Vector3.Lerp(vel, conveyorVelocity, Time.deltaTime * 5);
        transform.position += vel * Time.deltaTime;
        conveyorVelocity = Vector3.zero;
    }

    public void Tap(Player player) { //TODO refactor
        if (!GameManager.Playing || pickedup || deposited) return;

        Cart cart = player.GetCart();

        if (player.CloseEnough(transform)) {//check if close enough
            if (cart.HasSpaceFor(packSize)) {
                //PickupBox();
                cart.Pickup(this); // TODO refactor
            }
            else {
                player.Bubble.Speak(SpeechType.CartFull);
            }
        }
        else {
            player.Bubble.Speak(SpeechType.FarAway);
        }
    }

    public void PickupBox(Cart cart) { // TODO add which cart is pickedup to
        pickedup = true;
        StopRB();
        carryingCart = cart;
        //Cart cart = Cart.instance;

        positions = cart.FreePosition(packSize);
        transform.parent = cart.transform;
        transform.position = cart.TransfFromPos(positions);
        transform.localRotation = Quaternion.identity;

        cart.Owner.Bubble.Speak(SpeechType.BoxPickup);
        AudioManager.Play("box_drop"); // TODO audio pickup
        cart.Owner.AnimButton();
    }

    public void DepositBox(Deposit dep) {
        pickedup = false;
        deposited = true;
        StopRB();
        //add score
        ScoreManager.instance.AddScore(packSize);
        //add to individual player
        carryingCart.Owner.AddScore(packSize);
        dep.PositionBox(this);
    }

    void Shatter() {
        ComicBubble.AllSpeak(SpeechType.BoxLost);
        AudioManager.Play("box_crash");
        ScoreManager.instance.LoseLife();

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