// (c) Simone Guggiari 2018

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

////////// DESCRIPTION //////////


public class PlayerPickup : NetworkBehaviour {
    // --------------------- VARIABLES ---------------------

    // public


    // private
    List<GameObject> interactablesInRange = new List<GameObject>();
    GameObject closest;

    // references
    Player player;


    // --------------------- BASE METHODS ------------------
    void Start () {
        if (!isLocalPlayer) Destroy(this);

        player = GetComponent<Player>();
	}
	
	void Update () {
        if (!GameManager.Playing) return;

        SetHighlightColor(false);
        closest = ClosestInteractable();
        SetHighlightColor(true);

        if (InputManager.instance.GetInteractInput(player.InputId)) {
            if(closest!=null)
                closest.GetComponent<IInteractable>().Interact(player);
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
    void SetHighlightColor(bool highlight) { // works locally
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


    // other

}