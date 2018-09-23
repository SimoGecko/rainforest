// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

////////// DESCRIPTION //////////


public class PlayerInteraction : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------

    // public
    public float pickupDist = 8f;


    // private
    List<GameObject> interactablesInRange = new List<GameObject>();
    GameObject closest;

    // references
    Player player;
    public Transform pickupCenter;


    // --------------------- BASE METHODS ------------------
    void Start () {
        //if (!isLocalPlayer) Destroy(this);

        player = GetComponent<Player>();
	}
	
	void Update () {
        if (!GameManager.Playing) return;

        SetHighlightColor(false);
        closest = ClosestInteractable(); // must be done everywhere
        SetHighlightColor(true);

        if (isLocalPlayer) {
            if (InputManager.instance.GetInteractInput(player.InputId)) {
                if (closest != null)
                    CmdInteract();
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        IInteractable iint = other.GetComponent<IInteractable>();
        if (iint != null && !interactablesInRange.Contains(other.gameObject)) {
            interactablesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        IInteractable iint = other.GetComponent<IInteractable>();
        if (iint != null && interactablesInRange.Contains(other.gameObject)) {
            interactablesInRange.Remove(other.gameObject);
        }
    }



    // --------------------- CUSTOM METHODS ----------------


    // commands
    [Command]
    void CmdInteract() {
        //do I need the reference for this?
        closest.GetComponent<IInteractable>().Interact(player);
    }

    void SetHighlightColor(bool highlight) { // works locally
        if (!isLocalPlayer) return;
        if (closest != null) closest.GetComponentInChildren<MeshRenderer>().material = highlight ? ElementManager.instance.highlightMat : ElementManager.instance.normalMat;
    }


    //queries
    GameObject ClosestInteractable() {
        interactablesInRange.RemoveAll(g => g == null);
        //exclude the non interactable temporarily
        List<GameObject> filtered = interactablesInRange.Where(g => g.GetComponent<IInteractable>().CanInteract()).ToList();

        if (filtered.Count > 0) {
            return filtered.Aggregate((x, y) => DistToMe(x.transform) < DistToMe(y.transform) ? x : y);
        }
        return null;
    }

    float DistToMe(Transform t) {
        return Vector3.SqrMagnitude(t.position - transform.position);
    }

    public bool CloseEnough(Transform t) {
        Vector3 v = pickupCenter.position - t.transform.position;
        v.y = 0;
        return Vector3.SqrMagnitude(v) <= pickupDist * pickupDist;
    }

    // other

}