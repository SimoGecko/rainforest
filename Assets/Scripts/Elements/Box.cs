// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

////////// DESCRIPTION //////////

[RequireComponent(typeof(Rigidbody))]
public class Box : NetworkBehaviour, IInteractable {
    // --------------------- VARIABLES ---------------------

    // public
    public int packSize; // either 1, 2, 4

    // private
    Vector3 conveyorVelocity;
    Vector3 vel;

    public bool OnCart { get; private set; }
    public bool Deposited { get; private set; }

    List<int> positions;//which positions it's taking up on the cart

    // references
    Rigidbody rb;
    Cart carryingCart;
	
	
	// --------------------- BASE METHODS ------------------
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	void LateUpdate () {
        if (!isServer) return;

        if (OnConveyor) MoveOnConveyor();
	}

    private void OnTriggerEnter(Collider other) {
        if (!isServer) return;

        if (OnConveyor && other.tag == "ground") {
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

    public void Interact(Player player) {
        if (!GameManager.Playing || !CanInteract()) return;

        Cart cart = player.Cart;

        if (!player.CloseEnough(transform)) {
            player.Bubble.Speak(SpeechType.FarAway);
            return;
        }
        if (!cart.HasSpaceFor(packSize)) {
            player.Bubble.Speak(SpeechType.CartFull);
            return;
        }

        //actions (called from client)
        carryingCart = cart;
        CmdPickupBox();
        cart.Pickup(this);
    }

    [Command]
    void CmdPickupBox() {
        OnCart = true;
        positions = carryingCart.FreePosition(packSize);

        StopRB();
        transform.parent = carryingCart.transform;
        transform.position = carryingCart.TransfFromPos(positions);
        transform.localRotation = Quaternion.identity;

        carryingCart.Owner.Bubble.Speak(SpeechType.BoxPickup);
        AudioManager.Play("box_drop"); // TODO audio pickup
        carryingCart.Owner.AnimButton();
    }

    public void DepositBox(Deposit dep) {
        OnCart = false;
        Deposited = true;
        StopRB();
        //add score
        ScoreManager.instance.AddScore(carryingCart.Owner.id, packSize);
        //add to individual player
        //carryingCart.Owner.AddScore(packSize);
        dep.PositionBox(this);
    }

    void Shatter() {
        ComicBubble.AllSpeak(SpeechType.BoxLost);
        AudioManager.Play("box_crash");
        ScoreManager.instance.LoseLife();

        GameObject shatter = Instantiate(ElementManager.instance.shatterEffect, transform.position, Quaternion.Euler(0, Random.value * 360, 0));
        NetworkServer.Spawn(shatter);
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

    public void AddConveyorSpeed(Vector3 speed) {
        if (!isServer) return;

        conveyorVelocity += speed;
    }



	// queries


    public List<int> Positions { get { return positions; } }
    public bool CanInteract() { return !OnCart && !Deposited; }
    public bool OnConveyor { get { return !OnCart && !Deposited; } }

    // other

}